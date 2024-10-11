using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MGEIP;
using MGEIP.GameData;
using MGIEP;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Script.MGEIP.Service
{
    public class MainMenuService : MonoBehaviour
    {
        public static MainMenuService Instance;

        [SerializeField] private GameObject mainCanvas;
        [SerializeField] private MainMenuDataContainer mainMenuDataContainer;

        [Header("Start")]
        [SerializeField] private CanvasGroup beginPanel;
        [SerializeField] private Button startMenuNextButton;
        [SerializeField] private GameObject startInstructionBox;

        [Header("Disclaimer")]
        [SerializeField] private CanvasGroup disclaimerPanel;
        [SerializeField] private GameObject backgroundGO;
        [SerializeField] private Button disclaimerMenuNextButton;

        [Header("StoryTutorial")]
        [SerializeField] private CanvasGroup storyTutorialPanel;
        [SerializeField] private CanvasGroup basePanelElements;
        [SerializeField] private Transform subPanelParent;
        [SerializeField] private Button leftButton;
        [SerializeField] private Button rightButton;
        [SerializeField] private Button endButton;
        [SerializeField] private Image paginationCirclePrefab;
        [SerializeField] private CanvasGroup paginationParent;
        [SerializeField] private List<TMP_Text> mainMenuTexts = new();

        [Header("Pagination Sprites")]
        [SerializeField] private Sprite notViewedCircle;
        [SerializeField] private Sprite currentlyViewingCircle;
        [SerializeField] private Sprite alreadyViewedCircle;

        [Header("Loading Next Level")]
        [SerializeField] private CanvasGroup beforeLoadingGO;
        [SerializeField] private CanvasGroup afterLoadingGO;
        [SerializeField] private Image loadingBarFill;

        [Header("Tabs")]
        [SerializeField] private Button storyTabButton;
        [SerializeField] private Button tutorialTabButton;
        [SerializeField] private TMP_Text storyTabButtonLabel;
        [SerializeField] private TMP_Text tutorialTabButtonLabel;
        [SerializeField] private Sprite selectedButtonSprite;
        [SerializeField] private Sprite deselectedButtonSprite;
        [SerializeField] private Vector3 tabButtonSelectedScale;
        [SerializeField] private Color selectedButtonTextColor;
        [SerializeField] private Color deselectedButtonTextColor;

        [Header("Animation")]
        [SerializeField] private MainMenuAnimation mainMenuAnimation;
        [SerializeField] private float buttonScaleDuration = 1f;

        [Header("Slide counts")]
        [SerializeField] private int storySlideCount = 5;
        [SerializeField] private int tutorialSlideCount = 7;

        private int currentSubPanelIndex = 0;
        private bool isStoryPanelActive;
        private bool[] subPanelViewed;

        private Image storyTabImage;
        private Image tutorialTabImage;
        private CanvasGroup currentActivePanel;
        private CanvasGroup currentActiveSubPanel;

        private CanvasGroup[] subPanels;
        private List<Image> paginationCircles = new();

        public MainMenuDataContainer MainMenuDataContainer => mainMenuDataContainer;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            startMenuNextButton.onClick.AddListener(NextButtonClicked);
            disclaimerMenuNextButton.onClick.AddListener(NextButtonClicked);

            leftButton.onClick.AddListener(LeftButtonClicked);
            rightButton.onClick.AddListener(RightButtonClicked);

            endButton.onClick.AddListener(EndButtonClicked);

            storyTabButton.onClick.AddListener(StoryTabButtonClicked);
            tutorialTabButton.onClick.AddListener(TutorialTabButtonClicked);

            storyTabImage = storyTabButton.GetComponent<Image>();
            tutorialTabImage = tutorialTabButton.GetComponent<Image>();
        }

        private void OnDestroy()
        {
            startMenuNextButton.onClick.RemoveAllListeners();
            disclaimerMenuNextButton.onClick.RemoveAllListeners();

            leftButton.onClick.RemoveAllListeners();
            rightButton.onClick.RemoveAllListeners();

            endButton.onClick.RemoveAllListeners();

            storyTabButton.onClick.RemoveAllListeners();
            tutorialTabButton.onClick.RemoveAllListeners();
        }

        private void Start()
        {
            SoundManagerService.Instance.LoadAudio("MainMenuAudioClips");

            currentActivePanel = beginPanel;

            startMenuNextButton.gameObject.SetActive(false);
            startInstructionBox.gameObject.SetActive(false);
            mainMenuAnimation.RevealAnimation();

            subPanelViewed = new bool[subPanelParent.childCount];
            for (int i = 0; i < subPanelParent.childCount; i++)
            {
                Image paginationCircle = Instantiate<Image>(paginationCirclePrefab);
                paginationCircle.transform.SetParent(paginationParent.transform);
                paginationCircles.Add(paginationCircle);
            }

            subPanels = subPanelParent.GetComponentsInChildren<CanvasGroup>(true);

            tutorialTabButton.GetComponent<ButtonAnimation>().DisableButton();

            SwitchToStory();

            SetupMainMenuTexts();
        }

        #region Button functions

        public void NextButtonClicked()
        {
            if (beginPanel.gameObject.activeSelf)
            {
                ShowPanel(disclaimerPanel);
            }
            else if (disclaimerPanel.gameObject.activeSelf)
            {
                ShowPanel(storyTutorialPanel);
                ShowSubPanel(currentSubPanelIndex);
            }
        }

        public void RightButtonClicked()
        {
            currentSubPanelIndex++;
            ShowSubPanel(currentSubPanelIndex);
        }

        public void LeftButtonClicked()
        {
            currentSubPanelIndex--;
            ShowSubPanel(currentSubPanelIndex);
        }

        public void EndButtonClicked()
        {
            basePanelElements.DOFade(0, 0.25f);

            beforeLoadingGO.DOFade(0, 0.25f).OnComplete(() => beforeLoadingGO.gameObject.SetActive(false));

            afterLoadingGO.gameObject.SetActive(true);
            afterLoadingGO.alpha = 0;
            afterLoadingGO.DOFade(1, 0.25f);

            InitiateLoading();
        }

        #endregion

        #region Panel

        private void ShowPanel(CanvasGroup newPanel)
        {
            currentActivePanel.DOFade(0, 0.25f).OnComplete(() =>
            {
                currentActivePanel.gameObject.SetActive(false);

                if (newPanel == disclaimerPanel)
                {
                    newPanel.gameObject.SetActive(true);
                    newPanel.alpha = 0;
                    newPanel.transform.localScale = new Vector3(0.5f, 0.5f, 1f);

                    Sequence sequence = DOTween.Sequence();
                    sequence.Append(newPanel.transform.DOScale(Vector3.one, 0.5f))
                            .Join(newPanel.DOFade(1, 0.5f));
                    sequence.AppendCallback(() =>
                    {
                        backgroundGO.transform.SetParent(mainCanvas.transform);
                        backgroundGO.transform.SetAsFirstSibling();
                    });
                }
                else
                {
                    newPanel.gameObject.SetActive(true);
                    newPanel.alpha = 0;
                    newPanel.DOFade(1, 0.5f);
                }

                currentActivePanel = newPanel;
            });
        }

        private void ShowSubPanel(int index)
        {
            if (index < 0 || index >= subPanelParent.childCount)
            {
                Debug.LogError("Sub panel index error : " + index);
                return;
            }

            AnimateSubPanelActivation(subPanels[index]);

            subPanelViewed[currentSubPanelIndex] = true;
            UpdatePaginationCircles();

            if (currentSubPanelIndex > storySlideCount - 1 && isStoryPanelActive)
                SwitchToTutorial();
            else if (currentSubPanelIndex <= storySlideCount - 1 && !isStoryPanelActive)
                SwitchToStory();

            leftButton.interactable = index > 0;
            rightButton.interactable = index < subPanelParent.childCount - 1;
        }

        private void AnimateSubPanelActivation(CanvasGroup newPanel)
        {
            if (currentActiveSubPanel != null)
            {
                currentActiveSubPanel.DOFade(0, 0.25f).OnComplete(() =>
                {
                    currentActiveSubPanel.gameObject.SetActive(false);

                    newPanel.gameObject.SetActive(true);
                    newPanel.alpha = 0;
                    newPanel.DOFade(1, 0.25f);

                    currentActiveSubPanel = newPanel;
                });
            }
            else
            {
                newPanel.gameObject.SetActive(true);
                newPanel.alpha = 0;
                newPanel.DOFade(1, 0.25f);
                currentActiveSubPanel = newPanel;
            }
        }

        private void UpdatePaginationCircles()
        {
            for (int i = 0; i < paginationCircles.Count; i++)
            {
                if (i == currentSubPanelIndex)
                {
                    paginationCircles[i].sprite = currentlyViewingCircle;
                }
                else if (subPanelViewed[i])
                {
                    paginationCircles[i].sprite = alreadyViewedCircle;
                }
                else
                {
                    paginationCircles[i].sprite = notViewedCircle;
                }
            }
        }

        private void SwitchToStory()
        {
            // Switch button to story
            ActivateButton(storyTabButton);

            Sequence paginationSequence = DOTween.Sequence();

            paginationSequence.Append(paginationParent.DOFade(0, 0.25f));
            paginationSequence.AppendCallback(() =>
            {
                for (int i = 0; i < paginationCircles.Count; i++)
                {
                    paginationCircles[i].gameObject.SetActive(i <= storySlideCount - 1);
                }
            });
            paginationSequence.Append(paginationParent.DOFade(1, 0.25f));

            isStoryPanelActive = true;
        }

        private void SwitchToTutorial()
        {
            tutorialTabButton.GetComponent<ButtonAnimation>().EnableButton();

            // Switch button to tutorial
            ActivateButton(tutorialTabButton);

            Sequence paginationSequence = DOTween.Sequence();

            paginationSequence.Append(paginationParent.DOFade(0, 0.25f));
            paginationSequence.AppendCallback(() =>
            {
                for (int i = 0; i < paginationCircles.Count; i++)
                {
                    paginationCircles[i].gameObject.SetActive(i > storySlideCount - 1);
                }
            });
            paginationSequence.Append(paginationParent.DOFade(1, 0.25f));

            isStoryPanelActive = false;
        }

        private void ActivateButton(Button _button)
        {
            if (_button == storyTabButton)
            {
                AnimateButtonTransition(storyTabImage, selectedButtonSprite, storyTabButtonLabel, selectedButtonTextColor, tabButtonSelectedScale);

                if (!tutorialTabButton.interactable)
                    return;

                AnimateButtonTransition(tutorialTabImage, deselectedButtonSprite, tutorialTabButtonLabel, deselectedButtonTextColor, Vector3.one);
            }
            else
            {
                AnimateButtonTransition(tutorialTabImage, selectedButtonSprite, tutorialTabButtonLabel, selectedButtonTextColor, tabButtonSelectedScale);

                if (!tutorialTabButton.interactable)
                    return;

                AnimateButtonTransition(storyTabImage, deselectedButtonSprite, storyTabButtonLabel, deselectedButtonTextColor, Vector3.one);
            }
        }

        private void AnimateButtonTransition(Image buttonImage, Sprite newSprite, TMP_Text buttonLabel, Color newLabelColor, Vector3 newScale)
        {
            buttonImage.sprite = newSprite;

            buttonLabel.DOColor(newLabelColor, 0.5f);

            buttonImage.transform.DOScale(newScale, 0.5f);
        }

        private void StoryTabButtonClicked()
        {
            if (isStoryPanelActive)
                return;

            currentSubPanelIndex = 0;
            ShowSubPanel(currentSubPanelIndex);
        }

        private void TutorialTabButtonClicked()
        {
            if (!isStoryPanelActive)
                return;

            currentSubPanelIndex = storySlideCount;
            ShowSubPanel(currentSubPanelIndex);
        }

        private void InitiateLoading()
        {
            loadingBarFill.fillAmount = 0f;
            loadingBarFill.DOFillAmount(1f, 3f).SetEase(Ease.InOutQuad).OnComplete(() => SceneManager.LoadSceneAsync(1));

            SoundManagerService.Instance.ReleaseAudio();

            // StartCoroutine(LoadGameScene());
        }

        private void SetupMainMenuTexts()
        {
            for (int i = 0; i < mainMenuTexts.Count; i++)
            {
                mainMenuTexts[i].text = mainMenuDataContainer.MainMenuContent.mainMenuDataList[i].TextContent;
            }
        }

        #endregion

        public void AnimateStartButton()
        {
            startMenuNextButton.GetComponent<ButtonAnimation>().enabled = false;
            startMenuNextButton.gameObject.SetActive(true);
            startInstructionBox.gameObject.SetActive(true);
            startMenuNextButton.transform.localScale = Vector3.zero;
            startInstructionBox.transform.localScale = Vector3.zero;
            startMenuNextButton.transform.DOScale(Vector3.one, buttonScaleDuration)
                .SetEase(Ease.OutBack).OnComplete(() => startMenuNextButton.GetComponent<ButtonAnimation>().enabled = true);
            startInstructionBox.transform.DOScale(Vector3.one, buttonScaleDuration).SetEase(Ease.OutBack).OnComplete(() => mainMenuAnimation.StartBoatAnim());
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