﻿using MGEIP.GameData.ScenarioData;
using MGEIP.Service;
using MGIEP.Data;
using System.Collections.Generic;
using UnityEngine;

namespace MGEIP.Scenario
{
    public class ScenarioManager : MonoBehaviour
    {
        [SerializeField] private List<ScenarioButton> scenarioButtons;
        [SerializeField] private List<Scenario> scenarios;
        [SerializeField] private Scenario scenarioPrefab;
        [SerializeField] private GameObject scenarioHolder;

        [SerializeField] private int currentScenarioIndex;

        private int selectedScenarioIndex = -1;
        private int completedScenarios;
        private GameService gameService;
        public ScenarioDataContainer ScenariosDataContainer => gameService.GameDataContainer.ScenarioDataContainer;

        public void InitializeScenarioManager(GameService gameService)
        {
            this.gameService = gameService;

            List<ScenarioInfo> scenarioInfoList = ScenariosDataContainer.ScenarioContent.Scenarios;

            for (int i = 0; i < scenarioButtons.Count; i++)
            {
                scenarioButtons[i].Init(this);
            }

            for (int i = 0; i < scenarioInfoList.Count; i++)
            {
                CreateScenario(scenarioInfoList[i]);
            }

            /*
            for(int i = 0; i < scenarios.Count; i++)
            {
                int index = i;
                scenarios[i].ScenarioButton.onClick.AddListener(() => OnClickScenarioButton(index));
                scenarios[i].ScenarioPlayButton.onClick.AddListener(OnScenarioPlayButtonClick);
            }
            */

            gameService.GameUIService.OnScenarioStart += OnScenarioPlayButtonClick;
        }

        public void OnClickScenarioButton(int index)
        {
            /*
            if (selectedScenarioIndex != -1 && selectedScenarioIndex != index)
            {
                DisableSelectedScenario();
            }

            scenarios[index].ScenarioIndicator.SetActive(false);
            scenarios[index].ScenarioData.SetActive(true);

            selectedScenarioIndex = index;
            */

            string scenarioName = ScenariosDataContainer.ScenarioContent.Scenarios[index - 1].ScenarioName;
            string scenarioDesc = ScenariosDataContainer.ScenarioContent.Scenarios[index - 1].Description;
            gameService.GameUIService.EnableScenarioInfo(index, scenarioName, scenarioDesc);
        }

        public void OnScenarioPlayButtonClick(int _index)
        {
            SetCurrentScenario(_index - 1);
        }

        private void CreateScenario(ScenarioInfo scenarioInfo)
        {
            Scenario scenario = Instantiate<Scenario>(scenarioPrefab);
            scenario.gameObject.name = "Scenario" + scenarioInfo.ScenarioNo;
            scenario.SetScenarioInfo(scenarioInfo.ScenarioNo, scenarioInfo.ScenarioName, gameService, this);
            scenarios.Add(scenario);
            scenario.transform.SetParent(scenarioHolder.transform, false);

            if (DataHandler.Instance.AttemptData.completedScenarios[scenarioInfo.ScenarioNo - 1])
                SetScenarioComplete(scenarioInfo.ScenarioNo);
        }

        public void SetCurrentScenario(int scenarioIndex)
        {
            if (scenarioIndex >= 0 && scenarioIndex < scenarios.Count)
            {
                currentScenarioIndex = scenarioIndex;
                scenarios[currentScenarioIndex].CreateScene();
                scenarios[currentScenarioIndex].SetSceneStatus(0);

                gameService.GameUIService.MapUI.SetActive(false);
            }
            else
            {
                Debug.LogError("Invalid Scenario index");
            }
        }

        public void SetScenarioComplete(int scenarioIndex)
        {
            // Disabled for review
            /*
            scenarioButtons[scenarioIndex - 1].SetChecked();

            completedScenarios++;

            if (completedScenarios == scenarios.Count)
            {
                gameService.GameUIService.GetGameEndButton.gameObject.SetActive(true);
            }
            */
        }
    }
}