using MGEIP.Scenario.Scenes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MGEIP.GameData.ScenarioData
{
    [CreateAssetMenu(fileName = "ScenarioResourceContainer", menuName = "MGEIP Spreadsheet Container/ ScenarioResource", order = 6)]
    public class ResourceContainer : ScriptableObject
    {
        public List<SceneContainer> SceneContainer;
    }

    [Serializable]
    public class SceneContainer
    {
        public int scenarioNo;
        public List<ScenePrefab> ScenePrefab;
    }

    [Serializable]
    public class ScenePrefab
    {
        public int sceneNo;
        public Scene scene;
    }
}