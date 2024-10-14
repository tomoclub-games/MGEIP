using MGIEP;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AudioClipButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite enabledSpeaker;
    [SerializeField] private Sprite disabledSpeaker;

    [Header("Audio Info")]
    [SerializeField] private string audioClipName;
    [SerializeField] private AudioButtonType audioButtonType;

    public Button Button => button;

    private void Awake()
    {
        if (audioButtonType == AudioButtonType.Custom)
            return;
        else if (audioButtonType == AudioButtonType.StopClip)
            button.onClick.AddListener(StopAudioClip);
        else
            button.onClick.AddListener(PlayAudioClip);
    }

    private void OnDestroy()
    {
        if (audioButtonType == AudioButtonType.Custom)
            return;
        else if (audioButtonType == AudioButtonType.StopClip)
            button.onClick.RemoveListener(StopAudioClip);
        else
            button.onClick.RemoveListener(PlayAudioClip);
    }

    private void PlayAudioClip()
    {
        string clipName = "";

        switch (audioButtonType)
        {
            case AudioButtonType.InstructionClip: clipName = "Instructions/"; break;
            case AudioButtonType.MainMenuClip: clipName = "MainMenu/"; break;
            case AudioButtonType.GameSceneClip: clipName = "GameScene/"; break;
            default: break;
        }

        SoundManagerService.Instance.OnPlayVoiceOver?.Invoke(clipName + audioClipName);
        SoundManagerService.Instance.OnVoiceOverComplete += ResetButton;

        button.enabled = false;
        buttonImage.sprite = disabledSpeaker;
    }

    public void PlayAudioClip(string _audioClipName)
    {
        SoundManagerService.Instance.OnPlayVoiceOver?.Invoke(_audioClipName);
        SoundManagerService.Instance.OnVoiceOverComplete += ResetButton;

        button.enabled = false;
        buttonImage.sprite = disabledSpeaker;
    }

    private void StopAudioClip()
    {
        SoundManagerService.Instance.OnStopVoiceOver?.Invoke();
    }

    private void ResetButton()
    {
        SoundManagerService.Instance.OnVoiceOverComplete -= ResetButton;

        button.enabled = true;
        buttonImage.sprite = enabledSpeaker;
    }
}

public enum AudioButtonType
{
    InstructionClip, // use custom for instructions instead
    StopClip,
    Custom,
    MainMenuClip,
    GameSceneClip,
}