using MGIEP;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AudioClipButton : MonoBehaviour
{
    [SerializeField] private string audioClipName;
    [SerializeField] private AudioButtonType audioButtonType;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PlayAudioClip);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }

    private void PlayAudioClip()
    {
        if (audioButtonType == AudioButtonType.PlayClip)
            SoundManagerService.Instance.OnPlayVoiceOver?.Invoke(audioClipName);
        else
            SoundManagerService.Instance.OnStopVoiceOver?.Invoke();
    }
}

public enum AudioButtonType
{
    PlayClip,
    StopClip
}