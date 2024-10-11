using NorskaLib.Spreadsheets;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace MGEIP.GameData.ScenarioData
{
    [CreateAssetMenu(fileName = "InstructionDataContainer", menuName = "MGEIP Spreadsheet Container/ InstructionData", order = 2)]
    public class InstructionDataContainer : SpreadsheetsContainerBase
    {
        [SpreadsheetContent]
        [SerializeField] private InstructionContent instructionContent;

        public InstructionContent InstructionContent => instructionContent;
    }

    [Serializable]
    public class InstructionContent
    {
        [SpreadsheetPage("Instructions Dump")]
        public List<InstructionData> InstructionDataList;
    }

    [Serializable]
    public class InstructionData
    {
        public string InstructionMessage;
    }
}