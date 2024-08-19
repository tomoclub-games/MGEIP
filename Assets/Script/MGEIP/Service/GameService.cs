using MGEIP.GameData;
using MGEIP.Scenario;
using MGEIP.Scenario.Scenes;
using UnityEngine;

namespace MGEIP.Service
{
    public class GameService : MonoBehaviour
    {
        [SerializeField] private GameDataContainer gameDataContainer;
        [SerializeField] private ScenarioManager scenarioManager;

        public StartScene startScenePrefab;
        public StoryScene storyScenePrefab;
        public EndScene endScenePrefab;
        public MCQQuestionScene mcqQuestionScenePrefab;
        public AESliderQuestionScene aeSliderQuestionScenePrefab;
        public CESliderQuestionScene ceSliderQuestionScenePrefab;

        public GameDataContainer GameDataContainer => gameDataContainer;
        public ScenarioManager ScenarioManager => scenarioManager;

        private void Start()
        {
            ScenarioManager?.InitializeScenarioManager(this);
        }
    }
}