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
        [SerializeField] private GameObject[] onHoverGOs;
        [SerializeField] private GameObject exclamationMark;
        [SerializeField] private GameObject checkMark;
        [SerializeField] private Vector3 hoveredOnScale;

        private ScenarioManager scenarioManager;
        private bool hasCompleted;

        private Vector3 originalScale;
        private Vector3 hoverScale;

        public void Init(ScenarioManager _scenarioManager)
        {
            scenarioManager = _scenarioManager;
            originalScale = transform.localScale;
            hoverScale = originalScale + hoveredOnScale;
            SetUnchecked();
        }

        private void OnMouseOver()
        {
            if (EventSystem.current.IsPointerOverGameObject() || hasCompleted)
                return;

            foreach (GameObject go in onHoverGOs)
            {
                go.SetActive(true);
            }

            // transform.localScale = hoverScale;

            exclamationMark.SetActive(false);
            checkMark.SetActive(false);
        }

        private void OnMouseExit()
        {
            if (hasCompleted)
                return;

            foreach (GameObject go in onHoverGOs)
            {
                go.SetActive(false);
            }

            // transform.localScale = originalScale;

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
