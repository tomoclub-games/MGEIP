using MGEIP.GameData.SceneData;
using MGEIP.Service;
using MGIEP;
using MGIEP.Data;
using UnityEngine;

namespace MGEIP.Scenario.Scenes
{
    public class AESliderQuestionScene : QuestionScene
    {
        private SliderQuestion sliderQuestion;

        private int questionNo;
        private int currentAnswer;

        private bool hasSliderMoved;

        public override void EnterScene()
        {
            base.EnterScene();
            StartCurrentAESliderQuestionScene();
        }

        public void StartCurrentAESliderQuestionScene()
        {
            GameUIService.SetSliderPanelActive(true);
            GameUIService.AELabelGameobject.SetActive(true);
            GameUIService.SetSliderInstructionSetActive(true);
            AESliderQuestionSceneInfo();
        }

        public override void InitializeQuestionScene(int scenarioNo, SceneData sceneData, Scenario scenario, GameService gameService, ScenarioData scenarioData)
        {
            base.InitializeQuestionScene(scenarioNo, sceneData, scenario, gameService, scenarioData);

            sliderQuestion = new SliderQuestion();
            sliderQuestion.sceneNo = sceneData.SceneNo;
            sliderQuestion.questionText = UtilityService.RemoveRichTextTags(sceneData.QuestionText);

            this.scenarioData.questions.Add(sliderQuestion);
            questionNo = this.scenarioData.questions.Count;
        }

        public void SetAESliderQuestionSceneInfo()
        {
            isDialogueBoxActive = sceneData.DialogueBox;
            dialogue = sceneData.DialogueText;
            isNarrationBoxActive = sceneData.NarrationBox;
            narrationText = sceneData.NarrationText;
            questionText = sceneData.QuestionText;

            sceneSoundPlayer.SetSound(sceneData.BGSound, sceneData.SceneSound);
        }

        public void AESliderQuestionSceneInfo()
        {
            GameUIService.SetQuestionSceneNarrationBoxActive(isNarrationBoxActive);
            GameUIService.SetQuestionSceneNarrationText(narrationText);

            dialogueBox.SetActive(isDialogueBoxActive);
            dialogueText.SetText(dialogue);

            GameUIService.SetQuestionText(questionText);
            GameUIService.SetSliderHigherLowerLabels(sceneData.SliderLabelLower, sceneData.SliderLabelHigher);

            if (sliderQuestion.AnswerSelected)
            {
                GameUIService.SetSliderValue(currentAnswer);
                GameUIService.LockSlider();
                GameUIService.EnableConfirmButton();
            }
            else
            {
                GameUIService.UnlockSlider();
                GameUIService.SetSliderToDefault();
                GameUIService.OnSliderAnswerSelect += SliderSelect;
                GameUIService.DisableConfirmButton();
            }

            if (sliderQuestion.AnswerSelected)
            {
                GameUIService.OnConfirmButtonClick += ConfirmAnswer;
                hasSliderMoved = true;
            }
            else
                hasSliderMoved = false;
        }

        public override void CompleteQuestionScene()
        {
            base.CompleteQuestionScene();
            GameUIService.AELabelGameobject.SetActive(false);
            GameUIService.SetSliderPanelActive(false);
            GameUIService.SetSliderInstructionSetActive(false);

            GameUIService.OnSliderAnswerSelect -= SliderSelect;
            GameUIService.OnConfirmButtonClick -= ConfirmAnswer;
        }

        private void SliderSelect(int _selectedAnswer)
        {
            if (currentAnswer != _selectedAnswer)
                SoundManagerService.Instance.OnPlaySFX?.Invoke(SFXType.optionButton);

            currentAnswer = _selectedAnswer;

            if (!hasSliderMoved)
            {
                GameUIService.EnableConfirmButton();
                GameUIService.OnConfirmButtonClick += ConfirmAnswer;
                hasSliderMoved = true;
            }
        }

        private void ConfirmAnswer()
        {
            sliderQuestion.selectedAnswer = currentAnswer;
            sliderQuestion.SetAnswerSelected();

            ExitScene();
        }
    }
}