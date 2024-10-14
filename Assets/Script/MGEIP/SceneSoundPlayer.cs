using System.Collections;
using System.Collections.Generic;
using MGIEP;
using UnityEngine;

public class SceneSoundPlayer : MonoBehaviour
{
    [SerializeField] private string sceneSoundName;

    private void OnEnable()
    {
        PlaySceneSound();
    }

    private void OnDisable()
    {
        StopSceneSound();
    }

    private void PlaySceneSound()
    {
        if (sceneSoundName == string.Empty)
            return;

        SoundManagerService.Instance.OnPlaySceneSound?.Invoke(sceneSoundName);
    }

    private void StopSceneSound()
    {
        SoundManagerService.Instance.OnStopSceneSound?.Invoke();
    }
}
