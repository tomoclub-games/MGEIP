using MGEIP.GameData.ScenarioData;
using MGEIP.Service;
using System.Collections.Generic;
using UnityEngine;

namespace MGEIP.Scenario
{
    public class ScenarioManager : MonoBehaviour
    {
        [SerializeField] private List<Scenario> scenarios = new();
        [SerializeField] private Scenario scenarioPrefab;
        [SerializeField] private int startingScenarioIndex = 0;
        [SerializeField] private int currentScenarioIndex;

        private GameService gameService;
        public ScenarioDataContainer ScenariosDataContainer => gameService.GameDataContainer.ScenarioDataContainer;

        public void InitializeScenarioManager(GameService gameService)
        {
            this.gameService = gameService;
            List<ScenarioData> scenarioData = ScenariosDataContainer.ScenarioContent.Scenarios;

            for (int i = 0; i < scenarioData.Count; i++)
            {
                CreateScenario(scenarioData[i]);
            }
        }

        private void CreateScenario(ScenarioData scenarioData)
        {
            Scenario scenario = Instantiate<Scenario>(scenarioPrefab);
            scenario.SetScenarioInfo(scenarioData.ScenarioNo, scenarioData.ScenarioName, gameService);
            scenarios.Add(scenario);
            scenario.gameObject.transform.SetParent(transform, false);
        }

        /*public void SetScnerioManagerInfo()
        {
            currentScenarioIndex = startingScenarioIndex;

            InitializeTotalRewardPoint();
            SetCurrentAct(currentActIndex);
        }*/
    }
}