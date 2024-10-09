using MGEIP.GameData.SceneData;
using MGEIP.Service;
using MGIEP;
using MGIEP.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace MGEIP.Scenario.Scenes
{
    public class MCQQuestionScene : QuestionScene
    {
        private List<string> optionTexts = new();
        private List<string> optionKeywords = new();

        private MultipleChoiceQuestion multipleChoiceQuestion;

        private int questionNo;
        private int currentAnswer;
        private int selectedAnswer;

        private bool hasOptionBeenSelected;

        private string[] shuffledOptions;
        private string[] shuffledKeywordOptions;

        private int VOkeywordIndex;

        public override void EnterScene()
        {
            base.EnterScene();
            StartCurrentMCQQuestionScene();

            for (int i = 0; i < 4; i++)
            {
                int index = i;
                GameUIService.OptionVOButtons[i].Button.onClick.AddListener(() => PlayOptionVoiceOver(index));
            }
        }

        public void StartCurrentMCQQuestionScene()
        {
            GameUIService.SetOptionPanelActive(true);
            MCQQuestionSceneInfo();
        }

        public override void InitializeQuestionScene(int scenarioNo, SceneData sceneData, Scenario scenario, GameService gameService, ScenarioInfo scenarioInfo)
        {
            base.InitializeQuestionScene(scenarioNo, sceneData, scenario, gameService, scenarioInfo);

            multipleChoiceQuestion = new MultipleChoiceQuestion();

            multipleChoiceQuestion.sceneNo = sceneData.SceneNo;
            multipleChoiceQuestion.questionText = UtilityService.RemoveRichTextTags(sceneData.QuestionText);

            multipleChoiceQuestion.options.Add(UtilityService.RemoveRichTextTags(sceneData.Option1));
            multipleChoiceQuestion.options.Add(UtilityService.RemoveRichTextTags(sceneData.Option2));
            multipleChoiceQuestion.options.Add(UtilityService.RemoveRichTextTags(sceneData.Option3));
            multipleChoiceQuestion.options.Add(UtilityService.RemoveRichTextTags(sceneData.Option4));

            this.scenarioInfo.questions.Add(multipleChoiceQuestion);
            questionNo = this.scenarioInfo.questions.Count;
        }

        public void SetMCQQuestionSceneInfo()
        {
            isDialogueBoxActive = sceneData.DialogueBox;  
            dialogue = sceneData.DialogueText; 
            isNarrationBoxActive = sceneData.NarrationBox;
            narrationText = sceneData.NarrationText;
            questionText = sceneData.QuestionText;

            optionTexts.Add(sceneData.Option1);
            optionTexts.Add(sceneData.Option2);
            optionTexts.Add(sceneData.Option3);
            optionTexts.Add(sceneData.Option4);

            optionKeywords.Add(sceneData.Option1Keyword);
            optionKeywords.Add(sceneData.Option2Keyword);
            optionKeywords.Add(sceneData.Option3Keyword);
            optionKeywords.Add(sceneData.Option4Keyword);

            string[] stringOptions = optionTexts.ToArray();
            string[] stringKeywordOptions = optionKeywords.ToArray();

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

            if (multipleChoiceQuestion.AnswerSelected)
            {
                GameUIService.OnConfirmButtonClick += ConfirmAnswer;
                hasOptionBeenSelected = true;
            }
            else
                hasOptionBeenSelected = false;
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

            if (!hasOptionBeenSelected)
            {
                GameUIService.OnConfirmButtonClick += ConfirmAnswer;
                hasOptionBeenSelected = true;
            }
        }

        private void ConfirmAnswer()
        {
            selectedAnswer = currentAnswer;

            multipleChoiceQuestion.selectedAnswer = UtilityService.RemoveRichTextTags(shuffledOptions[selectedAnswer]);

            if (sceneData.UseKeyword)
            {
                int keywordIndex = optionKeywords.FindIndex(i => i == shuffledKeywordOptions[selectedAnswer]);
                scenario.SetEmotionKeyword(keywordIndex, shuffledKeywordOptions[selectedAnswer]);
            }

            multipleChoiceQuestion.SetAnswerSelected();

            ExitScene();
        }

        public override void ExitScene()
        {
            base.ExitScene();

            for (int i = 0; i < 4; i++)
            {
                GameUIService.OptionVOButtons[i].Button.onClick.RemoveAllListeners();
            }
        }

        public override void ExitToPrevScene()
        {
            base.ExitToPrevScene();

            for (int i = 0; i < 4; i++)
            {
                GameUIService.OptionVOButtons[i].Button.onClick.RemoveAllListeners();
            }
        }

        public void PlayOptionVoiceOver(int _optionNo)
        {
            VOkeywordIndex = optionTexts.FindIndex(i => i == shuffledOptions[_optionNo]);

            string narrationClipName = $"Scenarios/op_{VOkeywordIndex + 1}_{scenarioNo}_{sceneData.SceneNo}";
            GameUIService.OptionVOButtons[_optionNo].PlayAudioClip(narrationClipName);
        }
    }
}