using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DG.Tweening;
using MGEIP;
using MGEIP.GameData;
using MGIEP;
using MGIEP.Data;
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

        [Header("Form Panel")]
        [SerializeField] private CanvasGroup formPanel;
        [SerializeField] private TMP_InputField playerNameInput;
        [SerializeField] private TMP_InputField playerEmailInput;
        [SerializeField] private TMP_InputField playerDOBDayInput;
        [SerializeField] private TMP_InputField playerDOBMonthInput;
        [SerializeField] private TMP_InputField playerDOBYearInput;
        [SerializeField] private GenderDropdownHandler playerGenderInput;
        [SerializeField] private GameObject playerNameErrorLabel;
        [SerializeField] private GameObject playerEmailErrorLabel;
        [SerializeField] private GameObject playerDOBErrorLabel;
        [SerializeField] private GameObject playerGenderErrorLabel;
        [SerializeField] private Button formSubmitButton;

        [Header("Start")]
        [SerializeField] private CanvasGroup beginPanel;
        [SerializeField] private CanvasGroup gameNamePanel;
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

        [Header("Login panel")]
        [SerializeField] private CanvasGroup loginPanel;
        [SerializeField] private TMP_InputField playerLoginNameInput;
        [SerializeField] private Button loginButton;
        [SerializeField] private TMP_Text logLabel;
        [SerializeField] private CanvasGroup errorPanel;
        [SerializeField] private Button errorOkayButton;

        [Header("Welcome Back Panel")]
        [SerializeField] private CanvasGroup welcomeBackPanel;
        [SerializeField] private TMP_Text welcomeBackLabel;
        [SerializeField] private Button welcomeBackStartButton;
        [SerializeField] private Button skipTutorialButton;
        [SerializeField] private CanvasGroup welcomeBackSubPanel;
        [SerializeField] private CanvasGroup confirmSubPanel;
        [SerializeField] private Button confirmYesButton;
        [SerializeField] private Button confirmNoButton;
        [SerializeField] private CanvasGroup confirmLoadingSubPanel;
        [SerializeField] private Image confirmLoadingBarFill;

        private int currentSubPanelIndex = 0;
        private bool isStoryPanelActive;
        private bool[] subPanelViewed;

        private LoginType loginType;

        private Image storyTabImage;
        private Image tutorialTabImage;
        private CanvasGroup currentActivePanel;
        private CanvasGroup currentActiveSubPanel;

        private CanvasGroup[] subPanels;
        private List<Image> paginationCircles = new();

        private Sequence loadingBarSequence;

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

            loginButton.onClick.AddListener(LoginButtonClicked);

            storyTabImage = storyTabButton.GetComponent<Image>();
            tutorialTabImage = tutorialTabButton.GetComponent<Image>();

            welcomeBackStartButton.onClick.AddListener(WelcomeBackStartClicked);
            skipTutorialButton.onClick.AddListener(SkipTutorialClicked);
            confirmYesButton.onClick.AddListener(SkipTutorialYes);
            confirmNoButton.onClick.AddListener(SkipTutorialNo);

            errorOkayButton.onClick.AddListener(CloseErrorPanel);

            formSubmitButton.onClick.AddListener(FormUISubmitClicked);
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

            loginButton.onClick.RemoveAllListeners();

            welcomeBackStartButton.onClick.RemoveListener(WelcomeBackStartClicked);
            skipTutorialButton.onClick.RemoveListener(SkipTutorialClicked);
            confirmYesButton.onClick.RemoveListener(SkipTutorialYes);
            confirmNoButton.onClick.RemoveListener(SkipTutorialNo);

            errorOkayButton.onClick.RemoveListener(CloseErrorPanel);

            formSubmitButton.onClick.RemoveListener(FormUISubmitClicked);

            if (loadingBarSequence == null)
                return;

            if (loadingBarSequence.IsActive() || loadingBarSequence.IsPlaying())
                loadingBarSequence.Kill();
        }

        private void Start()
        {
            DataHandler.Instance.OnPlayerLogin += LoginSuccess;
            Debug.Log("Sub to OnPlayerLogin!");

            DataHandler.Instance.LoginPlayer();

            currentActivePanel = beginPanel;

            gameNamePanel.gameObject.SetActive(false);
            startMenuNextButton.gameObject.SetActive(false);
            startInstructionBox.gameObject.SetActive(false);

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

            SoundManagerService.Instance.OnPlayMusic?.Invoke("BGmusic");
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
            // loadingBarFill.fillAmount = 0f;
            // loadingBarFill.DOFillAmount(1f, 3f).SetEase(Ease.InOutQuad).OnComplete(() => SceneManager.LoadSceneAsync(1));

            // SoundManagerService.Instance.ReleaseAudio();

            StartCoroutine(LoadGameScene());
        }

        private void SetupMainMenuTexts()
        {
            for (int i = 0; i < mainMenuTexts.Count; i++)
            {
                mainMenuTexts[i].text = mainMenuDataContainer.MainMenuContent.mainMenuDataList[i].TextContent;
            }
        }

        #endregion

        public void AnimateGameName()
        {
            gameNamePanel.gameObject.SetActive(true);
            gameNamePanel.alpha = 0;
            gameNamePanel.DOFade(1, buttonScaleDuration).SetDelay(1f).OnComplete(CheckForLoginType);
        }

        private void CheckForLoginType()
        {
            switch (loginType)
            {
                case LoginType.newPlayer:
                    AnimateFormUIPanel();
                    break;
                case LoginType.newAttempt:
                    AnimateStartButton();
                    break;
                case LoginType.continueAttempt:
                    welcomeBackLabel.text = "Continue where you left off in the previous session";
                    AnimateWelcomeBackPanel();
                    break;
                case LoginType.repeatAttempt:
                    welcomeBackLabel.text = "Start a new assessment session";
                    AnimateWelcomeBackPanel();
                    break;
            }
        }

        public void AnimateStartButton()
        {
            startMenuNextButton.GetComponent<ButtonAnimation>().enabled = false;
            startMenuNextButton.gameObject.SetActive(true);
            startInstructionBox.gameObject.SetActive(true);
            startMenuNextButton.transform.localScale = Vector3.zero;
            startInstructionBox.transform.localScale = Vector3.zero;
            startMenuNextButton.transform.DOScale(Vector3.one, buttonScaleDuration)
                .SetEase(Ease.OutBack).OnComplete(() => startMenuNextButton.GetComponent<ButtonAnimation>().enabled = true);
            startInstructionBox.transform.DOScale(Vector3.one, buttonScaleDuration).SetEase(Ease.OutBack);
        }

        private IEnumerator LoadGameScene()
        {
            var scene = SceneManager.LoadSceneAsync(1);
            scene.allowSceneActivation = false;

            loadingBarFill.fillAmount = 0f;
            confirmLoadingBarFill.fillAmount = 0f;

            loadingBarSequence = DOTween.Sequence();

            loadingBarSequence.Append(loadingBarFill.DOFillAmount(1, 8f)).Join(confirmLoadingBarFill.DOFillAmount(1, 8f));

            while (scene.progress < 0.9f)
            {
                Debug.Log(scene.progress);
                // loadingBarFill.fillAmount = Mathf.Clamp01(scene.progress / 0.9f);
                // confirmLoadingBarFill.fillAmount = Mathf.Clamp01(scene.progress / 0.9f);
                yield return null;
            }

            scene.allowSceneActivation = true;
        }

        #region Login panel

        private void LoginButtonClicked()
        {
            if (playerLoginNameInput.text == "")
            {
                logLabel.gameObject.SetActive(true);
                logLabel.text = "Please enter a player name!";
                return;
            }

            loginButton.gameObject.SetActive(false);
            playerLoginNameInput.interactable = false;

            DataHandler.Instance.LoginPlayer(playerLoginNameInput.text);
            logLabel.gameObject.SetActive(false);
        }

        private void LoginSuccess(LoginType _loginType)
        {
            Debug.Log("Login success : " + _loginType);

            if (_loginType == LoginType.error)
            {
                ErrorConnectingToServer();
                return;
            }
            else
            {
                if (errorPanel.gameObject.activeSelf)
                    CloseErrorPanel();
            }

            loginType = _loginType;

            loginPanel.blocksRaycasts = false;
            loginPanel.DOFade(0, 0.5f).OnComplete(() => loginPanel.gameObject.SetActive(false));

            mainMenuAnimation.RevealAnimation();

            DataHandler.Instance.OnPlayerLogin -= LoginSuccess;
        }

        private void ErrorConnectingToServer()
        {
            errorPanel.gameObject.SetActive(true);
            errorPanel.alpha = 0;
            errorPanel.DOFade(1, 0.5f);
        }

        private void CloseErrorPanel()
        {
            errorPanel.DOFade(0, 0.5f).OnComplete(() =>
            {
                errorPanel.gameObject.SetActive(false);
            });

            loginButton.gameObject.SetActive(true);
            playerLoginNameInput.interactable = true;
        }

        #endregion

        #region Form UI

        private void AnimateFormUIPanel()
        {
            gameNamePanel.DOFade(0, buttonScaleDuration).SetDelay(2f).OnComplete(() =>
            {
                gameNamePanel.gameObject.SetActive(false);

                formPanel.gameObject.SetActive(true);
                formPanel.alpha = 0;

                formPanel.DOFade(1, 0.5f);
            });
        }

        private void CloseFormUIPanel()
        {
            formPanel.DOFade(0, 0.5f).OnComplete(() =>
            {
                formPanel.gameObject.SetActive(false);
                AnimateStartButton();
            });
        }

        private void FormUISubmitClicked()
        {
            if (!IsValidInput())
                return;

            int.TryParse(playerDOBDayInput.text, out int day);
            int.TryParse(playerDOBMonthInput.text, out int month);
            int.TryParse(playerDOBYearInput.text, out int year);

            DateTime playerDOB = new DateTime(year, month, day);

            DataHandler.Instance.UploadPlayerData(playerNameInput.text, playerEmailInput.text, playerDOB, playerGenderInput.CurrentSelectionText);

            CloseFormUIPanel();
        }

        private bool IsValidInput()
        {
            bool isValidated = true;

            playerNameErrorLabel.SetActive(false);
            playerEmailErrorLabel.SetActive(false);
            playerDOBErrorLabel.SetActive(false);
            playerGenderErrorLabel.SetActive(false);

            if (string.IsNullOrEmpty(playerNameInput.text))
            {
                playerNameErrorLabel.SetActive(true);
                playerNameInput.text = "";
                isValidated = false;
            }

            if (string.IsNullOrEmpty(playerEmailInput.text) || !IsValidEmail(playerEmailInput.text))
            {
                playerEmailErrorLabel.SetActive(true);
                playerEmailInput.text = "";
                isValidated = false;
            }

            if (!IsValidDate(playerDOBDayInput.text, playerDOBMonthInput.text, playerDOBYearInput.text))
            {
                playerDOBErrorLabel.SetActive(true);
                playerDOBDayInput.text = "";
                playerDOBMonthInput.text = "";
                playerDOBYearInput.text = "";
                isValidated = false;
            }

            if (playerGenderInput.CurrentSelection == -1)
            {
                playerGenderErrorLabel.SetActive(true);
                isValidated = false;
            }

            return isValidated;
        }

        private bool IsValidDate(string _day, string _month, string _year)
        {
            if (string.IsNullOrEmpty(_day) || string.IsNullOrEmpty(_month) || string.IsNullOrEmpty(_year))
                return false;

            if (int.TryParse(_day, out int day) &&
            int.TryParse(_month, out int month) &&
            int.TryParse(_year, out int year))
            {
                try
                {
                    DateTime playerDOB = new DateTime(year, month, day);

                    int minYear = 1900;
                    int maxYear = DateTime.Today.Year;

                    if (year < minYear || year > maxYear)
                        return false;
                    else if (playerDOB > DateTime.Today)
                        return false;
                    else
                        return true;
                }
                catch (ArgumentOutOfRangeException)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            Regex regex = new Regex(emailPattern);

            return regex.IsMatch(email);
        }

        #endregion

        #region Welcome Back

        private void AnimateWelcomeBackPanel()
        {
            gameNamePanel.DOFade(0, buttonScaleDuration).SetDelay(2f).OnComplete(() =>
            {
                gameNamePanel.gameObject.SetActive(false);

                welcomeBackPanel.gameObject.SetActive(true);
                welcomeBackPanel.alpha = 0;

                welcomeBackPanel.DOFade(1, 0.5f);
            });
        }

        private void CloseWelcomeBackPanel()
        {
            welcomeBackPanel.DOFade(0, 0.5f).OnComplete(() => welcomeBackPanel.gameObject.SetActive(false));
        }

        private void WelcomeBackStartClicked()
        {
            CloseWelcomeBackPanel();

            NextButtonClicked();
        }

        private void SkipTutorialClicked()
        {
            welcomeBackSubPanel.blocksRaycasts = false;
            welcomeBackSubPanel.DOFade(0, 0.5f).OnComplete(() => welcomeBackSubPanel.gameObject.SetActive(false));

            confirmSubPanel.gameObject.SetActive(true);
            confirmSubPanel.alpha = 0;
            confirmSubPanel.blocksRaycasts = false;
            confirmSubPanel.DOFade(1, 0.5f).OnComplete(() => confirmSubPanel.blocksRaycasts = true);
        }

        private void SkipTutorialYes()
        {
            confirmSubPanel.blocksRaycasts = false;
            confirmSubPanel.DOFade(0, 0.5f).OnComplete(() => confirmSubPanel.gameObject.SetActive(false));

            confirmLoadingSubPanel.gameObject.SetActive(true);
            confirmLoadingSubPanel.alpha = 0;
            confirmLoadingSubPanel.blocksRaycasts = false;

            confirmLoadingSubPanel.DOFade(1, 0.5f).OnComplete(() =>
            {
                StartCoroutine(LoadGameScene());
            });
        }

        private void SkipTutorialNo()
        {
            confirmSubPanel.blocksRaycasts = false;
            confirmSubPanel.DOFade(0, 0.5f).OnComplete(() => confirmSubPanel.gameObject.SetActive(false));

            welcomeBackSubPanel.gameObject.SetActive(true);
            welcomeBackSubPanel.alpha = 0;
            welcomeBackSubPanel.blocksRaycasts = false;
            welcomeBackSubPanel.DOFade(1, 0.5f).OnComplete(() => welcomeBackSubPanel.blocksRaycasts = true);
        }

        #endregion
    }
}