using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MGEIP
{
    [RequireComponent(typeof(Button), typeof(Image), typeof(RectTransform))]
    public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private bool animateOnHover = true;
        [SerializeField] private Vector3 hoverScale = new(1.2f, 1.2f, 1f);

        [Header("References")]
        [SerializeField] private Button button;
        [SerializeField] private Image buttonImage;
        [SerializeField] private TMP_Text buttonLabel;
        [SerializeField] private RectTransform rectTransform;

        private Vector3 originalScale;

        private void Awake()
        {
            originalScale = rectTransform.localScale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (animateOnHover)
                rectTransform.DOScale(hoverScale, 0.25f).SetEase(Ease.OutBack);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (animateOnHover)
                rectTransform.DOScale(originalScale, 0.25f).SetEase(Ease.OutBack);
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