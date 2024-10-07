using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Realms;
using MGIEP.Data;

namespace MGIEP
{
    public class RealmController : MonoBehaviour
    {
        public static RealmController Instance;

        private Realm _realm;

        private void Awake()
        {
            if (Instance == null)
            {
                DontDestroyOnLoad(gameObject);
                Instance = this;
            }

            if (_realm == null)
                _realm = Realm.GetInstance();
        }

        private void OnDisable()
        {
            if (_realm != null)
                _realm.Dispose();
        }

        private GameDataModel GetOrCreateGameData()
        {
            GameDataModel gameDataModel = _realm.Find<GameDataModel>("player1");

            if (gameDataModel == null)
            {
                _realm.Write(() =>
                {
                    gameDataModel = _realm.Add(new GameDataModel()
                    {
                        Id = "player1",
                        score = 0,
                        mgiepData = new MGIEPData("player1")
                    });
                });
            }

            return gameDataModel;
        }

        public MGIEPData GetMGIEPData()
        {
            GameDataModel gameDataModel = GetOrCreateGameData();
            return gameDataModel.mgiepData;
        }

        public void UpdateMGIEPData(MGIEPData mGIEPData)
        {
            GameDataModel gameDataModel = GetOrCreateGameData();

            _realm.Write(() =>
            {
                gameDataModel.mgiepData = mGIEPData;
            });
        }

        public void IncrementScore()
        {
            GameDataModel gameDataModel = GetOrCreateGameData();

            _realm.Write(() =>
            {
                gameDataModel.score++;
            });
        }
    }
}
