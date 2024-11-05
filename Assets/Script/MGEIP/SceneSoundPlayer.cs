using System;
using System.Collections;
using System.Collections.Generic;
using MGIEP;
using UnityEngine;

public class SceneSoundPlayer : MonoBehaviour
{
    [SerializeField] private string sceneSoundName;
    [SerializeField] private string musicSoundName;

    [SerializeField] private bool stopMusicOnSceneEnter;

    private void OnEnable()
    {
        if (stopMusicOnSceneEnter)
            SoundManagerService.Instance.OnStopMusic?.Invoke();

        PlaySceneSound();
        PlayMusicSound();
    }

    private void OnDisable()
    {
        StopSceneSound();
    }

    private void PlaySceneSound()
    {
        if (String.IsNullOrEmpty(sceneSoundName))
            return;

        SoundManagerService.Instance.OnPlaySceneSound?.Invoke(sceneSoundName);
    }

    private void PlayMusicSound()
    {
        if (String.IsNullOrEmpty(musicSoundName))
            return;

        SoundManagerService.Instance.OnPlayMusic?.Invoke(musicSoundName);
    }

    private void StopSceneSound()
    {
        SoundManagerService.Instance.OnStopSceneSound?.Invoke();
    }
}
