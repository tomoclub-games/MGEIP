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
        public void GetMGIEPData(string _loginToken)
        {
            DownloadMGIEPData(_loginToken);
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

        public void DownloadMGIEPData(string loginToken)
        {
            StartCoroutine(Download(loginToken, (result, isSuccess) =>
            {
                if (isSuccess && result != null && !string.IsNullOrEmpty(result.loginToken))
                {
                    Debug.Log("Data successfully downloaded: " + JsonConvert.SerializeObject(result));

                    mgiepData = result;

                    CheckForNewAttempt();
                }
                else if (isSuccess && result == null)
                {
                    Debug.Log("Failed to download data or player not found for player: " + loginToken + " - Setting up new attempt 1");

                    mgiepData = new MGIEPData(loginToken);

                    OnPlayerLogin?.Invoke(LoginType.newPlayer);
                }
                else
                {
                    Debug.Log("Failed to download data for player: " + loginToken + " - Unexpected error occurred");

                    OnPlayerLogin?.Invoke(LoginType.error);
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
                MGIEPData newMgiepData = new MGIEPData(mgiepData.loginToken);
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

        IEnumerator Download(string loginToken, System.Action<MGIEPData, bool> callback = null)
        {
            string url = $"https://ap-south-1.aws.data.mongodb-api.com/app/mgiepdevs-uzdhqga/endpoint/downloadAttemptInfo?loginToken={loginToken}";

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

                    var responseObj = JsonConvert.DeserializeObject<ServerResponse<MGIEPData>>(jsonResponse);

                    if (responseObj.success)
                    {
                        if (responseObj.playerFound)
                        {
                            callback?.Invoke(responseObj.data, true);
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

        IEnumerator Upload(string jsonData, System.Action<bool> callback = null)
        {
            string url = $"https://ap-south-1.aws.data.mongodb-api.com/app/mgiepdevs-uzdhqga/endpoint/updateAttemptInfo";

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
    }

    public class ServerResponse<T>
    {
        public bool success;
        public string error;
        public bool playerFound;
        public T data;
    }

    public enum LoginType
    {
        newPlayer,
        continueAttempt,
        newAttempt,
        error
    }
}
