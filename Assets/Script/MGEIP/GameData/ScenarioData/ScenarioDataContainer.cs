using NorskaLib.Spreadsheets;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace MGEIP.GameData.ScenarioData
{
    [CreateAssetMenu(fileName = "ScenarioDataContainer", menuName = "MGEIP Spreadsheet Container/ ScenarioData", order = 2)]
    public class ScenarioDataContainer : SpreadsheetsContainerBase
    {
        [SpreadsheetContent]
        [SerializeField] private ScenarioContent scenarioContent;
        public ScenarioContent ScenarioContent { get { return scenarioContent; } }
    }

    [Serializable]
    public class ScenarioContent
    {
        [SpreadsheetPage("Scenarios Dump")]
        public List<ScenarioInfo> Scenarios;
    }

    [Serializable]
    public class ScenarioInfo
    {
        public int ScenarioNo;
        public string ScenarioName;
        public string Description;
    }
}