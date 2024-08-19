using MGEIP.Service;
using UnityEngine;

namespace MGEIP.Scenario.Scenes
{
    public class StoryScene : Scene
    {
        [SerializeField] private int scenarioNo;
        [SerializeField] private Scenario scenario;
        [SerializeField] private GameService gameService;

        [SerializeField] private bool isDialogueBoxActive;
        [SerializeField] private bool isNarrationBoxActive;
        [SerializeField] private string dialogueText;
        [SerializeField] private string narrationText;

        public override void EnterScene()
        {
            throw new System.NotImplementedException();
        }

        public override void ExitScene()
        {
            throw new System.NotImplementedException();
        }

        public void InitializeStoryScene(int scenarioNo, Scenario scenario, GameService gameService)
        {
            this.scenarioNo = scenarioNo;
            this.scenario = scenario;
            this.gameService = gameService;
        }

        public void SetStorySceneInfo(bool isDialogueBoxActive, bool isNarrationBoxActive, string dialogueText, string narrationText)
        {
            this.isDialogueBoxActive = isDialogueBoxActive;
            this.isNarrationBoxActive = isNarrationBoxActive;
            this.dialogueText = dialogueText;
            this.narrationText = narrationText;
        }
    }
}