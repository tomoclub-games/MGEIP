using MGEIP.GameData.SceneData;
using MGEIP.Service;
using UnityEngine;
using UnityEngine.UI;

namespace MGEIP.Scenario.Scenes
{
    public class EndScene : Scene
    {
        [SerializeField] private int scenarioNo;
        [SerializeField] private Scenario scenario;
        [SerializeField] private GameService gameService;

        [SerializeField] private bool isNarrationBoxActive;
        [SerializeField] private string narrationText;

        private GameUIService GameUIService => gameService.GameUIService;
        private SceneData sceneData;

        public override void EnterScene()
        {
            StartCurrentEndScene();
        }

        public void StartCurrentEndScene()
        {
            GameUIService.SetEndStoryUIGameobjectActive(true);
            EndSceneInfo();
        }

        public void InitializeEndScene(int scenarioNo, SceneData sceneData, Scenario scenario, GameService gameService)
        {
            this.scenarioNo = scenarioNo;
            this.sceneData = sceneData;
            this.scenario = scenario;
            this.gameService = gameService;

            SetEndSceneInfo();
        }

        public void SetEndSceneInfo()
        {
            isNarrationBoxActive = sceneData.NarrationBox;
            narrationText = sceneData.NarrationText;
        }

        public void EndSceneInfo()
        {

            GameUIService.SetEndSceneNarrationBoxActive(isNarrationBoxActive);
            GameUIService.SetEndSceneNarrationText(narrationText);
        }

        public void SetEndSceneButtons(Button nextButton)
        {
            nextButton.onClick.AddListener(ExitScene);
        }

        public void CompleteEndScene()
        {
            GameUIService.SetEndSceneNarrationBoxActive(false);
            GameUIService.SetEndStoryUIGameobjectActive(false);
        }

        public override void ExitScene()
        {
            CompleteEndScene();
            scenario.IncreamentCurrentScene();
        }
    }
}