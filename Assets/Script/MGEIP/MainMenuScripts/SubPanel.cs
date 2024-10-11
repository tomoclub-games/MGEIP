using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubPanel : MonoBehaviour
{
    [SerializeField] private List<int> subPanelTextIDs;
    [SerializeField] private List<TMP_Text> subPanelTexts;

    public List<TMP_Text> SubPanelTexts => subPanelTexts;
}
