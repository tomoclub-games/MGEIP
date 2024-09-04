using MGEIP.GameData.ScenarioData;
using MGEIP.GameData.SceneData;
using UnityEngine;

namespace MGEIP.GameData
{
    [CreateAssetMenu(fileName = "GameDataContainer", menuName = "MGEIP Spreadsheet Container/ GameData", order = 1)]
    public class GameDataContainer : ScriptableObject
    {
        [SerializeField] private ScenarioDataContainer scenarioDataContainer;
        [SerializeField] private ScenarioArtDataContainer scenarioArtDataContainer;
        [SerializeField] private SceneDataContainer sceneDataContainer;
        [SerializeField] private ResourceContainer scenarioResourceContainer;

        public ScenarioDataContainer ScenarioDataContainer => scenarioDataContainer;
        public ScenarioArtDataContainer ScenarioArtDataContainer => scenarioArtDataContainer;
        public SceneDataContainer SceneDataContainer => sceneDataContainer;
        public ResourceContainer ScenarioResourceContainer => scenarioResourceContainer;
    }
}