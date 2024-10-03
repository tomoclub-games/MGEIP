using UnityEngine;
using MGEIP.Service;
using MGEIP.GameData.SceneData;
using UnityEngine.UI;
using TMPro;
using MGIEP;

namespace MGEIP.Scenario.Scenes
{
    public class PhotoCaptureScene : Scene
    {
        [SerializeField] private GameObject dialogueBox;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private AudioClipButton dialogueVOButton;

        public override void EnterScene()
        {
            base.EnterScene();

            StartPhotoCaptureScene();

            dialogueVOButton.Button.onClick.AddListener(PlayDialogueVoiceOver);
        }

        public void InitializePhotoCaptureScene(int scenarioNo, SceneData sceneData, Scenario scenario, GameService gameService)
        {
            this.scenarioNo = scenarioNo;
            this.sceneData = sceneData;
            this.scenario = scenario;
            this.gameService = gameService;

            SetPhotoCaptureSceneInfo();

            GameUIService.OnPhotoCapture += DisableDialogue;
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

            GameUIService.OnPhotoCapture -= DisableDialogue;
        }

        public override void ExitScene()
        {
            base.ExitScene();

            dialogueVOButton.Button.onClick.RemoveAllListeners();

            CompletePhotoCaptureScene();
            scenario.IncreamentCurrentScene();
        }

        public override void ExitToPrevScene()
        {
            base.ExitToPrevScene();

            dialogueVOButton.Button.onClick.RemoveAllListeners();

            CompletePhotoCaptureScene();
            scenario.DecrementCurrentScene();
        }

        private void DisableDialogue()
        {
            dialogueBox.SetActive(false);
        }

        public void PlayDialogueVoiceOver()
        {
            if (isDialogueBoxActive)
            {
                string dialogueClipName = $"Scenarios/dt_{scenarioNo}_{sceneData.SceneNo}";
                dialogueVOButton.PlayAudioClip(dialogueClipName);
            }
        }
    }
}
