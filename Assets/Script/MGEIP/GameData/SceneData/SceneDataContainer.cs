using NorskaLib.Spreadsheets;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MGEIP.GameData.SceneData
{
    [CreateAssetMenu(fileName = "SceneDataContainer", menuName = "MGEIP Spreadsheet Container/ SceneData", order = 4)]
    public class SceneDataContainer : SpreadsheetsContainerBase
    {
        [SpreadsheetContent]
        [SerializeField] private SceneContent sceneContent;
        public SceneContent SceneContent { get { return sceneContent; } }
    }

    [Serializable]
    public class SceneContent
    {
        [SpreadsheetPage("Sheet6")]
        public List<SceneData> Scenes;
    }

    [Serializable]
    public class SceneData
    {
        public int ScenarioNo;
        public int SceneNo;
        public SceneType SceneType;
        public bool DialogueBox;
        public bool NarrationBox;
        public string DialogueText;
        public string NarrationText;
        public string QuestionText;
        public string Option1;
        public string Option2;
        public string Option3;
        public string Option4;
        public bool UseKeyword;
        public string Option1Keyword;
        public string Option2Keyword;
        public string Option3Keyword;
        public string Option4Keyword;
        public string SliderLabelLower;
        public string SliderLabelHigher;
    }
}