using MGEIP.GameData.ScenarioData;
using MGEIP.Service;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MGEIP.Scenario
{
    public class ScenarioManager : MonoBehaviour
    {
        [SerializeField] private List<Scenario> scenarios;
        [SerializeField] private Scenario scenarioPrefab;
        [SerializeField] private GameObject scenarioHolder;

        [SerializeField] private int currentScenarioIndex;

        private int selectedScenarioIndex = -1;
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

            for(int i = 0; i < scenarios.Count; i++)
            {
                int index = i;
                scenarios[i].ScenarioButton.onClick.AddListener(() => OnClickScenarioButton(index));
                scenarios[i].ScenarioPlayButton.onClick.AddListener(OnScenarioPlayButtonClick);
            }
        }

        private void Update()
        {
            // Check for touch or mouse input
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 touchPosition = Input.mousePosition;

                // Check if touch is not on any scenario button
                bool touchedOnScenarioButton = false;
                for (int i = 0; i < scenarios.Count; i++)
                {
                    RectTransform buttonRect = scenarios[i].ScenarioButton.GetComponent<RectTransform>();
                    if (RectTransformUtility.RectangleContainsScreenPoint(buttonRect, touchPosition, Camera.main))
                    {
                        touchedOnScenarioButton = true;
                        break;
                    }
                }

                // If no scenario button was touched and a scenario is selected, disable it
                if (!touchedOnScenarioButton && selectedScenarioIndex != -1)
                {
                    DisableSelectedScenario();
                }
            }

            if (scenarios.All(scenario => scenario.isScenarioCompleted))
            {
                gameService.GameUIService.GetGameEndButton.gameObject.SetActive(true);
            }
        }

        public void DisableSelectedScenario()
        {
            if (selectedScenarioIndex != -1)
            {
                scenarios[selectedScenarioIndex].ScenarioIndicator.SetActive(true);
                scenarios[selectedScenarioIndex].ScenarioInfo.SetActive(false);

                selectedScenarioIndex = -1;
            }
        }

        public void OnClickScenarioButton(int index)
        {
            if (selectedScenarioIndex != -1 && selectedScenarioIndex != index)
            {
                DisableSelectedScenario();
            }

            scenarios[index].ScenarioIndicator.SetActive(false);
            scenarios[index].ScenarioInfo.SetActive(true);

            selectedScenarioIndex = index;
        }

        public void OnScenarioPlayButtonClick()
        {
            gameService.GameUIService.MapUI.SetActive(false);
            SetCurrentScenario(selectedScenarioIndex);
        }

        private void CreateScenario(ScenarioData scenarioData)
        {
            Scenario scenario = Instantiate<Scenario>(scenarioPrefab);
            scenario.gameObject.name = "Scenario" + scenarioData.ScenarioNo;
            scenario.SetScenarioInfo(scenarioData.ScenarioNo, scenarioData.ScenarioName, gameService);
            scenarios.Add(scenario);
            scenario.transform.SetParent(scenarioHolder.transform, false);
        }

        public void SetCurrentScenario(int scenarioIndex)
        {
            if (scenarioIndex >= 0 && scenarioIndex < scenarios.Count)
            {
                currentScenarioIndex = scenarioIndex;
                scenarios[currentScenarioIndex].CreateScene();
                scenarios[currentScenarioIndex].SetSceneStatus(0);
            }
            else
            {
                Debug.LogError("Invalid Scenario index");
            }
        }
    }
}