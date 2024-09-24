using MGEIP.GameData.SceneData;
using MGEIP.Service;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MGEIP.Scenario.Scenes
{
    public class QuestionScene : Scene
    {
        [SerializeField] protected int scenarioNo;
        [SerializeField] protected Scenario scenario;
        [SerializeField] protected GameService gameService;

        [SerializeField] protected bool isDialogueBoxActive;
        [SerializeField] protected bool isNarrationBoxActive;
        [SerializeField] protected string dialogue;
        [SerializeField] protected string narrationText;
        [SerializeField] protected string questionText;

        [SerializeField] protected GameObject dialogueBox;
        [SerializeField] protected TextMeshProUGUI dialogueText;

        protected GameUIService GameUIService => gameService.GameUIService;
        protected SceneData sceneData;

        public override void EnterScene()
        {
            GameUIService.SetQuestionSceneUIActive(true);
        }

        public virtual void InitializeQuestionScene(int scenarioNo, SceneData sceneData, Scenario scenario, GameService gameService)
        {
            this.scenarioNo = scenarioNo;
            this.sceneData = sceneData;
            this.scenario = scenario;
            this.gameService = gameService;
        }

        public virtual void SetQuestionButton(Button questionConfirmButton, Button questionPrevButton)
        {
            questionConfirmButton.onClick.AddListener(ExitScene);
            questionPrevButton.onClick.AddListener(ExitToPrevScene);
        }

        public virtual void CompleteQuestionScene()
        {
            GameUIService.SetQuestionSceneNarrationBoxActive(false);
            GameUIService.SetQuestionSceneUIActive(false);
        }

        public override void ExitScene()
        {
            CompleteQuestionScene();
            scenario.IncreamentCurrentScene();
        }

        public void ExitToPrevScene()
        {
            CompleteQuestionScene();
            scenario.DecrementCurrentScene();
        }
    }
}