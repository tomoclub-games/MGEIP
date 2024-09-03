using MGEIP.GameData.SceneData;
using MGEIP.Service;
using UnityEngine;
using UnityEngine.UI;

namespace MGEIP.Scenario.Scenes
{
    public class StartScene : Scene
    {
        [SerializeField] private int scenarioNo;
        [SerializeField] private Scenario scenario;
        [SerializeField] private GameService gameService;

        [SerializeField] private string scenarioName;
        [SerializeField] private bool isNarrationBoxActive;
        [SerializeField] private string narrationText;

        private GameUIService GameUIService => gameService.GameUIService;
        private SceneData sceneData;

        public override void EnterScene()
        {
            StartCurrentStartScene();
        }

        public void StartCurrentStartScene()
        {
            GameUIService.SetStartSceneUIGameobjectActive(true);
            StartSceneInfo();
        }

        public void InitializeStartScene(int scenarioNo, string scenarioName, SceneData sceneData, Scenario scenario, GameService gameService)
        {
            this.scenarioNo = scenarioNo;
            this.scenarioName = scenarioName;
            this.sceneData = sceneData;
            this.scenario = scenario;
            this.gameService = gameService;

            SetStartSceneInfo();
        }

        public void SetStartSceneInfo()
        {
            isNarrationBoxActive = sceneData.NarrationBox;
            narrationText = sceneData.NarrationText;
        }

        public void StartSceneInfo()
        {
            GameUIService.SetScenarioNameText(this.scenarioName);

            if (isNarrationBoxActive)
            {
                GameUIService.SetStartSceneNarrationBoxActive(true);
                GameUIService.SetStartSceneNarrationText(narrationText);
            }
        }

        public void SetStartSceneButtons(Button playButton)
        {
            playButton.onClick.AddListener(ExitScene);
        }

        public void CompleteStartScene()
        {
            GameUIService.SetStartSceneNarrationBoxActive(false);
            GameUIService.SetStartSceneUIGameobjectActive(false);
        }

        public override void ExitScene()
        {
            CompleteStartScene();
            scenario.IncreamentCurrentScene();
        }
    }
}