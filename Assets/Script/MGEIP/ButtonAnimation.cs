using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MGEIP
{
    [RequireComponent(typeof(Button), typeof(RectTransform))]
    public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private bool animateOnHover = true;
        [SerializeField] private Vector3 hoverScale;// = new(1.2f, 1.2f, 1f);

        [Header("References")]
        [SerializeField] private Image hoverImage;
        [SerializeField] private Button button;
        [SerializeField] private Image buttonImage;
        [SerializeField] private TMP_Text buttonLabel;
        [SerializeField] private RectTransform rectTransform;

        private Vector3 originalScale;

        private void Awake()
        {
            originalScale = rectTransform.localScale;
            hoverScale = new(originalScale.x + 0.2f, originalScale.y + 0.2f, originalScale.z + 0f);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (animateOnHover && button.interactable)
            {
                rectTransform.DOScale(hoverScale, 0.25f).SetEase(Ease.OutBack);
                if (hoverImage != null)
                {
                    hoverImage.gameObject.SetActive(true);
                    hoverImage.DOFade(1, 0.25f);
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (animateOnHover)
            {
                rectTransform.DOScale(originalScale, 0.25f).SetEase(Ease.OutBack);
                if (hoverImage != null)
                    hoverImage.DOFade(0, 0.25f).OnComplete(() => hoverImage.gameObject.SetActive(false));
            }
        }

        public void EnableButton()
        {
            button.interactable = true;

            buttonImage.DOFade(1, 0.25f);

            if (buttonLabel != null)
                buttonLabel.DOFade(1, 0.25f);
        }

        public void DisableButton()
        {
            button.interactable = false;

            buttonImage.DOFade(0.5f, 0.25f);

            if (buttonLabel != null)
                buttonLabel.DOFade(0.5f, 0.25f);
        }
    }
}
