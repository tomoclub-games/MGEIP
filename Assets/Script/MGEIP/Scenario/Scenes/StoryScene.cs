using MGEIP.GameData.SceneData;
using MGEIP.Service;
using UnityEngine;
using UnityEngine.UI;

namespace MGEIP.Scenario.Scenes
{
    public class StoryScene : Scene
    {
        [SerializeField] private int scenarioNo;
        [SerializeField] private Scenario scenario;
        [SerializeField] private GameService gameService;

        [SerializeField] private CharacterType characterType;
        [SerializeField] private bool isCharacterZoomActive;
        [SerializeField] private bool isSideCharacterEnable;
        [SerializeField] private bool isDialogueBoxActive;
        [SerializeField] private bool isNarrationBoxActive;
        [SerializeField] private string dialogueText;
        [SerializeField] private string narrationText;

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

            GameUIService.SetScenarioBackgroundSprite(sceneData.SceneBG);
            GameUIService.SetScenarioForegroundSprite(sceneData.SceneFG);
        }

        public void SetStorySceneInfo()
        {
            characterType = sceneData.CharacterType;
            isCharacterZoomActive = sceneData.ZoomCharacter;
            isSideCharacterEnable = sceneData.EnableSideChar;
            isDialogueBoxActive = sceneData.DialogueBox;
            isNarrationBoxActive = sceneData.NarrationBox;
            dialogueText = sceneData.DialogueText;
            narrationText = sceneData.NarrationText;
        }

        public void StorySceneInfo()
        {
            if (isNarrationBoxActive)
            {
                GameUIService.SetStorySceneNarrationBoxActive(true);
                GameUIService.SetStorySceneNarrationText(narrationText);
            }

            if (isCharacterZoomActive)
            {
                if (characterType == CharacterType.Main && isDialogueBoxActive)
                {
                    GameUIService.GetCharacterUI().SetZoomInMainCharDialogueText(dialogueText);
                    GameUIService.GetCharacterUI().SetZoomInMainCharDialogueBoxActive(true);
                }
                GameUIService.GetCharacterUI().SetZoomInMainCharacterActive(true);

                if (isSideCharacterEnable)
                {
                    if (characterType == CharacterType.Side && isDialogueBoxActive)
                    {
                        GameUIService.GetCharacterUI().SetZoomInSideCharDialogueText(dialogueText);
                        GameUIService.GetCharacterUI().SetZoomInSideCharDialogueBoxActive(true);
                    }
                    GameUIService.GetCharacterUI().SetZoomInSideCharacterActive(true);
                }
                GameUIService.GetCharacterUI().SetZoomInCharacterActive(true);
            }
            else
            {
                if (characterType == CharacterType.Main && isDialogueBoxActive)
                {
                    GameUIService.GetCharacterUI().SetZoomOutMainCharDialogueText(dialogueText);
                    GameUIService.GetCharacterUI().SetZoomOutMainCharDialogueBoxActive(true);
                }
                GameUIService.GetCharacterUI().SetZoomOutMainCharacterActive(true);

                if (isSideCharacterEnable)
                {
                    if (characterType == CharacterType.Side && isDialogueBoxActive)
                    {
                        GameUIService.GetCharacterUI().SetZoomOutSideCharDialogueText(dialogueText);
                        GameUIService.GetCharacterUI().SetZoomOutSideCharDialogueBoxActive(true);
                    }
                    GameUIService.GetCharacterUI().SetZoomOutSideCharacterActive(true);
                }
                GameUIService.GetCharacterUI().SetZoomOutCharacterActive(true);
            }
        }

        public void SetStorySceneButtons(Button nextButton)
        {
            nextButton.onClick.AddListener(ExitScene);
        }

        public void CompleteStoryScene()
        {
            GameUIService.GetCharacterUI().ResetCharacterUI();

            GameUIService.SetStorySceneNarrationBoxActive(false);
            GameUIService.SetStartStoryUIGameobjectActive(false);
        }

        public override void ExitScene()
        {
            CompleteStoryScene();
            scenario.IncreamentCurrentScene();
        }
    }
}