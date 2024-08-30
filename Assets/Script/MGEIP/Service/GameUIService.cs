using MGEIP.Characters;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace MGEIP.Service
{
    public class GameUIService : MonoBehaviour
    {
        [Header("Scenario Components")]
        public GameObject sceneHolder;
        public GameObject MapUI;
        public Sprite tickSprite;
        [SerializeField] private SpriteRenderer scenarioBackgroundSprite;
        [SerializeField] private SpriteRenderer scenarioForegroundSprite;
        [SerializeField] private SpriteAtlas scenarioBackgroundAtlas;

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

        [Header("Question Scene Components")]
        [SerializeField] private GameObject questionSceneUIGameobject;
        [SerializeField] private TextMeshProUGUI questionText;
        [SerializeField] private Button questionSceneConfirmButton;

        [Header("Options Components")]
        [SerializeField] private GameObject optionPanelGameobject;
        [SerializeField] private Button[] optionButton;
        [SerializeField] private TextMeshProUGUI[] optionButtonText;

        [Header("Slider Components")]
        [SerializeField] private GameObject sliderPanelGameobject;
        [SerializeField] private Slider answerSlider;
        [SerializeField] private GameObject ceLabelGameobject;
        [SerializeField] private GameObject aeLabelGameobject;

        [Header("End Scene Components")]
        [SerializeField] private GameObject endSceneUIGameobject;
        [SerializeField] private GameObject endSceneNarrationBoxGameobject;
        [SerializeField] private TextMeshProUGUI endSceneNarrationText;
        [SerializeField] private Button endSceneEndButton;

        [Header("Character Components")]
        [SerializeField] private CharacterUI characterUI;

        #region Scenario Methods

        public void SetScenarioBackgroundSprite(string spriteName)
        {
            if(spriteName == null)
                scenarioBackgroundSprite.gameObject.SetActive(false);
            else
                scenarioBackgroundSprite.sprite = GetScenarioBackgroundSprite(spriteName);
        }

        public void SetScenarioForegroundSprite(string spriteName)
        {
            if(spriteName == null)
                scenarioForegroundSprite.gameObject.SetActive(false);
            else
                scenarioForegroundSprite.sprite = GetScenarioBackgroundSprite(spriteName);
        }

        public Sprite GetScenarioBackgroundSprite(string spiteName)
        {
            if(spiteName != null)
                return scenarioBackgroundAtlas.GetSprite(spiteName);
            return null;
        }
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
        public Button[] OptionButton => optionButton;
        public Button QuestionSceneConfirmButton => questionSceneConfirmButton;

        public void SetOptionText(string[] text)
        {
            for(int i = 0; i < optionButtonText.Length; i++)
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

        public void SetQuestionText(string question)
        {
            questionText.SetText(question);
        }
        #endregion

        #region End Scene Methods

        public Button EndSceneEndButton => endSceneEndButton;

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

        #region Character Methods
        public CharacterUI GetCharacterUI() => characterUI;

        #endregion
    }
}