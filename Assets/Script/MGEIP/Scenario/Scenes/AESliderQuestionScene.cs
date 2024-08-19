using MGEIP.Service;
using System.Collections;
using UnityEngine;

namespace MGEIP.Scenario.Scenes
{
    public class AESliderQuestionScene : Scene
    {
        [SerializeField] private int scenarioNo;
        [SerializeField] private Scenario scenario;
        [SerializeField] private GameService gameService;

        [SerializeField] private bool isDialogueBoxActive;
        [SerializeField] private string dialogueText;
        [SerializeField] private string questionText;

        public override void EnterScene()
        {
            throw new System.NotImplementedException();
        }

        public override void ExitScene()
        {
            throw new System.NotImplementedException();
        }

        public void InitializeAESliderQuestionScene(int scenarioNo, Scenario scenario, GameService gameService)
        {
            this.scenarioNo = scenarioNo;
            this.scenario = scenario;
            this.gameService = gameService;
        }

        public void SetAESliderQuestionSceneInfo(bool isDialogueBoxActive, string dialogueText, string questionText)
        {
            this.isDialogueBoxActive = isDialogueBoxActive;
            this.dialogueText = dialogueText;
            this.questionText = questionText;
        }
    }
}