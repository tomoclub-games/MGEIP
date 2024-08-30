using MGEIP.GameData.SceneData;
using MGEIP.Service;
using UnityEngine;
using UnityEngine.UI;

namespace MGEIP.Scenario.Scenes
{
    public class QuestionScene : Scene
    {
        [SerializeField] protected int scenarioNo;
        [SerializeField] protected Scenario scenario;
        [SerializeField] protected GameService gameService;

        [SerializeField] protected CharacterType characterType;
        [SerializeField] protected bool isDialogueBoxActive;
        [SerializeField] protected string dialogueText;
        [SerializeField] protected string questionText;

        protected GameUIService GameUIService => gameService.GameUIService;
        protected SceneData sceneData;

        public override void EnterScene()
        {
            GameUIService.SetQuestionSceneUIActive(true);
            GameUIService.GetCharacterUI().SetZoomInCharacterActive(true);
            GameUIService.GetCharacterUI().SetZoomInMainCharacterActive(true);

            GameUIService.SetScenarioBackgroundSprite(sceneData.SceneBG);
            GameUIService.SetScenarioForegroundSprite(sceneData.SceneFG);
        }

        public void InitializeQuestionScene(int scenarioNo, SceneData sceneData, Scenario scenario, GameService gameService)
        {
            this.scenarioNo = scenarioNo;
            this.sceneData = sceneData;
            this.scenario = scenario;
            this.gameService = gameService;
        }

        public virtual void SetQuestionButton(Button questionConfirmButton)
        {
            questionConfirmButton.onClick.AddListener(ExitScene);
        }

        public virtual void CompleteQuestionScene()
        {
            GameUIService.SetQuestionSceneUIActive(false);
            GameUIService.GetCharacterUI().ResetCharacterUI();
        }

        public override void ExitScene()
        {
            CompleteQuestionScene();
            scenario.IncreamentCurrentScene();
        }
    }
}