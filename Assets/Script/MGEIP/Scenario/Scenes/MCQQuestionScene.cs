using MGEIP.GameData.SceneData;
using MGEIP.Service;
using MGIEP.Data;
using System.Linq;
using UnityEngine;
namespace MGEIP.Scenario.Scenes
{
    public class MCQQuestionScene : QuestionScene
    {
        [SerializeField] private string optionText1;
        [SerializeField] private string optionText2;
        [SerializeField] private string optionText3;
        [SerializeField] private string optionText4;

        [SerializeField] private string optionKeywordText1;
        [SerializeField] private string optionKeywordText2;
        [SerializeField] private string optionKeywordText3;
        [SerializeField] private string optionKeywordText4;

        private MultipleChoiceQuestion multipleChoiceQuestion;

        private int questionNo;
        private int currentAnswer;
        private int selectedAnswer;

        private string[] shuffledOptions;
        private string[] shuffledKeywordOptions;

        public override void EnterScene()
        {
            base.EnterScene();
            StartCurrentMCQQuestionScene();
        }

        public void StartCurrentMCQQuestionScene()
        {
            GameUIService.SetOptionPanelActive(true);
            MCQQuestionSceneInfo();
        }

        public override void InitializeQuestionScene(int scenarioNo, SceneData sceneData, Scenario scenario, GameService gameService)
        {
            base.InitializeQuestionScene(scenarioNo, sceneData, scenario, gameService);

            multipleChoiceQuestion = new MultipleChoiceQuestion();

            multipleChoiceQuestion.sceneNo = sceneData.SceneNo;
            multipleChoiceQuestion.questionText = sceneData.QuestionText;

            multipleChoiceQuestion.options.Add(sceneData.Option1);
            multipleChoiceQuestion.options.Add(sceneData.Option2);
            multipleChoiceQuestion.options.Add(sceneData.Option3);
            multipleChoiceQuestion.options.Add(sceneData.Option4);

            gameService.DataHandler.MGIEPData.scenarioList[scenarioNo - 1].questions.Add(multipleChoiceQuestion);
            questionNo = gameService.DataHandler.MGIEPData.scenarioList[scenarioNo - 1].questions.Count;
        }

        public void SetMCQQuestionSceneInfo()
        {
            isDialogueBoxActive = sceneData.DialogueBox;  
            dialogue = sceneData.DialogueText; 
            isNarrationBoxActive = sceneData.NarrationBox;
            narrationText = sceneData.NarrationText;
            questionText = sceneData.QuestionText;
            optionText1 = sceneData.Option1;
            optionText2 = sceneData.Option2;
            optionText3 = sceneData.Option3;
            optionText4 = sceneData.Option4;
            optionKeywordText1 = sceneData.Option1Keyword;
            optionKeywordText2 = sceneData.Option2Keyword;
            optionKeywordText3 = sceneData.Option3Keyword;
            optionKeywordText4 = sceneData.Option4Keyword;

            string[] stringOptions = new string[] { optionText1, optionText2, optionText3, optionText4 };
            string[] stringKeywordOptions = new string[] { optionKeywordText1, optionKeywordText2, optionKeywordText3, optionKeywordText4 };

            int[] indices = Enumerable.Range(0, stringOptions.Length).ToArray();
            indices = indices.OrderBy(x => Random.value).ToArray();

            shuffledOptions = indices.Select(i => stringOptions[i]).ToArray();
            shuffledKeywordOptions = indices.Select(i => stringKeywordOptions[i]).ToArray();
        }

        public void MCQQuestionSceneInfo()
        {
            GameUIService.SetQuestionSceneNarrationBoxActive(isNarrationBoxActive);
            GameUIService.SetQuestionSceneNarrationText(narrationText);

            dialogueBox.SetActive(isDialogueBoxActive);
            dialogueText.SetText(dialogue);

            GameUIService.SetQuestionText(questionText);

            GameUIService.SetOptionText(shuffledOptions);

            if (multipleChoiceQuestion.AnswerSelected)
                GameUIService.SetOptionSelected(selectedAnswer);

            GameUIService.OnMCQOptionSelect += OptionSelect;
            GameUIService.OnConfirmButtonClick += ConfirmAnswer;
        }

        public override void CompleteQuestionScene()
        {
            base.CompleteQuestionScene();

            GameUIService.SetOptionPanelActive(false);

            GameUIService.OnMCQOptionSelect -= OptionSelect;
            GameUIService.OnConfirmButtonClick -= ConfirmAnswer;
        }

        private void OptionSelect(int _optionNo)
        {
            currentAnswer = _optionNo;
        }

        private void ConfirmAnswer()
        {
            selectedAnswer = currentAnswer;

            multipleChoiceQuestion.selectedAnswer = shuffledOptions[selectedAnswer];

            if (sceneData.UseKeyword)
                scenario.SetEmotionKeyword(shuffledKeywordOptions[selectedAnswer]);

            multipleChoiceQuestion.SetAnswerSelected();

            ExitScene();
        }
    }
}