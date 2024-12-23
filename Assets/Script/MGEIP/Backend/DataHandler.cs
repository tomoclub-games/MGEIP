using System.Collections;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using System.Runtime.InteropServices;
using System;
using MGEIP.GameData;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace MGIEP.Data
{
    public class DataHandler : MonoBehaviour
    {
        public static DataHandler Instance;

        [SerializeField] private DatabaseAPIContainer databaseAPIContainer;

        [DllImport("__Internal")]
        private static extern System.IntPtr GetURLParameter(string paramName);

        [DllImport("__Internal")]
        private static extern void FreeMemory(System.IntPtr ptr);

        private string loginToken;
        private string metadata = "[SampleMetadata]";
        private int sessionNo;

        private string downloadGameDataURL;
        private string uploadGameDataURL;
        private string uploadPlayerSessionDataURL;

        private PlayerData playerData;
        private AttemptData attemptData;
        private SessionData sessionData;

        public AttemptData AttemptData => attemptData;

        public UnityAction OnLoginRequested;
        public UnityAction<LoginType> OnPlayerLogin;
        public UnityAction<bool> OnSessionDataUploaded;
        public UnityAction<bool> OnDataUploaded;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }

            UpdateDatabaseAPI();
            GetLoginTokenFromQuery();
        }

        public void UpdateDatabaseAPI()
        {
            downloadGameDataURL = databaseAPIContainer.DownloadGameDataURL;
            uploadGameDataURL = databaseAPIContainer.UploadGameDataURL;
            uploadPlayerSessionDataURL = databaseAPIContainer.UploadPlayerSessionDataURL;
        }

        public void LoginPlayer(string _loginToken)
        {
            loginToken = _loginToken;

            LoginPlayer();
        }

        public void LoginPlayer()
        {
            Debug.Log("Trying to login!");

            if (loginToken == null)
            {
                Debug.LogWarning("Error finding parameter 'loginToken'");
                return;
            }

            StartGameDataDownload(loginToken);
        }

        private void OnApplicationQuit()
        {
            UpdateSessionDataOnQuit();
        }

        // Sets up the attemptData and sessionData from DB
        public void StartGameDataDownload(string loginToken)
        {
            if (string.IsNullOrEmpty(downloadGameDataURL) || string.IsNullOrEmpty(uploadGameDataURL) || string.IsNullOrEmpty(uploadPlayerSessionDataURL))
            {
                OnPlayerLogin?.Invoke(LoginType.error);
                Debug.LogError("Invoke URL missing!");
                return;
            }

            StartCoroutine(DownloadGameData(loginToken, (result) =>
            {
                if (result == null)
                {
                    Debug.Log("Failed to download data for player: " + loginToken + " - Unexpected error occurred");

                    attemptData = null;
                    sessionNo = 0;

                    OnPlayerLogin?.Invoke(LoginType.error);

                    return;
                }

                if (result.playerFound)
                {
                    Debug.Log("Data successfully downloaded: " + JsonConvert.SerializeObject(result));

                    if (result.data == null)
                    {
                        // playerData exists, but attempt data doesnt -> New Attempt
                        attemptData = new AttemptData(loginToken);

                        if (sessionData == null || string.IsNullOrEmpty(sessionData.loginToken))
                            sessionNo = result.sessionNo;

                        OnPlayerLogin?.Invoke(LoginType.newAttempt);
                    }
                    else
                    {
                        // playerData exists, attempt data exists -> Check For Repeat Attempt
                        attemptData = result.data;

                        if (sessionData == null || string.IsNullOrEmpty(sessionData.loginToken))
                            sessionNo = result.sessionNo;

                        CheckForRepeatAttempt();
                    }

                    InitializeSessionData();
                }
                else
                {
                    Debug.Log("Failed to download data or player not found for player: " + loginToken + " - Setting up new sessionAttempt 1");

                    attemptData = new AttemptData(loginToken);

                    if (sessionData == null)
                        sessionNo = result.sessionNo;

                    OnPlayerLogin?.Invoke(LoginType.newPlayer);
                }
            }));
        }

        private void CheckForRepeatAttempt()
        {
            bool isAttemptComplete = false;

            for (int i = 0; i < attemptData.completedScenarios.Length; i++)
            {
                if (attemptData.completedScenarios[i] == false)
                {
                    isAttemptComplete = false;
                    break;
                }

                isAttemptComplete = true;
            }

            if (isAttemptComplete)
            {
                AttemptData repeatAttemptData = new AttemptData(attemptData.loginToken);
                repeatAttemptData.attemptNo = attemptData.attemptNo + 1;

                Debug.Log("New sessionAttempt no : " + repeatAttemptData.attemptNo);

                attemptData = repeatAttemptData;

                OnPlayerLogin?.Invoke(LoginType.repeatAttempt);
            }
            else
            {
                Debug.Log("Continue sessionAttempt!");

                OnPlayerLogin?.Invoke(LoginType.continueAttempt);
            }
        }

        private void GetLoginTokenFromQuery()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            // Get the pointer to the URL parameter
            System.IntPtr urlParamPtr1 = GetURLParameter("loginToken");
            System.IntPtr urlParamPtr2 = GetURLParameter("metadata");

            // Check if the pointer is valid
            if (urlParamPtr1 != System.IntPtr.Zero)
            {
                // Convert the pointer to a string
                loginToken = Marshal.PtrToStringAuto(urlParamPtr1);
                Debug.Log("Login Token: " + loginToken);

                // Free the allocated memory on the JavaScript side
                FreeMemory(urlParamPtr1);
            }
            else
            {
                Debug.Log("Login token not found in the URL.");
            }

            // Check if the pointer is valid
            if (urlParamPtr2 != System.IntPtr.Zero)
            {
                // Convert the pointer to a string
                metadata = Marshal.PtrToStringAuto(urlParamPtr2);
                Debug.Log("Meta Data: " + metadata);

                // Free the allocated memory on the JavaScript side
                FreeMemory(urlParamPtr2);
            }
            else
            {
                Debug.Log("Metadata not found in the URL.");
            }
#else
            Debug.Log("Running outside WebGL. No URL parameter.");
#endif
        }

        public void AddCompletedScenario(ScenarioData _scenarioData)
        {
            attemptData.completedScenarios[_scenarioData.scenarioNo - 1] = true;
            attemptData.scenarioList.Add(_scenarioData);

            SessionAttempt sessionAttempt = sessionData.attempts.Find(id => id.attemptNo == attemptData.attemptNo);

            if (sessionAttempt == null)
            {
                sessionAttempt = new SessionAttempt(attemptData.attemptNo);
                sessionData.attempts.Add(sessionAttempt);
            }

            sessionAttempt.completedScenarios.Add(_scenarioData.scenarioNo);

            // Upload attemptData and sessionData
            StartGameDataUpload();
        }

        public void StartGameDataUpload()
        {
            GameData playerData = new GameData(sessionData, attemptData);

            string jsonData = JsonConvert.SerializeObject(playerData, GetPolymorphicSettings());

            StartCoroutine(UploadGameData(jsonData, result =>
            {
                if (result)
                {
                    Debug.Log("Data successfully uploaded.");
                    OnDataUploaded?.Invoke(true);
                }
                else
                {
                    Debug.LogError("Failed to upload data.");
                    OnDataUploaded?.Invoke(false);
                }
            }));
        }

        #region Database access

        // GameData is sessionNo + Latest attemptData
        IEnumerator DownloadGameData(string loginToken, System.Action<ServerResponse<AttemptData>> callback = null)
        {
            string url = downloadGameDataURL + $"?loginToken={loginToken}";

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error: " + request.error);
                    callback?.Invoke(null);
                }
                else
                {
                    string jsonResponse = request.downloadHandler.text;

                    string fixedJson = FixTypeOrderInJson(jsonResponse);

                    Debug.Log("JSON Response DOWNLOAD PLAYER DATA: \n" + fixedJson);

                    var responseObj = JsonConvert.DeserializeObject<ServerResponse<AttemptData>>(fixedJson, GetPolymorphicSettings());

                    callback?.Invoke(responseObj);
                }
            }
        }

        // Game Data is sessionData + attemptData
        IEnumerator UploadGameData(string jsonData, System.Action<bool> callback = null)
        {
            Debug.Log("UploadGameData : ");
            Debug.Log(jsonData);

            using (UnityWebRequest request = new UnityWebRequest(uploadGameDataURL, "POST"))
            {
                request.SetRequestHeader("Content-Type", "application/json");

                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();

                yield return request.SendWebRequest();

                Debug.Log("Response: " + request.downloadHandler.text);

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error sending data to MongoDB: " + request.error);
                    if (callback != null)
                    {
                        callback.Invoke(false);
                    }
                }
                else
                {
                    Debug.Log("Successfully uploaded data to MongoDB: " + request.downloadHandler.text);
                    if (callback != null)
                    {
                        callback.Invoke(true);
                    }
                }
            }
        }

        #endregion

        private JsonSerializerSettings GetPolymorphicSettings()
        {
            return new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto, // Enables reading the $type field
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                Formatting = Formatting.Indented
            };
        }

        #region Session Data

        private void InitializeSessionData()
        {
            if (sessionData != null && !String.IsNullOrEmpty(sessionData.loginToken))
            {
                Debug.Log("Continue session!");
                return;
            }

            sessionData = new SessionData(loginToken, sessionNo + 1, attemptData.attemptNo);

            Debug.Log("Session Info set here!");

            StartPlayerSessionDataUpload();
        }

        private void StartPlayerSessionDataUpload()
        {
            PlayerSessionData playerSessionData = new PlayerSessionData(playerData, sessionData);

            StartCoroutine(UploadPlayerSessionData(JsonConvert.SerializeObject(playerSessionData), result =>
            {
                if (result)
                {
                    Debug.Log("Session Data successfully uploaded.");
                    OnSessionDataUploaded?.Invoke(true);
                }
                else
                {
                    Debug.LogError("Session Failed to upload data.");
                    OnSessionDataUploaded?.Invoke(false);
                }
            }));
        }

        // Player Session Data = sessionData + playerData
        IEnumerator UploadPlayerSessionData(string jsonData, System.Action<bool> callback = null)
        {
            Debug.Log("UploadPlayerSessionData : ");
            Debug.Log(jsonData);

            using (UnityWebRequest request = new UnityWebRequest(uploadPlayerSessionDataURL, "POST"))
            {
                request.SetRequestHeader("Content-Type", "application/json");

                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();

                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error sending session data to MongoDB: " + request.error);
                    if (callback != null)
                    {
                        callback.Invoke(false);
                    }
                }
                else
                {
                    Debug.Log("Successfully uploaded session data to MongoDB: " + request.downloadHandler.text);
                    if (callback != null)
                    {
                        callback.Invoke(true);
                    }
                }
            }
        }

        private void UpdateSessionDataOnQuit()
        {
            Debug.Log("UpdateSessionDataOnQuit called!");

            if (sessionData == null)
                return;

            DateTime sessionEndTime = DateTime.UtcNow;
            TimeSpan sessionDuration = sessionEndTime - sessionData.sessionStartTime;

            sessionData.sessionEndTime = sessionEndTime;
            sessionData.sessionDuration = sessionDuration.TotalSeconds;

            PlayerSessionData playerSessionData = new PlayerSessionData(null, sessionData);

            StartCoroutine(UploadPlayerSessionData(JsonConvert.SerializeObject(playerSessionData), result =>
            {
                if (result)
                {
                    Debug.Log("Session end data successfully uploaded.");
                }
                else
                {
                    Debug.LogError("Failed to upload session end data.");
                }
            }));
        }

        #endregion

        #region Player Data

        public void UploadPlayerData(string _playerName, string _playerEmail, DateTime? _playerDOB, string _playerGender)
        {
            if (loginToken == null || _playerName == null || _playerEmail == null || _playerDOB == null || _playerGender == null)
            {
                Debug.LogError("Null in one of the arguments for PlayerData object!");
                return;
            }

            playerData = new PlayerData(loginToken, metadata, _playerName, _playerEmail, _playerDOB, _playerGender);

            InitializeSessionData();
        }

        #endregion

        public static string FixTypeOrderInJson(string jsonResponse)
        {
            // Parse the JSON response into a JObject
            var root = JObject.Parse(jsonResponse);

            // Fix all objects recursively
            FixTypeOrder(root);

            // Convert the modified JSON back to a string
            return root.ToString();
        }

        private static void FixTypeOrder(JToken token)
        {
            if (token is JObject obj)
            {
                // Check if the object contains a $type property
                if (obj.ContainsKey("$type"))
                {
                    // Get the $type property
                    var typeProperty = obj["$type"];
                    // Remove it and re-add it to make it the first property
                    obj.Remove("$type");
                    var reorderedObj = new JObject { ["$type"] = typeProperty };
                    foreach (var property in obj.Properties())
                    {
                        reorderedObj.Add(property.Name, property.Value);
                    }

                    // Replace the original object with the reordered one
                    obj.Replace(reorderedObj);
                }

                // Process nested objects
                foreach (var child in obj.Properties())
                {
                    FixTypeOrder(child.Value);
                }
            }
            else if (token is JArray array)
            {
                // Copy the items to a temporary list to avoid modifying the collection during iteration
                var items = array.ToList();
                foreach (var item in items)
                {
                    FixTypeOrder(item);
                }
            }
        }
    }

    public class ServerResponse<T>
    {
        public bool playerFound;
        public int sessionNo;
        public string error;
        public T data;
    }

    public class GameData
    {
        public SessionData sessionData;
        public AttemptData attemptData;

        public GameData(SessionData _sessionData, AttemptData _attemptData)
        {
            sessionData = _sessionData;
            attemptData = _attemptData;
        }
    }

    public class PlayerSessionData
    {
        public PlayerData playerData;
        public SessionData sessionData;

        public PlayerSessionData(PlayerData _playerData, SessionData _sessionData)
        {
            playerData = _playerData;
            sessionData = _sessionData;
        }
    }

    public enum LoginType
    {
        newPlayer,
        newAttempt,
        continueAttempt,
        repeatAttempt,
        error
    }
}
