using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.MGEIP.Service
{
    public class EndMenuService : MonoBehaviour
    {
        [SerializeField] private Button QuitButton;

        private void Start()
        {
            QuitButton.onClick.AddListener(OnQuitButtonClick);
        }

        private void OnQuitButtonClick()
        {
            Application.Quit();
        }
    }
}