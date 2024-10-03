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
        if (audioButtonType == AudioButtonType.PlayClip)
            button.onClick.AddListener(PlayAudioClip);
        else if (audioButtonType == AudioButtonType.StopClip)
            button.onClick.AddListener(StopAudioClip);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }

    private void PlayAudioClip()
    {
        SoundManagerService.Instance.OnPlayVoiceOver?.Invoke(audioClipName);
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
    PlayClip,
    StopClip,
    Custom
}