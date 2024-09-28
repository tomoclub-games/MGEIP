using MGEIP.GameData.SceneData;
using MGEIP.Service;
using MGIEP.Data;

namespace MGEIP.Scenario.Scenes
{
    public class CESliderQuestionScene : QuestionScene
    {
        private SliderQuestion sliderQuestion;

        private int questionNo;
        private int currentAnswer;
        private int selectedAnswer;

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

        public override void InitializeQuestionScene(int scenarioNo, SceneData sceneData, Scenario scenario, GameService gameService)
        {
            base.InitializeQuestionScene(scenarioNo, sceneData, scenario, gameService);

            sliderQuestion = new SliderQuestion();
            sliderQuestion.sceneNo = sceneData.SceneNo;
            sliderQuestion.questionText = sceneData.QuestionText;

            gameService.DataHandler.MGIEPData.scenarioList[scenarioNo - 1].questions.Add(sliderQuestion);
            questionNo = gameService.DataHandler.MGIEPData.scenarioList[scenarioNo - 1].questions.Count;
        }

        public void SetCESliderQuestionSceneInfo()
        {
            isDialogueBoxActive = sceneData.DialogueBox;
            dialogue = sceneData.DialogueText;
            isNarrationBoxActive = sceneData.NarrationBox;
            narrationText = sceneData.NarrationText;
            questionText = sceneData.QuestionText;
        }

        public void CESliderQuestionSceneInfo()
        {
            GameUIService.SetQuestionSceneNarrationBoxActive(isNarrationBoxActive);
            GameUIService.SetQuestionSceneNarrationText(narrationText);

            dialogueBox.SetActive(isDialogueBoxActive);
            dialogueText.SetText(dialogue);

            string questionTextTemp = questionText;
            questionTextTemp = questionTextTemp.Replace("<emotion>", scenario.EmotionKeyword);

            GameUIService.SetQuestionText(questionTextTemp);
            GameUIService.SetSliderToDefault();

            sliderQuestion.questionText = questionTextTemp;

            if (sliderQuestion.AnswerSelected)
                GameUIService.SetSliderValue(selectedAnswer);
            else
                GameUIService.SetSliderToDefault();

            GameUIService.OnSliderAnswerSelect += SliderSelect;
            GameUIService.OnConfirmButtonClick += ConfirmAnswer;
        }

        public override void CompleteQuestionScene()
        {
            base.CompleteQuestionScene();
            GameUIService.SetSliderPanelActive(false);
            GameUIService.CELabelGameobject.SetActive(false);

            GameUIService.OnSliderAnswerSelect -= SliderSelect;
            GameUIService.OnConfirmButtonClick -= ConfirmAnswer;
        }

        private void SliderSelect(int _selectedAnswer)
        {
            currentAnswer = _selectedAnswer;
        }

        private void ConfirmAnswer()
        {
            selectedAnswer = currentAnswer;
            sliderQuestion.selectedAnswer = selectedAnswer;
            sliderQuestion.SetAnswerSelected();
        }
    }
}