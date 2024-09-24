using MGEIP.Service;
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

            string playerName = "Guest" + Random.Range(1, 1000);
            mgiepData = new MGIEPData(playerName);
        }
    }
}
