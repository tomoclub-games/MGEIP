using System;
using System.Collections;
using System.Collections.Generic;
using MGIEP;
using UnityEngine;

public class SceneSoundPlayer : MonoBehaviour
{
    private string sceneSoundName;
    private string musicSoundName;
    private string outroSoundName;

    public void SetSound(string _bgSound, string _sceneSound, string _outroSound = "")
    {
        musicSoundName = _bgSound;
        sceneSoundName = _sceneSound;
        outroSoundName = _outroSound;
    }

    public void SceneEnter()
    {
        PlaySceneSound();
        PlayMusicSound();
    }

    public void SceneExit()
    {
        StopSceneSound();
    }

    private void PlaySceneSound()
    {
        if (String.IsNullOrEmpty(sceneSoundName))
            SoundManagerService.Instance.OnStopSceneSound?.Invoke();
        else
            SoundManagerService.Instance.OnPlaySceneSound?.Invoke(sceneSoundName);
    }

    private void PlayMusicSound()
    {
        if (String.IsNullOrEmpty(musicSoundName))
            SoundManagerService.Instance.OnStopMusic?.Invoke();
        else
            SoundManagerService.Instance.OnPlayMusic?.Invoke(musicSoundName);
    }

    private void StopSceneSound()
    {
        SoundManagerService.Instance.OnStopSceneSound?.Invoke();
    }

    public void PlayOutroSound()
    {
        if (string.IsNullOrEmpty(outroSoundName))
            return;

        SoundManagerService.Instance.OnStopMusic?.Invoke();
        SoundManagerService.Instance.OnPlaySceneSound?.Invoke(outroSoundName);
    }
}
