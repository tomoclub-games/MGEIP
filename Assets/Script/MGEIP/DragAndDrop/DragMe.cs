using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DragMe : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	[SerializeField] private int scenarioNo;

	[SerializeField] private Texture2D dragCursorTexture;

	private Canvas canvas;
	private Image image;
	private CanvasGroup canvasGroup;
	private RectTransform rectTransform;
	private Vector3 initialPosition;
	private Quaternion initialRotation;
	private Vector3 initialScale;
	private bool isPlacementDone;

	public int ScenarioNo => scenarioNo;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		canvasGroup = GetComponent<CanvasGroup>();
		image = GetComponent<Image>();

		initialPosition = rectTransform.localPosition;
		initialRotation = rectTransform.localRotation;
		initialScale = rectTransform.localScale;
	}

	public void Init(Canvas _canvas)
	{
		canvas = _canvas;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		canvasGroup.blocksRaycasts = false;

		Cursor.SetCursor(dragCursorTexture, Vector2.zero, CursorMode.Auto);

		SetDraggedPosition(eventData);

		rectTransform.DORotate(Vector3.zero, 0.5f);
		rectTransform.DOScale(Vector3.one, 0.5f);
	}

	public void OnDrag(PointerEventData eventData)
	{
		SetDraggedPosition(eventData);

		transform.SetAsLastSibling();
	}

	private void SetDraggedPosition(PointerEventData eventData)
	{
		Vector3 globalMousePos;
		RectTransform rt = GetComponent<RectTransform>();

		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out globalMousePos))
		{
			rt.position = globalMousePos;
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        if (!isPlacementDone)
		{
			canvasGroup.DOFade(0, 0.5f).OnComplete(ResetDragMePosition);
		}
	}

	public void FadeOutDragMe()
	{
		canvasGroup.DOFade(0, 0.5f);
	}

	public void ResetDragMePosition()
	{
		rectTransform.localPosition = initialPosition;
		rectTransform.localRotation = initialRotation;
		rectTransform.localScale = initialScale;

		gameObject.SetActive(true);

		canvasGroup.DOFade(1f, 0.5f).OnComplete(() => EnableClicks());
	}

	public void EnableClicks()
	{
		canvasGroup.blocksRaycasts = true;
	}

	public void DisableClicks()
	{
		canvasGroup.blocksRaycasts = false;
	}

	public void PlacedPolaroid()
	{
		isPlacementDone = true;
	}
}
