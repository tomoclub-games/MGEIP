using System;
using System.Collections.Generic;
using UnityEngine;

namespace MGEIP.GameData.ScenarioData
{
    [CreateAssetMenu(fileName = "ScenarioArtDataContainer", menuName = "MGEIP Spreadsheet Container/ ScenarioArtData", order = 3)]
    public class ScenarioArtDataContainer : ScriptableObject
    {
        public List<ScenarioArt> ScenarioArt;
    }

    [Serializable]
    public class ScenarioArt
    {
        public int scenarioNo;
        public Vector2 minAnchorValue;
        public Vector2 maxAnchorValue;

        public Sprite scenarioIconSprite;
    }
}