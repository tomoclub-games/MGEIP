using Assets.Script.MGEIP.Service;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropMe : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private int scenarioNo;
	[SerializeField] private TMP_Text messageText;
	[SerializeField] private Image receivingImage;
	[SerializeField] private CanvasGroup canvasGroup;
	[SerializeField] private CanvasGroup greenHighlightOverlay;
	[SerializeField] private CanvasGroup redHighlightOverlay;
	[SerializeField] private CanvasGroup hoverOverlay;
	[SerializeField] private float blinkDuration = 0.2f;
	[SerializeField] private float displayDuration = 5f;

	private EndMenuService endMenuService;

	private DragMe currentDragMe;

	private Sequence messageSequence;
	private Sequence hoverOverlaySequence;

	public void Init(EndMenuService _endMenuService)
	{
		endMenuService = _endMenuService;

		ClearReceivingImage();
	}

	private void ClearReceivingImage()
	{
		receivingImage.sprite = null;
		receivingImage.color = Color.clear;
	}

	public void OnDrop(PointerEventData data)
	{
		if (receivingImage == null || currentDragMe != null)
			return;

		DragMe dragMe = data.pointerDrag.GetComponent<DragMe>();
		if (dragMe != null)
		{
			hoverOverlay.alpha = 0;
			currentDragMe = dragMe;
			currentDragMe.FadeOutDragMe();
			PlaceDragMe();
		}
	}

	private void PlaceDragMe()
	{
		currentDragMe.DisableClicks();

		if (scenarioNo == currentDragMe.ScenarioNo)
			AcceptDragMe();
		else
			DenyDragMe();

		FadeHoverOverlay(false);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (currentDragMe != null)
			return;

		GameObject dragObject = eventData.pointerDrag;

		if (dragObject != null && dragObject.GetComponent<DragMe>() != null)
		{
			if (hoverOverlay != null)
			{
				FadeHoverOverlay(true);
			}
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (currentDragMe != null)
			return;

		GameObject dragObject = eventData.pointerDrag;

		if (dragObject != null && dragObject.GetComponent<DragMe>() != null)
		{
			if (hoverOverlay != null)
			{
				FadeHoverOverlay(false);
			}
		}
	}

	#region Highlight

	private void AcceptDragMe()
	{
		currentDragMe.PlacedPolaroid();

		receivingImage.overrideSprite = endMenuService.GetNewspaperSpriteForScenario(currentDragMe.ScenarioNo);
		receivingImage.DOColor(Color.white, 0.25f);

		messageText.gameObject.SetActive(false);
		hoverOverlay.gameObject.SetActive(false);

		greenHighlightOverlay.alpha = 0;
		greenHighlightOverlay.gameObject.SetActive(true);

		greenHighlightOverlay.DOFade(0.75f, blinkDuration)
			.OnComplete(() => greenHighlightOverlay.DOFade(0, blinkDuration)
			.OnComplete(() => greenHighlightOverlay.gameObject.SetActive(false))
			.OnComplete(() => endMenuService.PlacedPolaroid()));
	}

	private void DenyDragMe()
	{
		redHighlightOverlay.alpha = 0;
		redHighlightOverlay.gameObject.SetActive(true);

		DisplayErrorMessage();

		redHighlightOverlay.DOFade(0.75f, blinkDuration)
			.OnComplete(() => redHighlightOverlay.DOFade(0, blinkDuration)
			.OnComplete(() => redHighlightOverlay.DOFade(0.75f, blinkDuration)
			.OnComplete(() => redHighlightOverlay.DOFade(0, blinkDuration)
			.OnComplete(() =>
			{
				redHighlightOverlay.gameObject.SetActive(false);
				FadeOutReceivingImage();
			}))));
	}

	#endregion

	private void FadeOutReceivingImage()
	{
		receivingImage.DOFade(0, blinkDuration)
				.OnComplete(() =>
				{
					receivingImage.sprite = null;
					receivingImage.color = Color.clear;
					currentDragMe.ResetDragMePosition();
					currentDragMe = null;
				});
	}

	private void DisplayErrorMessage()
	{
		if (messageSequence != null && messageSequence.IsActive())
		{
			messageSequence.Kill();
		}

		messageSequence = DOTween.Sequence();

		messageSequence.AppendCallback(() => messageText.text = "<color=#FF0000>Whoops! That was the wrong polaroid!</color>");
		messageSequence.AppendInterval(5f);
		messageSequence.Append(messageText.DOFade(0, 0.5f));
		messageSequence.AppendCallback(() => messageText.text = "Fill this place up");
		messageSequence.Append(messageText.DOFade(1, 0.5f));
	}

	private void FadeHoverOverlay(bool fadeIn)
	{
		if (hoverOverlaySequence != null && hoverOverlaySequence.IsActive())
		{
			hoverOverlaySequence.Kill();
		}

		hoverOverlaySequence = DOTween.Sequence();

		if (fadeIn)
		{
			hoverOverlaySequence.Append(hoverOverlay.DOFade(1, 0.25f));
		}
		else
		{
			hoverOverlaySequence.Append(hoverOverlay.DOFade(0, 0.25f));
		}
	}
}
