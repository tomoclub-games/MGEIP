﻿using MGEIP.GameData.SceneData;
using MGEIP.Service;
using MGIEP.Data;

namespace MGEIP.Scenario.Scenes
{
    public class AESliderQuestionScene : QuestionScene
    {
        private SliderQuestion sliderQuestion;
        private int questionNo;

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

        public override void InitializeQuestionScene(int scenarioNo, SceneData sceneData, Scenario scenario, GameService gameService)
        {
            base.InitializeQuestionScene(scenarioNo, sceneData, scenario, gameService);

            sliderQuestion = new SliderQuestion();
            sliderQuestion.sceneNo = sceneData.SceneNo;
            sliderQuestion.questionText = sceneData.QuestionText;

            gameService.DataHandler.MGIEPData.scenarioList[scenarioNo - 1].questions.Add(sliderQuestion);
            questionNo = gameService.DataHandler.MGIEPData.scenarioList[scenarioNo - 1].questions.Count;
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

            GameUIService.OnSliderAnswerSelect += SliderSelect;
        }

        public override void CompleteQuestionScene()
        {
            base.CompleteQuestionScene();
            GameUIService.SetSliderPanelActive(false);
            GameUIService.AELabelGameobject.SetActive(false);

            GameUIService.OnSliderAnswerSelect -= SliderSelect;
        }

        private void SliderSelect(int _selectedAnswer)
        {
            sliderQuestion.selectedAnswer = _selectedAnswer;
        }
    }
}