using MGEIP.GameData.ScenarioData;
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

        private MultipleChoiceQuestion multipleChoiceQuestion;
        private int questionNo;

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
        }

        public void MCQQuestionSceneInfo()
        {
            GameUIService.SetQuestionSceneNarrationBoxActive(isNarrationBoxActive);
            GameUIService.SetQuestionSceneNarrationText(narrationText);

            dialogueBox.SetActive(isDialogueBoxActive);
            dialogueText.SetText(dialogue);

            GameUIService.SetQuestionText(questionText);
            SetOptionText();

            GameUIService.OnMCQOptionSelect += OptionSelect;
        }

        private void SetOptionText()
        {
            string[] stringOptions = new string[] { optionText1, optionText2, optionText3, optionText4 };
            string[] shuffledOptions = stringOptions.OrderBy(x => Random.value).ToArray();

            GameUIService.SetOptionText(shuffledOptions);
        }

        public override void CompleteQuestionScene()
        {
            base.CompleteQuestionScene();

            GameUIService.SetOptionPanelActive(false);

            GameUIService.OnMCQOptionSelect -= OptionSelect;
        }

        private void OptionSelect(int _optionNo)
        {
            multipleChoiceQuestion.selectedAnswer = multipleChoiceQuestion.options[_optionNo];
        }
    }
}