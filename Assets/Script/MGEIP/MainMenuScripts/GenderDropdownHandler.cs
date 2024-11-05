using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GenderDropdownHandler : MonoBehaviour
{
    [SerializeField] private CanvasGroup dropdownPanel;
    [SerializeField] private TMP_Text dropdownLabel;
    [SerializeField] private Button dropdownButton;
    [SerializeField] private Transform dropdownArrow;
    [SerializeField] private List<Button> options;
    [SerializeField] private string[] optionTexts;

    [SerializeField] private Color selectedTextColor;

    private int currentSelection;

    public int CurrentSelection => currentSelection;
    public string CurrentSelectionText => optionTexts[currentSelection];

    private void Awake()
    {
        for (int i = 0; i < options.Count; i++)
        {
            int index = i;
            options[index].onClick.AddListener(() => OptionClicked(index));
        }

        currentSelection = -1;
        dropdownLabel.text = "Select Gender";

        dropdownButton.onClick.AddListener(ToggleDropdown);
    }

    private void OptionClicked(int _indexNo)
    {
        currentSelection = _indexNo;

        dropdownLabel.text = optionTexts[_indexNo];
        dropdownLabel.color = selectedTextColor;

        CloseDropdown();
    }

    private void OpenDropdown()
    {
        dropdownPanel.gameObject.SetActive(true);
        dropdownPanel.alpha = 0;

        dropdownPanel.DOFade(1, 0.1f);

        dropdownArrow.localScale = new Vector3(1, 1, 1);
    }

    private void ToggleDropdown()
    {
        if (dropdownPanel.gameObject.activeSelf)
            CloseDropdown();
        else
            OpenDropdown();
    }

    private void CloseDropdown()
    {
        dropdownPanel.DOFade(0, 0.1f).OnComplete(() =>
        {
            dropdownPanel.gameObject.SetActive(false);
        });

        dropdownArrow.localScale = new Vector3(1, -1, 1);
    }
}
