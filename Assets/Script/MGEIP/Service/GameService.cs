using MGEIP.GameData;
using MGEIP.Scenario;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MGEIP.Service
{
    public class GameService : MonoBehaviour
    {
        public static GameService Instance { get; private set; }

        [SerializeField] private GameDataContainer gameDataContainer;
        [SerializeField] private ScenarioManager scenarioManager;
        [SerializeField] private GameUIService gameUIService;

        public GameDataContainer GameDataContainer => gameDataContainer;
        public ScenarioManager ScenarioManager => scenarioManager;
        public GameUIService GameUIService => gameUIService;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        private void Start()
        {
            ScenarioManager.InitializeScenarioManager(this);
            GameUIService.GetGameEndButton.gameObject.SetActive(false);
            GameUIService.GetGameEndButton.onClick.AddListener(OnGameEndButtonClick);
        }

        private void OnGameEndButtonClick()
        {
            SceneManager.LoadScene(2);
        }
    }
}