using UnityEngine;
using MGEIP.Service;
using MGEIP.GameData.SceneData;
using UnityEngine.UI;
using TMPro;

namespace MGEIP.Scenario.Scenes
{
    public class PhotoCaptureScene : Scene
    {
        [SerializeField] protected int scenarioNo;
        [SerializeField] protected Scenario scenario;
        [SerializeField] protected GameService gameService;

        [SerializeField] private bool isDialogueBoxActive;
        [SerializeField] private bool isNarrationBoxActive;
        [SerializeField] private string dialogue;
        [SerializeField] private string narrationText;

        [SerializeField] private GameObject dialogueBox;
        [SerializeField] private TextMeshProUGUI dialogueText;

        protected GameUIService GameUIService => gameService.GameUIService;
        protected SceneData sceneData;

        public override void EnterScene()
        {
            StartPhotoCaptureScene();
        }

        public void InitializePhotoCaptureScene(int scenarioNo, SceneData sceneData, Scenario scenario, GameService gameService)
        {
            this.scenarioNo = scenarioNo;
            this.sceneData = sceneData;
            this.scenario = scenario;
            this.gameService = gameService;

            SetPhotoCaptureSceneInfo();
        }

        private void StartPhotoCaptureScene()
        {
            GameUIService.SetPhotoCaptureUIGameobjectActive(true);
            PhotoCaptureSceneInfo();
        }

        private void SetPhotoCaptureSceneInfo()
        {
            isDialogueBoxActive = sceneData.DialogueBox;
            isNarrationBoxActive = sceneData.NarrationBox;
            dialogue = sceneData.DialogueText;
            narrationText = sceneData.NarrationText;
        }

        private void PhotoCaptureSceneInfo()
        {
            GameUIService.SetPhotoCaptureNarrationBoxActive(isNarrationBoxActive);
            GameUIService.SetPhotoCaptureNarrationText(narrationText);

            dialogueBox.SetActive(isDialogueBoxActive);
            dialogueText.SetText(dialogue);
        }

        public void SetPhotoCaptureSceneButtons(Button nextButton, Button prevButton)
        {
            nextButton.onClick.AddListener(ExitScene);
            prevButton.onClick.AddListener(ExitToPrevScene);
        }

        private void CompletePhotoCaptureScene()
        {
            GameUIService.SetPhotoCaptureUIGameobjectActive(false);
        }

        public override void ExitScene()
        {
            CompletePhotoCaptureScene();
            scenario.IncreamentCurrentScene();
        }

        public void ExitToPrevScene()
        {
            CompletePhotoCaptureScene();
            scenario.DecrementCurrentScene();
        }
    }
}
