using MGEIP.Service;
using Newtonsoft.Json;
using UnityEngine;

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

            mgiepData = RealmController.Instance.GetMGIEPData();
        }

        [ContextMenu("Update MGIEPData")]
        public void UpdateMGIEPData()
        {
            RealmController.Instance.UpdateMGIEPData(mgiepData);
        }

        [ContextMenu("Update Score")]
        public void IncrementScore()
        {
            RealmController.Instance.IncrementScore();
        }

        [ContextMenu("Print Json Object")]
        public void GetJsonObject()
        {
            string jsonData = JsonConvert.SerializeObject(mgiepData);
            Debug.Log(jsonData);
        }
    }
}
