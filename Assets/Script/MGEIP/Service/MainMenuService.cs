using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Script.MGEIP.Service
{
    public class MainMenuService : MonoBehaviour
    {
        [Header("Start")]
        [SerializeField] private GameObject beginPanel;
        [SerializeField] private Button startMenuNextButton;

        [Header("Disclaimer")]
        [SerializeField] private GameObject disclaimerPanel;
        [SerializeField] private Button disclaimerMenuNextButton;

        [Header("Story")]
        [SerializeField] private GameObject storyPanel;
        [SerializeField] private Transform storySubPanelParent;
        [SerializeField] private Button storyEndButton;
        [SerializeField] private Button storyLeftButton;
        [SerializeField] private Button storyRightButton;
        [SerializeField] private Image[] storyPaginationCircles;

        [Header("Tutorial")]
        [SerializeField] private GameObject tutorialPanel;
        [SerializeField] private Transform tutorialSubPanelParent;
        [SerializeField] private Button tutorialEndButton;
        [SerializeField] private Button tutorialLeftButton;
        [SerializeField] private Button tutorialRightButton;
        [SerializeField] private Image[] tutorialPaginationCircles;

        [Header("Pagination Sprites")]
        [SerializeField] private Sprite notViewedCircle;
        [SerializeField] private Sprite currentlyViewingCircle;
        [SerializeField] private Sprite alreadyViewedCircle;

        [Header("Loading Next Level")]
        [SerializeField] private GameObject beforeLoadingGO;
        [SerializeField] private GameObject afterLoadingGO;
        [SerializeField] private Image loadingBarFill;

        [Header("Reveal Animation")]
        [SerializeField] private List<GameObject> leftClouds;
        [SerializeField] private List<GameObject> rightClouds;
        [SerializeField] private GameObject mapGO;
        [SerializeField] private float maxCloudDelay = 1f;
        [SerializeField] private float cloudAnimationDuration = 1f;
        [SerializeField] private float mapScaleDelay = 1f;
        [SerializeField] private float mapAnimationDuration = 3f;
        [SerializeField] private float buttonScaleDuration = 1f;

        private int currentStoryIndex = 0;
        private int currentTutorialIndex = 0;

        private bool[] storySubPanelViewed;
        private bool[] tutorialSubPanelViewed;

        private void Awake()
        {
            startMenuNextButton.onClick.AddListener(OnNextButtonClicked);
            disclaimerMenuNextButton.onClick.AddListener(OnNextButtonClicked);

            storyEndButton.onClick.AddListener(OnStoryEndButtonClicked);
            tutorialEndButton.onClick.AddListener(OnTutorialEndButtonClicked);

            storyLeftButton.onClick.AddListener(OnStoryPrevButtonClicked);
            storyRightButton.onClick.AddListener(OnStoryNextButtonClicked);

            tutorialLeftButton.onClick.AddListener(OnTutorialPrevButtonClicked);
            tutorialRightButton.onClick.AddListener(OnTutorialNextButtonClicked);
        }

        private void OnDestroy()
        {
            startMenuNextButton.onClick.RemoveAllListeners();
            disclaimerMenuNextButton.onClick.RemoveAllListeners();

            storyEndButton.onClick.RemoveAllListeners();
            tutorialEndButton.onClick.RemoveAllListeners();

            storyLeftButton.onClick.RemoveAllListeners();
            storyRightButton.onClick.RemoveAllListeners();

            tutorialLeftButton.onClick.RemoveAllListeners();
            tutorialRightButton.onClick.RemoveAllListeners();
        }

        private void Start()
        {
            ShowPanel(beginPanel);
            RevealAnimation();

            storySubPanelViewed = new bool[storyPaginationCircles.Length];
            tutorialSubPanelViewed = new bool[tutorialPaginationCircles.Length];

            UpdateStoryPaginationCircles();
            UpdateTutorialPaginationCircles();
        }

        #region Button functions

        public void OnNextButtonClicked()
        {
            if (beginPanel.activeSelf)
            {
                ShowPanel(disclaimerPanel);
            }
            else if (disclaimerPanel.activeSelf)
            {
                ShowPanel(storyPanel);
                ShowStorySubPanel(currentStoryIndex);
            }
        }

        public void OnStoryNextButtonClicked()
        {
            currentStoryIndex++;
            ShowStorySubPanel(currentStoryIndex);
        }

        public void OnStoryPrevButtonClicked()
        {
            currentStoryIndex--;
            ShowStorySubPanel(currentStoryIndex);
        }

        public void OnTutorialNextButtonClicked()
        {
            currentTutorialIndex++;
            ShowTutorialSubPanel(currentTutorialIndex);
        }

        public void OnTutorialPrevButtonClicked()
        {
            currentTutorialIndex--;
            ShowTutorialSubPanel(currentTutorialIndex);
        }

        public void OnStoryEndButtonClicked()
        {
            ShowPanel(tutorialPanel);
            ShowTutorialSubPanel(currentTutorialIndex);
        }

        public void OnTutorialEndButtonClicked()
        {
            InitiateLoading();
        }

        #endregion

        private void ShowPanel(GameObject panel)
        {
            beginPanel.SetActive(false);
            disclaimerPanel.SetActive(false);
            storyPanel.SetActive(false);
            tutorialPanel.SetActive(false);

            panel.SetActive(true);
        }

        private void ShowStorySubPanel(int index)
        {
            for (int i = 0; i < storySubPanelParent.childCount; i++)
            {
                storySubPanelParent.GetChild(i).gameObject.SetActive(i == index);
            }

            OnStorySubPanelChanged();

            storyLeftButton.interactable = index > 0;
            storyRightButton.interactable = index < storySubPanelParent.childCount - 1;
        }

        private void ShowTutorialSubPanel(int index)
        {
            for (int i = 0; i < tutorialSubPanelParent.childCount; i++)
            {
                tutorialSubPanelParent.GetChild(i).gameObject.SetActive(i == index);
            }

            OnTutorialSubPanelChanged();

            tutorialLeftButton.interactable = index > 0;
            tutorialRightButton.interactable = index < tutorialSubPanelParent.childCount - 1;
        }

        public void OnStorySubPanelChanged()
        {
            storySubPanelViewed[currentStoryIndex] = true;

            UpdateStoryPaginationCircles();
        }

        public void OnTutorialSubPanelChanged()
        {
            tutorialSubPanelViewed[currentTutorialIndex] = true;

            UpdateTutorialPaginationCircles();
        }

        private void UpdateStoryPaginationCircles()
        {
            for (int i = 0; i < storyPaginationCircles.Length; i++)
            {
                if (i == currentStoryIndex)
                {
                    storyPaginationCircles[i].sprite = currentlyViewingCircle;
                }
                else if (storySubPanelViewed[i])
                {
                    storyPaginationCircles[i].sprite = alreadyViewedCircle;
                }
                else
                {
                    storyPaginationCircles[i].sprite = notViewedCircle;
                }
            }
        }

        private void UpdateTutorialPaginationCircles()
        {
            for (int i = 0; i < tutorialPaginationCircles.Length; i++)
            {
                if (i == currentTutorialIndex)
                {
                    tutorialPaginationCircles[i].sprite = currentlyViewingCircle;
                }
                else if (tutorialSubPanelViewed[i])
                {
                    tutorialPaginationCircles[i].sprite = alreadyViewedCircle;
                }
                else
                {
                    tutorialPaginationCircles[i].sprite = notViewedCircle;
                }
            }
        }

        private void InitiateLoading()
        {
            tutorialLeftButton.gameObject.SetActive(false);
            tutorialRightButton.gameObject.SetActive(false);

            beforeLoadingGO.SetActive(false);
            afterLoadingGO.SetActive(true);

            loadingBarFill.fillAmount = 0f;
            loadingBarFill.DOFillAmount(1f, 3f).SetEase(Ease.InOutQuad).OnComplete(() => SceneManager.LoadSceneAsync(1));

            // StartCoroutine(LoadGameScene());
        }

        private void RevealAnimation()
        {
            startMenuNextButton.gameObject.SetActive(false);

            foreach (GameObject cloud in leftClouds)
            {
                float randomDelay = Random.Range(0f, maxCloudDelay);
                cloud.transform.DOMoveX(cloud.transform.position.x - 10f, cloudAnimationDuration).SetEase(Ease.InOutQuad).SetDelay(randomDelay);
            }

            foreach (GameObject cloud in rightClouds)
            {
                float randomDelay = Random.Range(0f, maxCloudDelay);
                cloud.transform.DOMoveX(cloud.transform.position.x + 10f, cloudAnimationDuration).SetEase(Ease.InOutQuad).SetDelay(randomDelay);
            }

            mapGO.transform.localScale = Vector3.zero;
            mapGO.transform.DOScale(Vector3.one, mapAnimationDuration).SetEase(Ease.OutSine).SetDelay(mapScaleDelay).OnComplete(AnimateStartButton);
        }

        private void AnimateStartButton()
        {
            startMenuNextButton.gameObject.SetActive(true);
            startMenuNextButton.transform.localScale = Vector3.zero; // Initially scale button to 0
            startMenuNextButton.transform.DOScale(Vector3.one, buttonScaleDuration)
                .SetEase(Ease.OutBack);
        }

        private IEnumerator LoadGameScene()
        {
            var scene = SceneManager.LoadSceneAsync(1);
            scene.allowSceneActivation = false;

            loadingBarFill.fillAmount = 0f;

            while (scene.progress < 0.9f)
            {
                Debug.Log(scene.progress);
                loadingBarFill.fillAmount = Mathf.Clamp01(scene.progress / 0.9f);
                yield return null;
            }

            scene.allowSceneActivation = true;
        }
    }
}