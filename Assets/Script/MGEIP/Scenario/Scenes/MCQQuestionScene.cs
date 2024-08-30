﻿using MGEIP.GameData.SceneData;
using MGEIP.Service;
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

        public void SetMCQQuestionSceneInfo()
        {
            characterType = sceneData.CharacterType;
            isDialogueBoxActive = sceneData.DialogueBox;  
            dialogueText = sceneData.DialogueText; 
            questionText = sceneData.QuestionText; 
            optionText1 = sceneData.Option1; 
            optionText2 = sceneData.Option2; 
            optionText3 = sceneData.Option3; 
            optionText4 = sceneData.Option4; 
        }

        public void MCQQuestionSceneInfo()
        {
            if (characterType == CharacterType.Main && isDialogueBoxActive)
            {
                GameUIService.GetCharacterUI().SetZoomInMainCharDialogueText(dialogueText);
                GameUIService.GetCharacterUI().SetZoomInMainCharDialogueBoxActive(true);
            }

            GameUIService.SetQuestionText(questionText);
            SetOptionText();
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
            GameUIService.GetCharacterUI().SetZoomInMainCharDialogueBoxActive(false);
        }
    }
}