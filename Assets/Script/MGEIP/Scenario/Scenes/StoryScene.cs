using MGEIP.GameData.SceneData;
using MGEIP.Service;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MGEIP.Scenario.Scenes
{
    public class StoryScene : Scene
    {
        [SerializeField] private int scenarioNo;
        [SerializeField] private Scenario scenario;
        [SerializeField] private GameService gameService;

        [SerializeField] private bool isDialogueBoxActive;
        [SerializeField] private bool isNarrationBoxActive;
        [SerializeField] private string dialogue;
        [SerializeField] private string narrationText;

        [SerializeField] private GameObject dialogueBox;
        [SerializeField] private TextMeshProUGUI dialogueText;

        private GameUIService GameUIService => gameService.GameUIService;
        private SceneData sceneData;

        public override void EnterScene()
        {
            StartCurrentStoryScene();
        }

        public void InitializeStoryScene(int scenarioNo, SceneData sceneData, Scenario scenario, GameService gameService)
        {
            this.scenarioNo = scenarioNo;
            this.sceneData = sceneData;
            this.scenario = scenario;
            this.gameService = gameService;

            SetStorySceneInfo();
        }

        public void StartCurrentStoryScene()
        {
            GameUIService.SetStartStoryUIGameobjectActive(true);
            StorySceneInfo();
        }

        public void SetStorySceneInfo()
        {
            isDialogueBoxActive = sceneData.DialogueBox;
            isNarrationBoxActive = sceneData.NarrationBox;
            dialogue = sceneData.DialogueText;
            narrationText = sceneData.NarrationText;
        }

        public void StorySceneInfo()
        {
            GameUIService.SetStorySceneNarrationBoxActive(isNarrationBoxActive);
            GameUIService.SetStorySceneNarrationText(narrationText);

            dialogueBox.SetActive(isDialogueBoxActive);
            dialogueText.SetText(dialogue);
        }

        public void SetStorySceneButtons(Button nextButton, Button prevButton)
        {
            nextButton.onClick.AddListener(ExitScene);
            prevButton.onClick.AddListener(ExitToPrevScene);
        }

        public void CompleteStoryScene()
        {
            GameUIService.SetStorySceneNarrationBoxActive(false);
            GameUIService.SetStartStoryUIGameobjectActive(false);
        }

        public override void ExitScene()
        {
            CompleteStoryScene();
            scenario.IncreamentCurrentScene();
        }

        public void ExitToPrevScene()
        {
            CompleteStoryScene();
            scenario.DecrementCurrentScene();
        }
    }
}