using MGIEP;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AudioClipButton : MonoBehaviour
{
    [SerializeField] private string audioClipName;

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
        SoundManagerService.Instance.OnPlayVoiceOver?.Invoke(audioClipName);
    }
}
