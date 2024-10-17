using System.Collections;
using System.Text;
using MGEIP.Service;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using System.Runtime.InteropServices;

namespace MGIEP.Data
{
    public class DataHandler : MonoBehaviour
    {
        public static DataHandler Instance;

        [DllImport("__Internal")]
        private static extern System.IntPtr GetURLParameter(string paramName);

        [DllImport("__Internal")]
        private static extern void FreeMemory(System.IntPtr ptr);

        public MGIEPData mgiepData;

        public MGIEPData MGIEPData => mgiepData;

        public UnityAction OnLoginRequested;
        public UnityAction<LoginType> OnPlayerLogin;
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
        }

        public void Start()
        {
            string loginToken = null;

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

            if (loginToken == null)
            {
                Debug.LogWarning("Error finding parameter 'loginToken'");
                return;
            }

            Debug.Log("Trying to login!");
            GetMGIEPData(loginToken);
        }

        [ContextMenu("Get MGIEP Data")]
        public void GetMGIEPData(string playerName)
        {
            DownloadMGIEPData(playerName);
        }

        [ContextMenu("Send MGIEP Data")]
        public void SendMGIEPData()
        {
            StartCoroutine(Upload(GetJsonObject(), result =>
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

        [ContextMenu("Print Json Object")]
        public string GetJsonObject()
        {
            string jsonData = JsonConvert.SerializeObject(mgiepData);
            return jsonData;
        }

        public void DownloadMGIEPData(string playerName)
        {
            StartCoroutine(Download(playerName, result =>
            {
                if (result != null && !string.IsNullOrEmpty(result.playerName))
                {
                    Debug.Log("Data successfully downloaded: " + JsonConvert.SerializeObject(result));

                    mgiepData = result;

                    CheckForNewAttempt();
                }
                else
                {
                    Debug.Log("Failed to download data or player not found for player: " + playerName + " - Setting up new attempt 1");

                    mgiepData = new MGIEPData(playerName);

                    OnPlayerLogin?.Invoke(LoginType.newPlayer);
                }
            }));
        }

        private void CheckForNewAttempt()
        {
            bool isAttemptComplete = false;

            for (int i = 0; i < mgiepData.completedScenarios.Length; i++)
            {
                if (mgiepData.completedScenarios[i] == false)
                {
                    isAttemptComplete = false;
                    break;
                }

                isAttemptComplete = true;
            }

            if (isAttemptComplete)
            {
                MGIEPData newMgiepData = new MGIEPData(mgiepData.playerName);
                newMgiepData.attemptNo = mgiepData.attemptNo + 1;

                Debug.Log("New attempt no : " + newMgiepData.attemptNo);

                mgiepData = newMgiepData;

                OnPlayerLogin?.Invoke(LoginType.newAttempt);
            }
            else
            {
                OnPlayerLogin?.Invoke(LoginType.continueAttempt);

                Debug.Log("Continue attempt!");
            }
        }

        IEnumerator Download(string playerName, System.Action<MGIEPData> callback = null)
        {
            string url = $"https://ap-south-1.aws.data.mongodb-api.com/app/mgiepdevs-uzdhqga/endpoint/downloadAttemptInfo?playerName={playerName}";

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

                    // Deserialize the response into MGIEPData object
                    var responseObj = JsonConvert.DeserializeObject<ServerResponse<MGIEPData>>(jsonResponse);

                    if (responseObj.success && responseObj.data != null && !string.IsNullOrEmpty(responseObj.data.playerName))
                    {
                        callback?.Invoke(responseObj.data);
                    }
                    else
                    {
                        Debug.Log("Error: " + responseObj.error ?? "Player not found.");
                        callback?.Invoke(null);
                    }
                }
            }
        }

        IEnumerator Upload(string jsonData, System.Action<bool> callback = null)
        {
            string url = $"https://ap-south-1.aws.data.mongodb-api.com/app/mgiepdevs-uzdhqga/endpoint/updateAttemptInfo";

            using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
            {
                request.SetRequestHeader("Content-Type", "application/json");

                // Convert your JSON data to a byte array
                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();

                // Send the request and wait for the response
                yield return request.SendWebRequest();

                // Check for any errors
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
                    // If successful, invoke the callback with true
                    Debug.Log("Successfully uploaded data to MongoDB: " + request.downloadHandler.text);
                    if (callback != null)
                    {
                        callback.Invoke(true);
                    }
                }
            }
        }
    }

    public class ServerResponse<T>
    {
        public bool success;
        public string error;
        public T data;
    }

    public enum LoginType
    {
        newPlayer,
        continueAttempt,
        newAttempt
    }
}
