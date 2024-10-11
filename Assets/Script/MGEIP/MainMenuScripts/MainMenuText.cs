using System.Collections;
using System.Collections.Generic;
using Assets.Script.MGEIP.Service;
using TMPro;
using UnityEngine;

public class MainMenuText : MonoBehaviour
{
    [SerializeField] private int textID;

    private TMP_Text textLabel;

    private void Awake()
    {
        textLabel = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        textLabel.text = MainMenuService.Instance.MainMenuDataContainer.MainMenuContent.mainMenuDataList[textID - 1].TextContent;
    }
}
