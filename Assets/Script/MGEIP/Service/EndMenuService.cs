using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        [SerializeField] private CanvasGroup page3Panel;
        [SerializeField] private CanvasGroup endPanel;
        [SerializeField] private CanvasGroup instructionBox;
        [SerializeField] private CanvasGroup pageFlipInstructionBox;
        [SerializeField] private List<DragMe> dragMes;
        [SerializeField] private List<DropMe> dropMes;
        [SerializeField] private Button endButton;

        private List<Sprite> newspaperSprites = new();

        private int placedPolaroids;
        private int currentPageNumber = 1;
        private int minPages = 1;
        private int maxPages = 3;
        private CanvasGroup currentPage;

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

            Sprite[] sprites = Resources.LoadAll<Sprite>("Art/EndScreen/");
            newspaperSprites.AddRange(sprites);

            endButton.onClick.AddListener(BackToHomeScreen);
        }

        private void OnDestroy()
        {
            introNextButton.onClick.RemoveAllListeners();

            endButton.onClick.RemoveAllListeners();
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

            currentPage = page1Panel;
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

        public void NextPageButtonClicked()
        {
            if (currentPageNumber == maxPages)
                return;

            currentPageNumber++;

            ShowPage(currentPageNumber);
        }

        public void PrevPageButtonClicked()
        {
            if (currentPageNumber == minPages)
                return;

            currentPageNumber--;

            ShowPage(currentPageNumber);
        }

        private void ShowPage(int _pageNumber)
        {
            CanvasGroup newPage = null;
            if (_pageNumber == 1)
                newPage = page1Panel;
            else if (_pageNumber == 2)
                newPage = page2Panel;
            else if (_pageNumber == 3)
                newPage = page3Panel;

            currentPage.blocksRaycasts = false;
            currentPage.DOFade(0, 0.5f).OnComplete(() =>
            {
                currentPage.gameObject.SetActive(false);

                newPage.gameObject.SetActive(true);
                newPage.alpha = 0;
                newPage.DOFade(1, 0.5f).OnComplete(() => newPage.blocksRaycasts = true);

                currentPage = newPage;
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

        public void BackToHomeScreen()
        {
            SceneManager.LoadScene(0);
        }
    }
}