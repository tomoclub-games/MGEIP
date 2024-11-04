using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GenderDropdownHandler : MonoBehaviour
{
    private TMP_Text dropdownLabel;

    private int currentSelection;
    private Transform optionsParent;
    private List<Button> options = new();

    private void Awake()
    {
        
    }
}
