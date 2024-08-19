using MGEIP.GameData.ScenarioData;
using MGEIP.GameData.SceneData;
using UnityEngine;

namespace MGEIP.GameData
{
    [CreateAssetMenu(fileName = "GameDataContainer", menuName = "MGEIP Spreadsheet Container/ GameData", order = 1)]
    public class GameDataContainer : ScriptableObject
    {
        [SerializeField] private ScenarioDataContainer scenarioDataContainer;
        [SerializeField] private SceneDataContainer sceneDataContainer;

        public ScenarioDataContainer ScenarioDataContainer => scenarioDataContainer;
        public SceneDataContainer SceneDataContainer => sceneDataContainer;
    }
}