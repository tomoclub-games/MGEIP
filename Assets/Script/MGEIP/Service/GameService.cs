using MGEIP.GameData;
using MGEIP.Scenario;
using MGIEP;
using MGIEP.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MGEIP.Service
{
    public class GameService : MonoBehaviour
    {
        [SerializeField] private GameDataContainer gameDataContainer;
        [SerializeField] private ScenarioManager scenarioManager;
        [SerializeField] private GameUIService gameUIService;
        [SerializeField] private DataHandler dataHandler;

        public GameDataContainer GameDataContainer => gameDataContainer;
        public ScenarioManager ScenarioManager => scenarioManager;
        public GameUIService GameUIService => gameUIService;
        public DataHandler DataHandler => dataHandler;

        private void Start()
        {
            ScenarioManager.InitializeScenarioManager(this);
            // GameUIService.GetGameEndButton.gameObject.SetActive(false);
            GameUIService.GetGameEndButton.onClick.AddListener(OnGameEndButtonClick);

            // SoundManagerService.Instance.LoadAudio("GameSceneAudioClips");
        }

        private void OnGameEndButtonClick()
        {
            // SoundManagerService.Instance.ReleaseAudio();

            gameUIService.FadeOut();

            Invoke(nameof(LoadNextScene), 2);
        }

        private void LoadNextScene()
        {
            SceneManager.LoadScene(2);
        }
    }
}