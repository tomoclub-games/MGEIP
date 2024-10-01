using DG.Tweening;
using MGIEP;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MGEIP.Service
{
    public class GameUIService : MonoBehaviour
    {
        [Header("Scenario Components")]
        public GameObject sceneHolder;
        public GameObject MapUI;
        public Sprite tickSprite;
        [SerializeField] private Button gameEndButton;

        [Header("Start Scene Components")]
        [SerializeField] private GameObject startSceneUIGameobject;
        [SerializeField] private TextMeshProUGUI scenarioNameTextInScene;
        [SerializeField] private GameObject startSceneNarrationBoxGameobject;
        [SerializeField] private TextMeshProUGUI startSceneNarrationText;
        [SerializeField] private Button startScenePlayButton;

        [Header("Story Scene Components")]
        [SerializeField] private GameObject storySceneUIGameobject;
        [SerializeField] private GameObject storySceneNarrationBoxGameobject;
        [SerializeField] private TextMeshProUGUI storySceneNarrationText;
        [SerializeField] private Button storySceneNextButton;
        [SerializeField] private Button storyScenePrevButton;

        [Header("Question Scene Components")]
        [SerializeField] private GameObject questionSceneNarrationBoxGameobject;
        [SerializeField] private TextMeshProUGUI questionSceneNarrationText;
        [SerializeField] private GameObject questionSceneUIGameobject;
        [SerializeField] private TextMeshProUGUI questionText;
        [SerializeField] private Button questionSceneConfirmButton;
        [SerializeField] private Button questionScenePrevButton;
        [SerializeField] private Image[] sliderLabels;

        [Header("Options Components")]
        [SerializeField] private GameObject optionPanelGameobject;
        [SerializeField] private Button[] optionButtons;
        [SerializeField] private TextMeshProUGUI[] optionButtonText;

        [Header("Slider Components")]
        [SerializeField] private GameObject sliderPanelGameobject;
        [SerializeField] private Slider answerSlider;
        [SerializeField] private GameObject ceLabelGameobject;
        [SerializeField] private GameObject aeLabelGameobject;
        [SerializeField] private TMP_Text aeLowerLabel;
        [SerializeField] private TMP_Text aeHigherLabel;

        [Header("Photo Capture Scene Components")]
        [SerializeField] private GameObject photoCaptureSceneUIGameobject;
        [SerializeField] private GameObject cameraUIGameObject;
        [SerializeField] private GameObject postCameraUIGameObject;
        [SerializeField] private GameObject collageUIGameObject;
        [SerializeField] private Button photoCaptureScenePrevButton;
        [SerializeField] private Button photoCaptureShutterButton;
        [SerializeField] private Button postCameraNextButton;
        [SerializeField] private Button photoCaptureSceneNextButton;
        [SerializeField] private GameObject photoCaptureSceneNarrationBoxGameobject;
        [SerializeField] private TextMeshProUGUI photoCaptureSceneNarrationText;

        [Header("End Scene Components")]
        [SerializeField] private GameObject endSceneUIGameobject;
        [SerializeField] private GameObject endSceneNarrationBoxGameobject;
        [SerializeField] private TextMeshProUGUI endSceneNarrationText;
        [SerializeField] private Button endSceneEndButton;
        [SerializeField] private Button endScenePrevButton;

        [Header("Sprites")]
        [SerializeField] private Sprite selectedSliderLabelSprite;
        [SerializeField] private Sprite deselectedSliderLabelSprite;

        [Header("Scenario Info Panel")]
        [SerializeField] private GameObject scenarioInfoPanel;
        [SerializeField] private TMP_Text scenarioInfoName;
        [SerializeField] private TMP_Text scenarioInfoDesc;
        [SerializeField] private GameObject[] scenarioInfoImages;
        [SerializeField] private Button scenarioInfoCloseButton;
        [SerializeField] private Button scenarioInfoStartButton;
        [SerializeField] private GameObject scenarioInfoBeforeGO;
        [SerializeField] private GameObject scenarioInfoAfterGO;
        [SerializeField] private Image scenarioInfoLoadingBarFill;

        [Header("VO Buttons")]
        [SerializeField] private Button scenarioNameVOButton;
        [SerializeField] private Button scenarioDescVOButton;
        [SerializeField] private Button startSceneTitleVOButton;
        [SerializeField] private Button startSceneNarrationVOButton;
        [SerializeField] private Button storySceneNarrationVOButton;
        [SerializeField] private Button questionSceneNarrationVOButton;
        [SerializeField] private Button endSceneNarrationVOButton;
        [SerializeField] private Button photoCaptureSceneNarrationVOButton;
        [SerializeField] private Button questionVOButton;
        [SerializeField] private Button[] optionVOButtons;

        private int currentScenarioInfoIndex = 0;

        public UnityAction<int> OnScenarioStart;
        public UnityAction OnPhotoCapture;

        #region Button events

        public UnityAction<int> OnMCQOptionSelect;
        public UnityAction<int> OnSliderAnswerSelect;
        public UnityAction OnConfirmButtonClick;
        public UnityAction OnPrevButtonClick;

        public UnityAction OnStartSceneTitleVOClick;
        public UnityAction OnNarrationVOClick;
        public UnityAction OnQuestionVOClick;
        public UnityAction<int> OnOptionVOClick;

        #endregion

        private void Awake()
        {
            for (int i = 0; i < 4; i++)
            {
                int buttonIndex = i;
                optionButtons[i].onClick.AddListener(() => OnMCQOptionSelect?.Invoke(buttonIndex));
            }

            questionSceneConfirmButton.onClick.AddListener(() => OnConfirmButtonClick?.Invoke());
            questionScenePrevButton.onClick.AddListener(() => OnPrevButtonClick?.Invoke());

            answerSlider.onValueChanged.AddListener((value) =>
            {
                int selectedValue = (int)value;
                UpdateSliderLabelImage(selectedValue);
                OnSliderAnswerSelect?.Invoke((int)value);
            });

            scenarioInfoCloseButton.onClick.AddListener(ScenarioInfoCloseButtonClick);
            scenarioInfoStartButton.onClick.AddListener(ScenarioPlayButtonClick);

            photoCaptureShutterButton.onClick.AddListener(ShutterButtonClicked);
            postCameraNextButton.onClick.AddListener(PostCameraNextButtonClicked);

            // VO Buttons

            scenarioNameVOButton.onClick.AddListener(() => PlayScenarioTitleVO());
            scenarioDescVOButton.onClick.AddListener(() => PlayScenarioDescVO());

            startSceneTitleVOButton.onClick.AddListener(() => OnStartSceneTitleVOClick?.Invoke());

            startSceneNarrationVOButton.onClick.AddListener(() => OnNarrationVOClick?.Invoke());
            storySceneNarrationVOButton.onClick.AddListener(() => OnNarrationVOClick?.Invoke());
            questionSceneNarrationVOButton.onClick.AddListener(() => OnNarrationVOClick?.Invoke());
            endSceneNarrationVOButton.onClick.AddListener(() => OnNarrationVOClick?.Invoke());
            photoCaptureSceneNarrationVOButton.onClick.AddListener(() => OnNarrationVOClick?.Invoke());

            questionVOButton.onClick.AddListener(() => OnQuestionVOClick?.Invoke());

            for (int i = 0; i < 4; i++)
            {
                int buttonIndex = i;
                optionVOButtons[i].onClick.AddListener(() => OnOptionVOClick?.Invoke(buttonIndex));
            }
        }

        private void OnDestroy()
        {
            for (int i = 0; i < 4; i++)
            {
                optionButtons[i].onClick.RemoveAllListeners();
            }

            questionSceneConfirmButton.onClick.RemoveAllListeners();
            questionScenePrevButton.onClick.RemoveAllListeners();

            answerSlider.onValueChanged.RemoveAllListeners();

            scenarioInfoCloseButton.onClick.RemoveAllListeners();
            scenarioInfoStartButton.onClick.RemoveAllListeners();

            photoCaptureShutterButton.onClick.RemoveAllListeners();
            postCameraNextButton.onClick.RemoveAllListeners();

            // VO Buttons

            scenarioNameVOButton.onClick.RemoveAllListeners();
            scenarioDescVOButton.onClick.RemoveAllListeners();

            startSceneTitleVOButton.onClick.RemoveAllListeners();

            startSceneNarrationVOButton.onClick.RemoveAllListeners();
            storySceneNarrationVOButton.onClick.RemoveAllListeners();
            questionSceneNarrationVOButton.onClick.RemoveAllListeners();
            endSceneNarrationVOButton.onClick.RemoveAllListeners();
            photoCaptureSceneNarrationVOButton.onClick.RemoveAllListeners();

            questionVOButton.onClick.RemoveAllListeners();

            for (int i = 0; i < 4; i++)
            {
                optionVOButtons[i].onClick.RemoveAllListeners();
            }
        }

        private void Start()
        {
            answerSlider.value = 1;
            UpdateSliderLabelImage((int)answerSlider.value);
        }

        #region Scenario Methods
        public Button GetGameEndButton => gameEndButton;
        #endregion

        #region Start Scene Methods
        public Button StartScenePlayButton => startScenePlayButton;

        public void SetStartSceneUIGameobjectActive(bool active)
        {
            startSceneUIGameobject.SetActive(active);
        }

        public void SetScenarioNameText(string scenarioName)
        {
            scenarioNameTextInScene.SetText(scenarioName);
        }

        public void SetStartSceneNarrationBoxActive(bool active)
        {
            startSceneNarrationBoxGameobject.SetActive(active);
        }

        public void SetStartSceneNarrationText(string storySceneNarration)
        {
            startSceneNarrationText.SetText(storySceneNarration);
        }
        #endregion

        #region Story Scene Methods

        public Button StorySceneNextButton => storySceneNextButton;
        public Button StoryScenePrevButton => storyScenePrevButton;

        public void SetStartStoryUIGameobjectActive(bool active)
        {
            storySceneUIGameobject.SetActive(active);
        }

        public void SetStorySceneNarrationBoxActive(bool active)
        {
            storySceneNarrationBoxGameobject.SetActive(active);
        }

        public void SetStorySceneNarrationText(string storySceneNarration)
        {
            storySceneNarrationText.SetText(storySceneNarration);
        }
        #endregion

        #region Question Scene Methods

        public GameObject CELabelGameobject => ceLabelGameobject;
        public GameObject AELabelGameobject => aeLabelGameobject;
        public Slider AnswerSlider => answerSlider;

        public void SetOptionText(string[] text)
        {
            for (int i = 0; i < optionButtonText.Length; i++)
            {
                optionButtonText[i].SetText(text[i]);
            }
        }

        public void SetQuestionSceneUIActive(bool active)
        {
            questionSceneUIGameobject.SetActive(active);
        }

        public void SetOptionPanelActive(bool active)
        {
            optionPanelGameobject.SetActive(active);
        }

        public void SetSliderPanelActive(bool active)
        {
            sliderPanelGameobject.SetActive(active);
        }

        public void SetSliderToDefault()
        {
            answerSlider.value = 1;
        }

        public void SetQuestionText(string question)
        {
            questionText.SetText(question);
        }

        public void SetQuestionSceneNarrationBoxActive(bool active)
        {
            questionSceneNarrationBoxGameobject.SetActive(active);
        }

        public void SetQuestionSceneNarrationText(string questionSceneNarration)
        {
            questionSceneNarrationText.SetText(questionSceneNarration);
        }

        public void SetSliderHigherLowerLabels(string _lower, string _higher)
        {
            aeLowerLabel.text = "Extremely " + _lower;
            aeHigherLabel.text = "Extremely " + _higher;
        }

        public void SetOptionSelected(int _optionNo)
        {
            optionButtons[_optionNo].Select();
        }

        public void SetSliderValue(int _sliderVal)
        {
            answerSlider.value = _sliderVal;
        }

        public void UpdateSliderLabelImage(int selectedValue)
        {
            for (int i = 0; i < sliderLabels.Length; i++)
            {
                if (i == selectedValue - 1)
                    sliderLabels[i].sprite = selectedSliderLabelSprite;
                else
                    sliderLabels[i].sprite = deselectedSliderLabelSprite;
            }
        }
        #endregion

        #region Photo Capture Scene Methods

        public Button PhotoCaptureSceneNextButton => photoCaptureSceneNextButton;
        public Button PhotoCaptureScenePrevButton => photoCaptureScenePrevButton;

        public void SetPhotoCaptureUIGameobjectActive(bool active)
        {
            photoCaptureSceneUIGameobject.SetActive(active);

            cameraUIGameObject.SetActive(true);
            postCameraUIGameObject.SetActive(false);
            collageUIGameObject.SetActive(false);
        }

        public void SetPhotoCaptureNarrationBoxActive(bool active)
        {
            photoCaptureSceneNarrationBoxGameobject.SetActive(active);
        }

        public void SetPhotoCaptureNarrationText(string storySceneNarration)
        {
            photoCaptureSceneNarrationText.SetText(storySceneNarration);
        }

        public void ShutterButtonClicked()
        {
            cameraUIGameObject.SetActive(false);
            postCameraUIGameObject.SetActive(true);
            OnPhotoCapture?.Invoke();
        }

        public void PostCameraNextButtonClicked()
        {
            postCameraUIGameObject.SetActive(false);
            collageUIGameObject.SetActive(true);
        }

        #endregion

        #region End Scene Methods

        public Button EndSceneEndButton => endSceneEndButton;
        public Button EndScenePrevButton => endScenePrevButton;

        public void SetEndStoryUIGameobjectActive(bool active)
        {
            endSceneUIGameobject.SetActive(active);
        }

        public void SetEndSceneNarrationBoxActive(bool active)
        {
            endSceneNarrationBoxGameobject.SetActive(active);
        }

        public void SetEndSceneNarrationText(string EndSceneNarration)
        {
            endSceneNarrationText.SetText(EndSceneNarration);
        }
        #endregion

        #region Scenario Info Methods

        public void EnableScenarioInfo(int _scenarioNo, string _scenarioName, string _scenarioDescription)
        {
            scenarioInfoName.text = _scenarioName;
            scenarioInfoDesc.text = _scenarioDescription;

            foreach (GameObject go in scenarioInfoImages)
            {
                go.SetActive(false);
            }
            scenarioInfoImages[_scenarioNo - 1].SetActive(true);

            currentScenarioInfoIndex = _scenarioNo;

            scenarioInfoPanel.SetActive(true);
            scenarioInfoBeforeGO.SetActive(true);
            scenarioInfoAfterGO.SetActive(false);
        }

        public void ScenarioInfoCloseButtonClick()
        {
            scenarioInfoPanel.SetActive(false);
        }

        public void ScenarioPlayButtonClick()
        {
            scenarioInfoBeforeGO.SetActive(false);
            scenarioInfoAfterGO.SetActive(true);

            scenarioInfoLoadingBarFill.fillAmount = 0;
            scenarioInfoLoadingBarFill.DOFillAmount(1f, 3f).SetEase(Ease.InOutQuad).OnComplete(() => { OnScenarioStart?.Invoke(currentScenarioInfoIndex); scenarioInfoPanel.SetActive(false); });
        }

        public void PlayScenarioDescVO()
        {
            string scenarioDescClipName = $"sd_{currentScenarioInfoIndex}";
            SoundManagerService.Instance.PlayVoiceOver(scenarioDescClipName);
        }

        public void PlayScenarioTitleVO()
        {
            string scenarioNameClip = $"sn_{currentScenarioInfoIndex}";
            SoundManagerService.Instance.PlayVoiceOver(scenarioNameClip);
        }

        #endregion
    }
}