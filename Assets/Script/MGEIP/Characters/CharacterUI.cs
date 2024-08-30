using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MGEIP.Characters
{
    public class CharacterUI : MonoBehaviour
    {
        [Header("Character Components")]
        [SerializeField] private GameObject zoomOutCharacter;
        [SerializeField] private GameObject zoomInCharacter;

        [SerializeField] private Image zoomOutMainCharacter;
        [SerializeField] private Image zoomOutSideCharacter;

        [SerializeField] private Image zoomInMainCharacter;
        [SerializeField] private Image zoomInSideCharacter;

        [SerializeField] private GameObject zoomOutMainCharDialogueBox;
        [SerializeField] private TextMeshProUGUI zoomOutMainCharDialogueText;

        [SerializeField] private GameObject zoomOutSideCharDialogueBox;
        [SerializeField] private TextMeshProUGUI zoomOutSideCharDialogueText;

        [SerializeField] private GameObject zoomInMainCharDialogueBox;
        [SerializeField] private TextMeshProUGUI zoomInMainCharDialogueText;

        [SerializeField] private GameObject zoomInSideCharDialogueBox;
        [SerializeField] private TextMeshProUGUI zoomInSideCharDialogueText;

        public Image ZoomOutMainCharacter => zoomOutMainCharacter;

        public void ResetCharacterUI()
        {
            SetZoomInCharacterActive(false);
            SetZoomOutCharacterActive(false);
            SetZoomInMainCharacterActive(false);
            SetZoomInSideCharacterActive(false);
            SetZoomOutMainCharacterActive(false);
            SetZoomOutSideCharacterActive(false);
            SetZoomInMainCharDialogueBoxActive(false);
            SetZoomInSideCharDialogueBoxActive(false);
            SetZoomOutMainCharDialogueBoxActive(false);
            SetZoomOutSideCharDialogueBoxActive(false);
        }

        public void SetZoomOutCharacterActive(bool active)
        {
            zoomOutCharacter.SetActive(active);
        }

        public void SetZoomInCharacterActive(bool active)
        {
            zoomInCharacter.SetActive(active);
        }

        public void SetZoomOutMainCharacterActive(bool active)
        {
            zoomOutMainCharacter.gameObject.SetActive(active);
        }
        
        public void SetZoomOutSideCharacterActive(bool active)
        {
            zoomOutSideCharacter.gameObject.SetActive(active);
        }
        
        public void SetZoomInMainCharacterActive(bool active)
        {
            zoomInMainCharacter.gameObject.SetActive(active);
        }
        
        public void SetZoomInSideCharacterActive(bool active)
        {
            zoomInSideCharacter.gameObject.SetActive(active);
        }
        
        public void SetZoomOutMainCharDialogueBoxActive(bool active)
        {
            zoomOutMainCharDialogueBox.SetActive(active);
        }
        
        public void SetZoomOutSideCharDialogueBoxActive(bool active)
        {
            zoomOutSideCharDialogueBox.SetActive(active);
        }
        
        public void SetZoomInMainCharDialogueBoxActive(bool active)
        {
            zoomInMainCharDialogueBox.SetActive(active);
        }

        public void SetZoomInSideCharDialogueBoxActive(bool active)
        {
            zoomInSideCharDialogueBox.SetActive(active);
        }

        public void SetZoomOutMainCharDialogueText(string text)
        {
            zoomOutMainCharDialogueText.SetText(text);
        } 
        
        public void SetZoomOutSideCharDialogueText(string text)
        {
            zoomOutSideCharDialogueText.SetText(text);
        }
        
        public void SetZoomInMainCharDialogueText(string text)
        {
            zoomInMainCharDialogueText.SetText(text);
        } 
        
        public void SetZoomInSideCharDialogueText(string text)
        {
            zoomInSideCharDialogueText.SetText(text);
        }    
    }
}