using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InstructionController : MonoBehaviour
{
    [SerializeField] private int instructionId;

    [Header("References")]
    [SerializeField] private TMP_Text instructionLabel;
    [SerializeField] private AudioClipButton instructionAudioClipButton;

    private void Awake()
    {
        instructionAudioClipButton.Button.onClick.AddListener(PlayInstructionVoiceOver);
    }

    private void OnDestroy()
    {
        instructionAudioClipButton.Button.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        if (instructionId == 0)
            return;

        instructionLabel.text = InstructionService.Instance.InstructionDataContainer.InstructionContent.InstructionDataList[instructionId - 1].InstructionMessage;
    }

    private void PlayInstructionVoiceOver()
    {
        if (instructionId == 0)
            return;

        string dialogueClipName = $"Instructions/instruction_{instructionId}";
        instructionAudioClipButton.PlayAudioClip(dialogueClipName);
    }
}
