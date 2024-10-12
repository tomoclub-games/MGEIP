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
        [SerializeField] private GameObject lockedImage;
        [SerializeField] private GameObject exclamationMark;
        [SerializeField] private GameObject checkMark;
        [SerializeField] private Vector3 hoveredOnScale;
        [SerializeField] private bool isScenarioLocked;

        private ScenarioManager scenarioManager;
        private bool hasCompleted;

        private Vector3 originalScale;
        private Vector3 hoverScale;

        public void Init(ScenarioManager _scenarioManager)
        {
            scenarioManager = _scenarioManager;
            originalScale = transform.localScale;
            hoverScale = originalScale + hoveredOnScale;
            if (isScenarioLocked)
                SetLocked();
            else
                SetUnchecked();
        }

        private void OnMouseOver()
        {
            if (EventSystem.current.IsPointerOverGameObject() || hasCompleted || isScenarioLocked)
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
            if (hasCompleted || isScenarioLocked)
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
            if (EventSystem.current.IsPointerOverGameObject() || hasCompleted || isScenarioLocked)
                return;

            SoundManagerService.Instance.OnStopVoiceOver?.Invoke();
            scenarioManager.OnClickScenarioButton(scenarioNo);
        }

        public void SetLocked()
        {
            lockedImage.SetActive(true);
            exclamationMark.SetActive(false);
            checkMark.SetActive(false);
        }

        public void SetUnchecked()
        {
            lockedImage.SetActive(false);
            exclamationMark.SetActive(true);
            checkMark.SetActive(false);
        }

        public void SetChecked()
        {
            hasCompleted = true;

            lockedImage.SetActive(false);
            exclamationMark.SetActive(false);
            checkMark.SetActive(true);
        }
    }
}
