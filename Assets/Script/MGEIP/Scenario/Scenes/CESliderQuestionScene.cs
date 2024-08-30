using MGEIP.GameData.SceneData;
using UnityEngine;
using UnityEngine.UI;

namespace MGEIP.Scenario.Scenes
{
    public class CESliderQuestionScene : QuestionScene
    {
        public override void EnterScene()
        {
            base.EnterScene();
            StartCurrentCESliderQuestionScene();
        }

        public void StartCurrentCESliderQuestionScene()
        {
            GameUIService.SetSliderPanelActive(true);
            GameUIService.CELabelGameobject.SetActive(true);
            CESliderQuestionSceneInfo();
        }

        public void SetCESliderQuestionSceneInfo()
        {
            isDialogueBoxActive = sceneData.DialogueBox;
            dialogueText = sceneData.DialogueText;
            questionText = sceneData.QuestionText;
        }

        public void CESliderQuestionSceneInfo()
        {
            if (characterType == CharacterType.Main && isDialogueBoxActive)
            {
                GameUIService.GetCharacterUI().SetZoomInMainCharDialogueText(dialogueText);
                GameUIService.GetCharacterUI().SetZoomInMainCharDialogueBoxActive(true);
            }

            GameUIService.SetQuestionText(questionText);
        }

        public override void CompleteQuestionScene()
        {
            base.CompleteQuestionScene();
            GameUIService.SetSliderPanelActive(false);
            GameUIService.CELabelGameobject.SetActive(false);
            GameUIService.GetCharacterUI().SetZoomInMainCharDialogueBoxActive(false);
        }
    }
}