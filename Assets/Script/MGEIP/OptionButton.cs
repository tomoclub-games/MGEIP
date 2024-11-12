using System.Collections;
using System.Collections.Generic;
using MGEIP.Service;
using UnityEngine;
using UnityEngine.UI;

public class OptionButton : MonoBehaviour
{
    private GameUIService gameUIService;

    private Button button;
    private Image buttonImage;

    public Sprite defaultSprite;
    public Sprite selectedSprite;

    private bool isLocked;

    private int buttonIndex;

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();

        buttonImage.sprite = defaultSprite;
        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }

    public void Init(int _buttonIndex, GameUIService _gameUIService)
    {
        buttonIndex = _buttonIndex;
        gameUIService = _gameUIService;
    }

    private void OnButtonClicked()
    {
        if (isLocked) return;

        SetOptionSelected();
        gameUIService.OnMCQOptionSelect?.Invoke(buttonIndex);
    }

    public void LockButton()
    {
        isLocked = true;
        button.interactable = false;
    }

    public void UnlockButton()
    {
        isLocked = false;
        button.interactable = true;
        buttonImage.sprite = defaultSprite;
    }

    public void SetOptionSelected()
    {
        gameUIService.DeselectAllOptions();
        buttonImage.sprite = selectedSprite;
        buttonImage.pixelsPerUnitMultiplier = 4;
    }

    public void DeselectOption()
    {
        buttonImage.sprite = defaultSprite;
        buttonImage.pixelsPerUnitMultiplier = 3;
    }
}
