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
            // Hide all panels
            beginPanel.SetActive(false);
            disclaimerPanel.SetActive(false);
            storyPanel.SetActive(false);
            tutorialPanel.SetActive(false);

            // Show the specified panel
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
            // Mark the current sub-panel as viewed
            storySubPanelViewed[currentStoryIndex] = true;

            // Update the pagination circles' sprites
            UpdateStoryPaginationCircles();
        }

        public void OnTutorialSubPanelChanged()
        {
            // Mark the current sub-panel as viewed
            tutorialSubPanelViewed[currentTutorialIndex] = true;

            // Update the pagination circles' sprites
            UpdateTutorialPaginationCircles();
        }

        private void UpdateStoryPaginationCircles()
        {
            for (int i = 0; i < storyPaginationCircles.Length; i++)
            {
                if (i == currentStoryIndex)
                {
                    // Set the currently viewing sprite
                    storyPaginationCircles[i].sprite = currentlyViewingCircle;
                }
                else if (storySubPanelViewed[i])
                {
                    // Set the viewed sprite
                    storyPaginationCircles[i].sprite = alreadyViewedCircle;
                }
                else
                {
                    // Set the not viewed sprite
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
                    // Set the currently viewing sprite
                    tutorialPaginationCircles[i].sprite = currentlyViewingCircle;
                }
                else if (tutorialSubPanelViewed[i])
                {
                    // Set the viewed sprite
                    tutorialPaginationCircles[i].sprite = alreadyViewedCircle;
                }
                else
                {
                    // Set the not viewed sprite
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
            loadingBarFill.DOFillAmount(1f, 3f).SetEase(Ease.InOutQuad).OnComplete(() => StartGame());
        }

        private void StartGame()
        {
            SceneManager.LoadSceneAsync(1);
        }
    }
}