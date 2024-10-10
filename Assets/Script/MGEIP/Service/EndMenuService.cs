using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.MGEIP.Service
{
    public class EndMenuService : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private CanvasGroup newspaperPanel;
        [SerializeField] private CanvasGroup introPanelGO;
        [SerializeField] private Button introNextButton;
        [SerializeField] private CanvasGroup polaroidPanelGO;
        [SerializeField] private CanvasGroup newsPaperPanelGO;
        [SerializeField] private CanvasGroup page1Panel;
        [SerializeField] private CanvasGroup page2Panel;
        [SerializeField] private Button nextPageButton;
        [SerializeField] private Button prevPageButton;
        [SerializeField] private CanvasGroup endPanel;
        [SerializeField] private CanvasGroup instructionBox;
        [SerializeField] private CanvasGroup pageFlipInstructionBox;
        [SerializeField] private List<DragMe> dragMes;
        [SerializeField] private List<DropMe> dropMes;    

        private List<Sprite> newspaperSprites = new();

        private int placedPolaroids;

        private void Awake()
        {
            foreach (DragMe dragMe in dragMes)
            {
                dragMe.Init(canvas);
            }

            foreach (DropMe dropMe in dropMes)
            {
                dropMe.Init(this);
            }

            introNextButton.onClick.AddListener(IntroNextButtonClicked);
            nextPageButton.onClick.AddListener(NextPageButtonClicked);
            prevPageButton.onClick.AddListener(PrevPageButtonClicked);

            Sprite[] sprites = Resources.LoadAll<Sprite>("Art/EndScreen/");
            newspaperSprites.AddRange(sprites);
        }

        private void OnDestroy()
        {
            introNextButton.onClick.RemoveAllListeners();
            nextPageButton.onClick.RemoveAllListeners();
            prevPageButton.onClick.RemoveAllListeners();
        }

        private void Start()
        {
            endPanel.gameObject.SetActive(false);
            introPanelGO.gameObject.SetActive(false);
            polaroidPanelGO.gameObject.SetActive(true);
            instructionBox.gameObject.SetActive(false);
            pageFlipInstructionBox.gameObject.SetActive(false);

            newspaperPanel.gameObject.SetActive(true);
            newspaperPanel.alpha = 0;

            polaroidPanelGO.alpha = 0;
            polaroidPanelGO.DOFade(1, 0.5f);
            polaroidPanelGO.blocksRaycasts = false;

            newspaperPanel.DOFade(1, 0.5f).OnComplete(() =>
            {
                introPanelGO.gameObject.SetActive(true);
                introPanelGO.alpha = 0;
                introPanelGO.blocksRaycasts = false;
                introPanelGO.DOFade(1, 0.5f).OnComplete(() => introPanelGO.blocksRaycasts = true);
            });
        }

        private void IntroNextButtonClicked()
        {
            introNextButton.enabled = false;
            introPanelGO.DOFade(0, 0.5f).OnComplete(() =>
            {
                newsPaperPanelGO.gameObject.SetActive(true);
                newsPaperPanelGO.blocksRaycasts = false;
                newsPaperPanelGO.alpha = 0;
                newsPaperPanelGO.DOFade(1, 0.5f).OnComplete(() =>
                {
                    newsPaperPanelGO.blocksRaycasts = true;
                    polaroidPanelGO.blocksRaycasts = true;
                });
                instructionBox.gameObject.SetActive(true);
                instructionBox.DOFade(0, 0.5f).SetDelay(5f).OnComplete(() =>
                {
                    pageFlipInstructionBox.gameObject.SetActive(true);
                    pageFlipInstructionBox.DOFade(0, 0.5f).SetDelay(5f);
                });
            });
        }

        private void NextPageButtonClicked()
        {
            page1Panel.blocksRaycasts = false;
            page1Panel.DOFade(0, 0.5f).OnComplete(() =>
            {
                page1Panel.gameObject.SetActive(false);

                page2Panel.gameObject.SetActive(true);
                page2Panel.alpha = 0;
                page2Panel.DOFade(1, 0.5f).OnComplete(() => page2Panel.blocksRaycasts = true);
            });
        }

        private void PrevPageButtonClicked()
        {
            page2Panel.blocksRaycasts = false;
            page2Panel.DOFade(0, 0.5f).OnComplete(() =>
            {
                page2Panel.gameObject.SetActive(false);

                page1Panel.gameObject.SetActive(true);
                page1Panel.alpha = 0;
                page1Panel.DOFade(1, 0.5f).OnComplete(() => page1Panel.blocksRaycasts = true);
            });
        }

        public void PlacedPolaroid()
        {
            placedPolaroids++;

            if (placedPolaroids == 10)
                ShowEndScreen();
        }

        public void ShowEndScreen()
        {
            endPanel.alpha = 0;

            Sequence endScreenSequence = DOTween.Sequence();

            endScreenSequence.AppendInterval(1f);
            endScreenSequence.Append(newsPaperPanelGO.DOFade(0, 0.5f));
            endScreenSequence.AppendInterval(1f);
            endScreenSequence.Append(newspaperPanel.DOFade(0, 0.5f));
            endScreenSequence.AppendCallback(() => newspaperPanel.gameObject.SetActive(false));
            endScreenSequence.AppendCallback(() => endPanel.gameObject.SetActive(true));
            endScreenSequence.Append(endPanel.DOFade(1, 0.5f));
        }

        public Sprite GetNewspaperSpriteForScenario(int _scenarioNo)
        {
            return newspaperSprites.Find(sprite => sprite.name == $"sn_{_scenarioNo}_newspaper");
        }
    }
}