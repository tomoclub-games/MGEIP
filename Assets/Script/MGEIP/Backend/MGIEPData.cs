using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace MGIEP.Data
{
    [Serializable]
    public class PlayerData
    {
        public string loginToken;
        public string metadata;
        public string playerName;
        public string playerEmail;
        public DateTime playerDOB;
        public string playerGender;

        public PlayerData(string _loginToken, string _metadata, string _playerName, string _playerEmail, DateTime? _playerDOB, string _playerGender)
        {
            loginToken = _loginToken;
            metadata = _metadata;
            playerName = _playerName;
            playerEmail = _playerEmail;
            playerDOB = (DateTime)_playerDOB;
            playerGender = _playerGender;
        }
    }

    [Serializable]
    public class SessionData
    {
        public string loginToken;
        public int sessionNo;
        public DateTime sessionStartTime;
        public DateTime sessionEndTime;
        public double sessionDuration;
        public List<SessionAttempt> attempts;

        public SessionData(string _loginToken, int _sessionNo, int _attemptNo)
        {
            Debug.Log("Initializing sessionInfo : " + _sessionNo);

            loginToken = _loginToken;
            sessionNo = _sessionNo;
            sessionStartTime = DateTime.UtcNow;
            attempts = new List<SessionAttempt>();

            SessionAttempt newAttempt = new SessionAttempt(_attemptNo);
            attempts.Add(newAttempt);
        }
    }

    [Serializable]
    public class SessionAttempt
    {
        public int attemptNo;
        public List<int> completedScenarios;

        public SessionAttempt(int _attemptNo)
        {
            attemptNo = _attemptNo;
            completedScenarios = new List<int>();
        }
    }

    [Serializable]
    public class AttemptData
    {
        public string loginToken;
        public int attemptNo;
        public bool[] completedScenarios;
        public List<ScenarioData> scenarioList;

        public AttemptData(string _loginToken)
        {
            loginToken = _loginToken;
            attemptNo = 1;
            completedScenarios = new bool[10];
            scenarioList = new List<ScenarioData>();
        }

        public void PrintAttemptInfo()
        {
            foreach (ScenarioData scenarioInfo in scenarioList)
            {
                scenarioInfo.PrintScenarioInfo();
            }
        }
    }

    [Serializable]
    public class ScenarioData
    {
        public int scenarioNo;
        public string scenarioTitle;
        public double scenarioDuration;
        public List<Question> questions;

        public ScenarioData()
        {
            scenarioNo = 0;
            scenarioTitle = "";
            questions = new List<Question>();
        }

        public ScenarioData(int _scenarioNo, string _scenarioTitle)
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

    [Serializable, JsonObject]
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

    [Serializable, JsonObject]
    public class MultipleChoiceQuestion : Question
    {
        public List<string> options;
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
            Debug.Log("Selected Answer : " + selectedAnswer);
        }
    }

    [Serializable, JsonObject]
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
