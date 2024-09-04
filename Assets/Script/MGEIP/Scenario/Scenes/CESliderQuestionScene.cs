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
            dialogue = sceneData.DialogueText;
            questionText = sceneData.QuestionText;
        }

        public void CESliderQuestionSceneInfo()
        {
            dialogueBox.SetActive(isDialogueBoxActive);
            dialogueText.SetText(dialogue);

            GameUIService.SetQuestionText(questionText);
        }

        public override void CompleteQuestionScene()
        {
            base.CompleteQuestionScene();
            GameUIService.SetSliderPanelActive(false);
            GameUIService.CELabelGameobject.SetActive(false);
        }
    }
}