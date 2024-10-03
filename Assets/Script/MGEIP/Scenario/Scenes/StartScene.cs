using MGEIP.GameData.SceneData;
using MGEIP.Service;
using MGIEP;
using UnityEngine;
using UnityEngine.UI;

namespace MGEIP.Scenario.Scenes
{
    public class StartScene : Scene
    {
        public override void EnterScene()
        {
            base.EnterScene();

            StartCurrentStartScene();

            GameUIService.StartSceneTitleVOButton.Button.onClick.AddListener(PlayScenarioNameVO);
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

            if (gameService.DataHandler.MGIEPData.scenarioList[scenarioNo - 1].questions.Count > 0)
                gameService.DataHandler.MGIEPData.scenarioList[scenarioNo - 1].questions.Clear();
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
            base.ExitScene();

            GameUIService.StartSceneTitleVOButton.Button.onClick.RemoveAllListeners();

            CompleteStartScene();
            scenario.IncreamentCurrentScene();
        }

        public void PlayScenarioNameVO()
        {
            string scenarioNameClip = $"sn_{scenarioNo}";
            GameUIService.StartSceneTitleVOButton.PlayAudioClip(scenarioNameClip);
        }
    }
}