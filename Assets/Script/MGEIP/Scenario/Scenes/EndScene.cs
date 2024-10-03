using MGEIP.GameData.SceneData;
using MGEIP.Service;
using MGIEP;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MGEIP.Scenario.Scenes
{
    public class EndScene : Scene
    {
        [SerializeField] private GameObject dialogueBox;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private AudioClipButton dialogueVOButton;

        public override void EnterScene()
        {
            base.EnterScene();

            StartCurrentEndScene();

            dialogueVOButton.Button.onClick.AddListener(PlayDialogueVoiceOver);
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
            isDialogueBoxActive = sceneData.DialogueBox;
            isNarrationBoxActive = sceneData.NarrationBox;
            dialogue = sceneData.DialogueText;
            narrationText = sceneData.NarrationText;
        }

        public void EndSceneInfo()
        {
            GameUIService.SetEndSceneNarrationBoxActive(isNarrationBoxActive);
            GameUIService.SetEndSceneNarrationText(narrationText);

            dialogueBox.SetActive(isDialogueBoxActive);
            dialogueText.SetText(dialogue);
        }

        public void SetEndSceneButtons(Button nextButton, Button prevButton)
        {
            nextButton.onClick.AddListener(ExitScene);
            prevButton.onClick.AddListener(ExitToPrevScene);
        }

        public void CompleteEndScene()
        {
            GameUIService.SetEndSceneNarrationBoxActive(false);
            GameUIService.SetEndStoryUIGameobjectActive(false);
        }

        public override void ExitScene()
        {
            base.ExitScene();

            dialogueVOButton.Button.onClick.RemoveAllListeners();

            CompleteEndScene();
            scenario.IncreamentCurrentScene();
        }

        public override void ExitToPrevScene()
        {
            base.ExitToPrevScene();

            dialogueVOButton.Button.onClick.RemoveAllListeners();

            CompleteEndScene();
            scenario.DecrementCurrentScene();
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