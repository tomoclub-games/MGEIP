using System.Collections;
using System.Collections.Generic;
using MGIEP;
using UnityEngine;
using UnityEngine.UI;

public class SFXSoundPlayer : MonoBehaviour
{
    [SerializeField] private SFXType sFXType;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(PlayButtonSFX);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(PlayButtonSFX);
    }

    private void PlayButtonSFX()
    {
        SoundManagerService.Instance.OnPlaySFX?.Invoke(sFXType);
    }
}
