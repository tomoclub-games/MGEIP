using MGEIP.GameData.ScenarioData;
using MGEIP.GameData.SceneData;
using UnityEngine;

namespace MGEIP.GameData
{
    [CreateAssetMenu(fileName = "DatabaseAPIContainer", menuName = "MGEIP Spreadsheet Container/ APIContainer", order = 1)]
    public class DatabaseAPIContainer : ScriptableObject
    {
        [SerializeField] private string downloadGameDataURL;
        [SerializeField] private string uploadGameDataURL;
        [SerializeField] private string uploadPlayerSessionDataURL;

        public string DownloadGameDataURL => downloadGameDataURL;
        public string UploadGameDataURL => uploadGameDataURL;
        public string UploadPlayerSessionDataURL => uploadPlayerSessionDataURL;
    }
}