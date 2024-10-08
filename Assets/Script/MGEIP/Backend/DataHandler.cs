using System.Collections;
using System.Text;
using MGEIP.Service;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace MGIEP.Data
{
    public class DataHandler : MonoBehaviour
    {
        private GameService gameService;
        private MGIEPData mgiepData;

        public MGIEPData MGIEPData => mgiepData;

        public void InitializeDataHandler(GameService _gameService)
        {
            gameService = _gameService;

            string playerName = "Guest" + Random.Range(1, 1000);
            mgiepData = new MGIEPData(playerName);

            gameService.GameUIService.UploadDataButton.onClick.AddListener(SendMGIEPData);
        }

        private void OnDestroy()
        {
            gameService.GameUIService.UploadDataButton.onClick.RemoveAllListeners();
        }

        [ContextMenu("Print Json Object")]
        public string GetJsonObject()
        {
            string jsonData = JsonConvert.SerializeObject(mgiepData);
            Debug.Log(jsonData);
            return jsonData;
        }

        public static MGIEPData SetMGIEPData(string jsonData)
        {
            return JsonConvert.DeserializeObject<MGIEPData>(jsonData);
        }

        [ContextMenu("Send MGIEP Data")]
        public void SendMGIEPData()
        {
            StartCoroutine(Upload(GetJsonObject(), result =>
            {
                if (result)
                {
                    Debug.Log("Data successfully uploaded.");
                }
                else
                {
                    Debug.LogError("Failed to upload data.");
                }
            }));
        }

        IEnumerator Download(string id, System.Action<MGIEPData> callback = null)
        {
            using (UnityWebRequest request = UnityWebRequest.Get("http://localhost:3000/plummies/" + id))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(request.error);
                    if (callback != null)
                    {
                        callback.Invoke(null);
                    }
                }
                else
                {
                    if (callback != null)
                    {
                        callback.Invoke(SetMGIEPData(request.downloadHandler.text));
                    }
                }
            }
        }

        IEnumerator Upload(string jsonData, System.Action<bool> callback = null)
        {
            string url = "https://ap-south-1.aws.data.mongodb-api.com/app/mgiepdevs-uzdhqga/endpoint/insert";

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
}
