using System.Collections;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using System.Runtime.InteropServices;
using System;

namespace MGIEP.Data
{
    public class DataHandler : MonoBehaviour
    {
        public static DataHandler Instance;

        [DllImport("__Internal")]
        private static extern System.IntPtr GetURLParameter(string paramName);

        [DllImport("__Internal")]
        private static extern void FreeMemory(System.IntPtr ptr);

        private string loginToken;
        private int sessionNo;

        private AttemptInfo attemptInfo;
        private SessionInfo sessionInfo;

        public AttemptInfo AttemptInfo => attemptInfo;
        public SessionInfo SessionInfo => sessionInfo;

        public UnityAction OnLoginRequested;
        public UnityAction<LoginType> OnPlayerLogin;
        public UnityAction<bool> OnSessionDataUploaded;
        public UnityAction<bool> OnDataUploaded;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }

            GetLoginTokenFromQuery();
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

            GetPlayerData(loginToken);
        }

        private void OnApplicationQuit()
        {
            UpdateSessionInfoOnQuit();
        }

        // Sets up the AttemptInfo and SessionInfo from DB
        public void GetPlayerData(string loginToken)
        {
            StartCoroutine(DownloadPlayerData(loginToken, (result, isSuccess) =>
            {
                if (isSuccess && result != null && !string.IsNullOrEmpty(result.data.loginToken))
                {
                    Debug.Log("Data successfully downloaded: " + JsonConvert.SerializeObject(result));

                    attemptInfo = result.data;

                    if (sessionInfo == null)
                        sessionNo = result.sessionNo;

                    CheckForNewAttempt();
                }
                else if (isSuccess && result == null)
                {
                    Debug.Log("Failed to download data or player not found for player: " + loginToken + " - Setting up new attempt 1");

                    attemptInfo = new AttemptInfo(loginToken);
                    sessionNo = 0;

                    OnPlayerLogin?.Invoke(LoginType.newPlayer);
                }
                else
                {
                    Debug.Log("Failed to download data for player: " + loginToken + " - Unexpected error occurred");

                    attemptInfo = null;
                    sessionNo = 0;

                    OnPlayerLogin?.Invoke(LoginType.error);
                }

                Debug.Log("Session no : " + sessionNo);

                InitializeSessionInfo();
            }));
        }

        private void CheckForNewAttempt()
        {
            bool isAttemptComplete = false;

            for (int i = 0; i < attemptInfo.completedScenarios.Length; i++)
            {
                if (attemptInfo.completedScenarios[i] == false)
                {
                    isAttemptComplete = false;
                    break;
                }

                isAttemptComplete = true;
            }

            if (isAttemptComplete)
            {
                AttemptInfo newattemptInfo = new AttemptInfo(attemptInfo.loginToken);
                newattemptInfo.attemptNo = attemptInfo.attemptNo + 1;

                Debug.Log("New attempt no : " + newattemptInfo.attemptNo);

                attemptInfo = newattemptInfo;

                OnPlayerLogin?.Invoke(LoginType.newAttempt);
            }
            else
            {
                OnPlayerLogin?.Invoke(LoginType.continueAttempt);

                Debug.Log("Continue attempt!");
            }
        }

        private void GetLoginTokenFromQuery()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            // Get the pointer to the URL parameter
            System.IntPtr urlParamPtr = GetURLParameter("loginToken");

            // Check if the pointer is valid
            if (urlParamPtr != System.IntPtr.Zero)
            {
                // Convert the pointer to a string
                loginToken = Marshal.PtrToStringAuto(urlParamPtr);
                Debug.Log("Login Token: " + loginToken);

                // Free the allocated memory on the JavaScript side
                FreeMemory(urlParamPtr);
            }
            else
            {
                Debug.Log("Login token not found in the URL.");
            }
#else
            Debug.Log("Running outside WebGL. No URL parameter.");
#endif
        }

        public void AddCompletedScenario(ScenarioInfo _scenarioInfo)
        {
            attemptInfo.completedScenarios[_scenarioInfo.scenarioNo - 1] = true;
            attemptInfo.scenarioList.Add(_scenarioInfo);

            Attempt attempt = sessionInfo.attempts.Find(id => id.attemptNo == attemptInfo.attemptNo);

            if (attempt == null)
            {
                attempt = new Attempt(attemptInfo.attemptNo);
                sessionInfo.attempts.Add(attempt);
            }

            attempt.completedScenarios.Add(_scenarioInfo.scenarioNo);

            // Upload AttemptInfo and SessionInfo
            SetPlayerData();
        }

        public void SetPlayerData()
        {
            PlayerData playerData = new PlayerData(sessionInfo, attemptInfo);

            StartCoroutine(UploadPlayerData(JsonConvert.SerializeObject(playerData), result =>
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

        // Player Data is sessionNo + Latest AttemptInfo
        IEnumerator DownloadPlayerData(string loginToken, System.Action<ServerResponse<AttemptInfo>, bool> callback = null)
        {
            string url = $"https://ap-south-1.aws.data.mongodb-api.com/app/mgiepdevs-uzdhqga/endpoint/downloadPlayerData?loginToken={loginToken}";

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error: " + request.error);
                    callback?.Invoke(null, false);
                }
                else
                {
                    string jsonResponse = request.downloadHandler.text;

                    Debug.Log("JSON Response DOWNLOAD PLAYER DATA: \n" + jsonResponse);

                    var responseObj = JsonConvert.DeserializeObject<ServerResponse<AttemptInfo>>(jsonResponse);

                    if (responseObj.success)
                    {
                        if (responseObj.playerFound)
                        {
                            callback?.Invoke(responseObj, true);
                        }
                        else
                        {
                            Debug.Log("Player not found.");
                            callback?.Invoke(null, true);
                        }
                    }
                    else
                    {
                        Debug.LogError("Error: " + responseObj.error);
                        callback?.Invoke(null, false);
                    }
                }
            }
        }

        // Player Data is SessionInfo + AttemptInfo
        IEnumerator UploadPlayerData(string jsonData, System.Action<bool> callback = null)
        {
            string url = $"https://ap-south-1.aws.data.mongodb-api.com/app/mgiepdevs-uzdhqga/endpoint/uploadPlayerData";

            using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
            {
                request.SetRequestHeader("Content-Type", "application/json");

                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();

                yield return request.SendWebRequest();

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

        #region Session Data

        private void InitializeSessionInfo()
        {
            if (sessionInfo != null)
            {
                Debug.Log("Continue session!");
                return;
            }

            sessionInfo = new SessionInfo(loginToken, sessionNo + 1, attemptInfo.attemptNo);

            Debug.Log("Session Info set here!");

            UpdateSessionInfo();
        }

        private void UpdateSessionInfo()
        {
            StartCoroutine(UploadSessionInfo(JsonConvert.SerializeObject(sessionInfo), result =>
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

        IEnumerator UploadSessionInfo(string jsonData, System.Action<bool> callback = null)
        {
            string url = $"https://ap-south-1.aws.data.mongodb-api.com/app/mgiepdevs-uzdhqga/endpoint/uploadSessionInfo";

            using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
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

        private void UpdateSessionInfoOnQuit()
        {
            Debug.Log("UpdateSessionInfoOnQuit called!");

            if (sessionInfo == null)
                return;

            DateTime sessionEndTime = DateTime.UtcNow;
            TimeSpan sessionDuration = sessionEndTime - sessionInfo.sessionStartTime;

            sessionInfo.sessionEndTime = sessionEndTime;
            sessionInfo.sessionDuration = sessionDuration.TotalSeconds;

            StartCoroutine(UploadSessionInfo(JsonConvert.SerializeObject(sessionInfo), result =>
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
    }

    public class ServerResponse<T>
    {
        public bool success;
        public bool playerFound;
        public int sessionNo;
        public string error;
        public T data;
    }

    public class PlayerData
    {
        public SessionInfo sessionInfo;
        public AttemptInfo attemptInfo;

        public PlayerData(SessionInfo _sessionInfo, AttemptInfo _attemptInfo)
        {
            sessionInfo = _sessionInfo;
            attemptInfo = _attemptInfo;
        }
    }

    public enum LoginType
    {
        newPlayer,
        continueAttempt,
        newAttempt,
        error
    }
}
