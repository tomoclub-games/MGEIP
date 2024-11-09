using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MGIEP;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PersistentUIService : MonoBehaviour
{
    [SerializeField] private Button settingsButton;
    [SerializeField] private CanvasGroup volumePanel;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider voSlider;
    [SerializeField] private TMP_Text musicSliderLabel;
    [SerializeField] private TMP_Text sfxSliderLabel;
    [SerializeField] private TMP_Text voSliderLabel;
    [SerializeField] private Button closeVolumeButton;

    public void Init()
    {
        musicSlider.onValueChanged.AddListener(SoundManagerService.Instance.OnSetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SoundManagerService.Instance.OnSetSFXVolume);
        voSlider.onValueChanged.AddListener(SoundManagerService.Instance.OnSetVoiceOverVolume);

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        voSlider.onValueChanged.AddListener(SetVOVolume);

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

    public void SetInitialVolumes(float _musicVolume, float _sfxVolume, float _voiceOverVolume)
    {
        musicSlider.value = _musicVolume;
        SetMusicVolume(_musicVolume);

        sfxSlider.value = _sfxVolume;
        SetSFXVolume(_sfxVolume);

        voSlider.value = _voiceOverVolume;
        SetVOVolume(_voiceOverVolume);
    }

    private void EnableVolumePanel()
    {
        volumePanel.gameObject.SetActive(true);
        volumePanel.alpha = 0;

        volumePanel.DOFade(1, 0.2f);
    }

    private void DisableVolumePanel()
    {
        volumePanel.DOFade(0, 0.2f).OnComplete(() =>
        {
            volumePanel.gameObject.SetActive(false);
        });
    }

    private void SetMusicVolume(float _volume)
    {
        musicSliderLabel.text = Mathf.Round(_volume * 100).ToString();
    }

    private void SetSFXVolume(float _volume)
    {
        sfxSliderLabel.text = Mathf.Round(_volume * 100).ToString();
    }

    private void SetVOVolume(float _volume)
    {
        voSliderLabel.text = Mathf.Round(_volume * 100).ToString();
    }
}
