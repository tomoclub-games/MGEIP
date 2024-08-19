using MGEIP.Service;
using System.Collections;
using UnityEngine;

namespace MGEIP.Scenario.Scenes
{
    public class MCQQuestionScene : Scene
    {
        [SerializeField] private int scenarioNo;
        [SerializeField] private Scenario scenario;
        [SerializeField] private GameService gameService;

        [SerializeField] private bool isDialogueBoxActive;
        [SerializeField] private string dialogueText;
        [SerializeField] private string questionText;
        [SerializeField] private string optionText1;
        [SerializeField] private string optionText2;
        [SerializeField] private string optionText3;
        [SerializeField] private string optionText4;

        public override void EnterScene()
        {
            throw new System.NotImplementedException();
        }

        public override void ExitScene()
        {
            throw new System.NotImplementedException();
        }

        public void InitializeMCQQuestionScene(int scenarioNo, Scenario scenario, GameService gameService)
        {
            this.scenarioNo = scenarioNo;
            this.scenario = scenario;
            this.gameService = gameService;
        }

        public void SetMCQQuestionSceneInfo(bool isDialogueBoxActive, string dialogueText, string questionText, string option1, string option2, string option3, string option4)
        {
            this.isDialogueBoxActive = isDialogueBoxActive;
            this.dialogueText = dialogueText;
            this.questionText = questionText;
            this.optionText1 = option1;
            this.optionText2 = option2;
            this.optionText3 = option3;
            this.optionText4 = option4;
        }
    }
}