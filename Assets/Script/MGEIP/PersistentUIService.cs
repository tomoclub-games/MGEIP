using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MGIEP;
using UnityEngine;
using UnityEngine.UI;

public class PersistentUIService : MonoBehaviour
{
    [SerializeField] private Button settingsButton;
    [SerializeField] private CanvasGroup volumePanel;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider voSlider;
    [SerializeField] private Button closeVolumeButton;

    public void Init()
    {
        musicSlider.onValueChanged.AddListener(SoundManagerService.Instance.OnSetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SoundManagerService.Instance.OnSetSFXVolume);
        voSlider.onValueChanged.AddListener(SoundManagerService.Instance.OnSetVoiceOverVolume);

        settingsButton.onClick.AddListener(EnableVolumePanel);
        closeVolumeButton.onClick.AddListener(DisableVolumePanel);
    }

    private void OnDestroy()
    {
        musicSlider.onValueChanged.RemoveAllListeners();
        sfxSlider.onValueChanged.RemoveAllListeners();
        voSlider.onValueChanged.RemoveAllListeners();

        settingsButton.onClick.RemoveListener(EnableVolumePanel);
        closeVolumeButton.onClick.RemoveListener(DisableVolumePanel);
    }

    private void Start()
    {
        volumePanel.gameObject.SetActive(false);
    }

    private void EnableVolumePanel()
    {
        volumePanel.gameObject.SetActive(true);
        volumePanel.alpha = 0;

        volumePanel.DOFade(1, 0.2f);
    }

    private void DisableVolumePanel()
    {
        volumePanel.DOFade(0, 0.5f).OnComplete(() =>
        {
            volumePanel.gameObject.SetActive(false);
        });
    }
}
