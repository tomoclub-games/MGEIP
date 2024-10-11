using System.Collections;
using NorskaLib.Spreadsheets;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MGEIP.GameData
{
    [CreateAssetMenu(fileName = "MainMenuDataContainer", menuName = "MGEIP Spreadsheet Container/ MainMenu Data", order = 2)]
    public class MainMenuDataContainer : SpreadsheetsContainerBase
    {
        [SpreadsheetContent]
        [SerializeField] private MainMenuContent mainMenuContent;

        public MainMenuContent MainMenuContent => mainMenuContent;
    }

    [Serializable]
    public class MainMenuContent
    {
        [SpreadsheetPage("MainMenu Dump")]
        public List<MainMenuData> mainMenuDataList;
    }

    [Serializable]
    public class MainMenuData
    {
        public string TextContent;
    }
}
