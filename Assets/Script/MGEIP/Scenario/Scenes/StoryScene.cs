using MGEIP.GameData.SceneData;
using MGEIP.Service;
using MGIEP;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MGEIP.Scenario.Scenes
{
    public class StoryScene : Scene
    {
        [SerializeField] private GameObject dialogueBox;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private AudioClipButton dialogueVOButton;

        public override void EnterScene()
        {
            base.EnterScene();
            StartCurrentStoryScene();

            GameUIService.StorySceneNextButton.gameObject.GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);
            GameUIService.StoryScenePrevButton.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            dialogueVOButton.Button.onClick.AddListener(PlayDialogueVoiceOver);
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
            base.ExitScene();

            dialogueVOButton.Button.onClick.RemoveAllListeners();

            CompleteStoryScene();
            scenario.IncreamentCurrentScene();
        }

        public override void ExitToPrevScene()
        {
            base.ExitToPrevScene();

            dialogueVOButton.Button.onClick.RemoveAllListeners();

            CompleteStoryScene();
            scenario.DecrementCurrentScene();
        }

        public void PlayDialogueVoiceOver()
        {
            if (isDialogueBoxActive)
            {
                string dialogueClipName = $"Scenarios/sc_{scenarioNo}/dt_{scenarioNo}_{sceneData.SceneNo}";
                dialogueVOButton.PlayAudioClip(dialogueClipName);
            }
        }
    }
}