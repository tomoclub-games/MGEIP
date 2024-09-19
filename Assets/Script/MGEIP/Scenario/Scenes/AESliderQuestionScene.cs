using MGEIP.Service;

namespace MGEIP.Scenario.Scenes
{
    public class AESliderQuestionScene : QuestionScene
    {
        public override void EnterScene()
        {
            base.EnterScene();
            StartCurrentAESliderQuestionScene();
        }

        public void StartCurrentAESliderQuestionScene()
        {
            GameUIService.SetSliderPanelActive(true);
            GameUIService.AELabelGameobject.SetActive(true);
            AESliderQuestionSceneInfo();
        }

        public void SetAESliderQuestionSceneInfo()
        {
            isDialogueBoxActive = sceneData.DialogueBox;
            dialogue = sceneData.DialogueText;
            isNarrationBoxActive = sceneData.NarrationBox;
            narrationText = sceneData.NarrationText;
            questionText = sceneData.QuestionText;
        }

        public void AESliderQuestionSceneInfo()
        {
            GameUIService.SetQuestionSceneNarrationBoxActive(isNarrationBoxActive);
            GameUIService.SetQuestionSceneNarrationText(narrationText);

            dialogueBox.SetActive(isDialogueBoxActive);
            dialogueText.SetText(dialogue);

            GameUIService.SetQuestionText(questionText);
        }

        public override void CompleteQuestionScene()
        {
            base.CompleteQuestionScene();
            GameUIService.SetSliderPanelActive(false);
            GameUIService.AELabelGameobject.SetActive(false);
        }
    }
}