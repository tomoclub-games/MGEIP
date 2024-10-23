using System;
using System.Collections.Generic;
using UnityEngine;

namespace MGIEP.Data
{
    [SerializeField]
    public class SessionInfo
    {
        public string loginToken;
        public int sessionNo;
        public DateTime sessionStartTime;
        public DateTime sessionEndTime;
        public double sessionDuration;
        public List<Attempt> attempts;

        public SessionInfo(string _loginToken, int _sessionNo, int _attemptNo)
        {
            Debug.Log("Initializing sessionInfo : " + _sessionNo);

            loginToken = _loginToken;
            sessionNo = _sessionNo;
            sessionStartTime = DateTime.UtcNow;
            attempts = new List<Attempt>();

            Attempt newAttempt = new Attempt(_attemptNo);
            attempts.Add(newAttempt);
        }
    }

    [Serializable]
    public class Attempt
    {
        public int attemptNo;
        public List<int> completedScenarios;

        public Attempt(int _attemptNo)
        {
            attemptNo = _attemptNo;
            completedScenarios = new List<int>();
        }
    }

    [Serializable]
    public class AttemptInfo
    {
        public string loginToken;
        public int attemptNo;
        public bool[] completedScenarios;
        public List<ScenarioInfo> scenarioList;

        public AttemptInfo(string _loginToken)
        {
            loginToken = _loginToken;
            attemptNo = 1;
            completedScenarios = new bool[10];
            scenarioList = new List<ScenarioInfo>();
        }

        public void PrintAttemptInfo()
        {
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
        public double scenarioDuration;
        public List<Question> questions;

        public ScenarioInfo()
        {
            scenarioNo = 0;
            scenarioTitle = "";
            questions = new List<Question>();
        }

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
        protected bool answerSelected;

        public bool AnswerSelected => answerSelected;

        public void SetAnswerSelected()
        {
            answerSelected = true;
        }

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
