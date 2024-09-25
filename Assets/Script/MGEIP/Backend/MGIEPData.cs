using System;
using System.Collections.Generic;
using UnityEngine;

namespace MGIEP.Data
{
    [Serializable]
    public class MGIEPData
    {
        public string playerName;
        public List<ScenarioInfo> scenarioList;

        public MGIEPData(string _playerName)
        {
            playerName = _playerName;
            scenarioList = new List<ScenarioInfo>();
        }

        public void PrintMGIEPData()
        {
            Debug.Log("Player Name : " + playerName);
            foreach (ScenarioInfo scenarioInfo in scenarioList)
            {
                scenarioInfo.PrintScenarioInfo();
            }
        }
    }

    [Serializable]
    public class ScenarioInfo
    {
        public int scenarioNo;
        public string scenarioTitle;
        public List<Question> questions;

        public ScenarioInfo(int _scenarioNo, string _scenarioTitle)
        {
            scenarioNo = _scenarioNo;
            scenarioTitle = _scenarioTitle;
            questions = new List<Question>();
        }

        public void PrintScenarioInfo()
        {
            Debug.Log($"Scenario No : {scenarioNo} Scenario Title : {scenarioTitle}");

            foreach (Question question in questions)
                question.PrintQuestion();
        }
    }

    [Serializable]
    public class Question
    {
        public int sceneNo;
        public string questionText;

        public virtual void PrintQuestion()
        {
            Debug.Log("Scene No : " + sceneNo + " Question : " + questionText);
        }
    }

    [Serializable]
    public class MultipleChoiceQuestion : Question
    {
        public List<string> options;
        public string correctAnswer;
        public string selectedAnswer;

        public MultipleChoiceQuestion()
        {
            options = new List<string>();
        }

        public override void PrintQuestion()
        {
            base.PrintQuestion();

            Debug.Log("Options : ");
            foreach (string option in options)
            {
                Debug.Log(option);
            }
            Debug.Log("Correct Answer : " + correctAnswer);
            Debug.Log("Selected Answer : " + selectedAnswer);
        }
    }

    [Serializable]
    public class SliderQuestion : Question
    {
        public int selectedAnswer;

        public override void PrintQuestion()
        {
            base.PrintQuestion();

            Debug.Log("Selected Answer : " + selectedAnswer);
        }
    }
}
