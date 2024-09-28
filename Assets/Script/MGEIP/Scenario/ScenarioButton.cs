using System.Collections;
using System.Collections.Generic;
using MGEIP.Service;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MGEIP.Scenario
{
    public class ScenarioButton : MonoBehaviour
    {
        [SerializeField] private int scenarioNo;
        [SerializeField] private GameObject onHoverGO;

        private ScenarioManager scenarioManager;

        public void Init(ScenarioManager _scenarioManager)
        {
            scenarioManager = _scenarioManager;
        }

        private void OnMouseOver()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            onHoverGO.SetActive(true);
        }

        private void OnMouseExit()
        {
            onHoverGO.SetActive(false);
        }

        private void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            scenarioManager.OnClickScenarioButton(scenarioNo);
        }
    }
}
