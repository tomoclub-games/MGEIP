using System.Collections;
using System.Collections.Generic;
using MGEIP.Service;
using MGIEP;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MGEIP.Scenario
{
    public class ScenarioButton : MonoBehaviour
    {
        [SerializeField] private int scenarioNo;
        [SerializeField] private GameObject onHoverGO;
        [SerializeField] private GameObject exclamationMark;
        [SerializeField] private GameObject checkMark;

        private ScenarioManager scenarioManager;
        private bool hasCompleted;

        public void Init(ScenarioManager _scenarioManager)
        {
            scenarioManager = _scenarioManager;

            SetUnchecked();
        }

        private void OnMouseOver()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            onHoverGO.SetActive(true);

            exclamationMark.SetActive(false);
            checkMark.SetActive(false);
        }

        private void OnMouseExit()
        {
            onHoverGO.SetActive(false);

            if (hasCompleted)
                SetChecked();
            else
                SetUnchecked();
        }

        private void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject() || hasCompleted)
                return;

            SoundManagerService.Instance.OnStopVoiceOver?.Invoke();
            scenarioManager.OnClickScenarioButton(scenarioNo);
        }

        public void SetUnchecked()
        {
            exclamationMark.SetActive(true);
            checkMark.SetActive(false);
        }

        public void SetChecked()
        {
            hasCompleted = true;

            exclamationMark.SetActive(false);
            checkMark.SetActive(true);
        }
    }
}
