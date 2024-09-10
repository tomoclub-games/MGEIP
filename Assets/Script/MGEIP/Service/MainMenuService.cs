using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Script.MGEIP.Service
{
    public class MainMenuService : MonoBehaviour
    {
        [SerializeField] private GameObject startMenu;
        [SerializeField] private Button startMenuNextButton;

        [SerializeField] private GameObject disclaimerMenu;
        [SerializeField] private Button disclaimerMenuNextButton;

        [SerializeField] private GameObject storyMenu;
        [SerializeField] private Button storyMenuNextButton;

        [SerializeField] private GameObject TutorialMenu;
        [SerializeField] private Button StartGameButton;

        private void Start()
        {
            startMenu.SetActive(true);
            disclaimerMenu.SetActive(false);
            storyMenu.SetActive(false);
            TutorialMenu.SetActive(false);

            startMenuNextButton.onClick.AddListener(EnableDisclaimerMenu);
            disclaimerMenuNextButton.onClick.AddListener(EnableStoryMenu);
            storyMenuNextButton.onClick.AddListener(EnableTutorialMenu);
            StartGameButton.onClick.AddListener(StartGame);
        }

        private void EnableDisclaimerMenu()
        {
            startMenu.SetActive(false);
            disclaimerMenu.SetActive(true);
        }

        private void EnableStoryMenu()
        {
            disclaimerMenu.SetActive(false );
            storyMenu.SetActive(true);
        }

        private void EnableTutorialMenu()
        {
            storyMenu.SetActive(false ) ;
            TutorialMenu.SetActive (true );
        }

        private void StartGame()
        {
            SceneManager.LoadSceneAsync(1);
        }
    }
}