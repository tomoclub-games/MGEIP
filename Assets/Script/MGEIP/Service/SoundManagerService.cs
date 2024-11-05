using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
// using UnityEngine.ResourceManagement.AsyncOperations;

namespace MGIEP
{
    public class SoundManagerService : MonoBehaviour
    {
        public static SoundManagerService Instance;

        [SerializeField] private AudioSource voiceOverSource;
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sceneSoundSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("SFX Clips")]
        [SerializeField] private List<SFX> sfxSounds;

        private bool isVoiceOverCancelled;

        // private IList<AudioClip> loadedClips = new List<AudioClip>();
        // private Stack<AsyncOperationHandle<IList<AudioClip>>> handles = new();

        private Coroutine voiceOverCoroutine;

        public UnityAction<string> OnPlayVoiceOver;
        public UnityAction OnStopVoiceOver;
        public UnityAction OnVoiceOverComplete;

        public UnityAction<string> OnPlayMusic;
        public UnityAction OnStopMusic;

        public UnityAction<string> OnPlaySceneSound;
        public UnityAction OnStopSceneSound;

        public UnityAction<SFXType> OnPlaySFX;
        public UnityAction OnStopSFX;

        public bool IsVoiceOverCancelled => isVoiceOverCancelled;

        private void Awake()
        {
            if (Instance == null)
            {
                DontDestroyOnLoad(gameObject);
                Instance = this;
            }

            OnPlayVoiceOver += PlayVoiceOver;
            OnStopVoiceOver += StopVoiceOver;

            OnPlayMusic += PlayMusic;
            OnStopMusic += StopMusic;

            OnPlaySceneSound += PlaySceneSound;
            OnStopSceneSound += StopSceneSound;

            OnPlaySFX += PlaySFX;
        }

        private void OnDestroy()
        {
            OnPlayVoiceOver -= PlayVoiceOver;
            OnStopVoiceOver -= StopVoiceOver;

            OnPlayMusic -= PlayMusic;
            OnStopMusic -= StopMusic;

            OnPlaySceneSound -= PlaySceneSound;
            OnStopSceneSound -= StopSceneSound;

            OnPlaySFX -= PlaySFX;
        }

        #region Voice Over

        public void PlayVoiceOver(string _audioClipName)
        {
            isVoiceOverCancelled = false;

            AudioClip audioClip = FindAudioClipByName("Audio/VoiceOvers/" + _audioClipName);

            if (audioClip == null)
            {
                OnVoiceOverComplete?.Invoke();
                return;
            }
            else if (audioClip == voiceOverSource.clip)
            {
                Debug.Log("Already playing!");
                return;
            }

            Debug.Log("Playing audio clip : " + audioClip.name);

            voiceOverSource.clip = audioClip;
            voiceOverSource.Play();

            if (voiceOverCoroutine != null)
            {
                StopCoroutine(voiceOverCoroutine);
                OnVoiceOverComplete?.Invoke();
            }

            voiceOverCoroutine = StartCoroutine(WaitForVoiceOverToFinish());
        }

        private IEnumerator WaitForVoiceOverToFinish()
        {
            yield return new WaitWhile(() => voiceOverSource.isPlaying);

            voiceOverSource.clip = null;
            OnVoiceOverComplete?.Invoke();

            voiceOverCoroutine = null;
        }

        public bool IsPlayingVoiceOver()
        {
            return voiceOverSource.isPlaying;
        }

        private void StopVoiceOver()
        {
            voiceOverSource.Stop();
            isVoiceOverCancelled = true;
        }

        #endregion

        /*

        #region Addressables

        public void LoadAudio(string addressableGroupLabel)
        {
            AsyncOperationHandle<IList<AudioClip>> handle = Addressables.LoadAssetsAsync<AudioClip>(addressableGroupLabel, null);
            handles.Push(handle);

            handle.Completed += OnSceneAudioLoaded;
        }

        public void OnSceneAudioLoaded(AsyncOperationHandle<IList<AudioClip>> operationHandle)
        {
            if (operationHandle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("Loaded new audio clips");
                loadedClips.AddRange(operationHandle.Result);
            }
            else
            {
                Debug.LogError($"Failed to load Addressable group. Status: {operationHandle.Status}, Error: {operationHandle.OperationException}");
            }
        }

        public void ReleaseAudio()
        {
            if (handles.Count < 1)
                return;

            AsyncOperationHandle<IList<AudioClip>> latestHandle = handles.Pop();

            if (latestHandle.IsValid())
            {
                if (latestHandle.Result != null)
                {
                    foreach (AudioClip audioClip in latestHandle.Result)
                    {
                        loadedClips.Remove(audioClip);
                    }
                }

                Addressables.Release(latestHandle);
                Debug.Log("Released audio clips");
            }
        }

                #endregion
        */

        #region Music

        private void PlayMusic(string _musicName)
        {
            AudioClip audioClip = FindAudioClipByName("Audio/Music/" + _musicName);

            if (audioClip == null || musicSource.clip == audioClip)
                return;

            musicSource.clip = audioClip;
            musicSource.Play();
        }

        private void StopMusic()
        {
            musicSource.Stop();
            musicSource.clip = null;
        }

        #endregion

        #region SFX

        private void PlaySFX(SFXType sFXType)
        {
            AudioClip audioClip = GetAudioForSFXType(sFXType);

            if (audioClip == null)
                return;

            sfxSource.PlayOneShot(audioClip);
        }

        #endregion

        #region Scene sound

        private void PlaySceneSound(string _sceneSoundName)
        {
            AudioClip audioClip = FindAudioClipByName("Audio/SceneSound/" + _sceneSoundName);

            if (audioClip == null)
                return;

            sceneSoundSource.clip = audioClip;
            sceneSoundSource.Play();
        }

        private void StopSceneSound()
        {
            sceneSoundSource.Stop();
            sceneSoundSource.clip = null;
        }

        #endregion

        private AudioClip FindAudioClipByName(string clipName)
        {
            AudioClip audioClip = Resources.Load<AudioClip>(clipName);

            if (audioClip == null)
            {
                Debug.Log("Audio clip not found! : " + clipName);
                return null;
            }

            return audioClip;
        }

        private AudioClip GetAudioForSFXType(SFXType sFXType)
        {
            return sfxSounds.Find(id => id.sfxType == sFXType).audioClip;
        }
    }

    public enum SFXType
    {
        shutterButton,
        optionButton,
        confirmButton
    }

    [Serializable]
    public class SFX
    {
        public SFXType sfxType;
        public AudioClip audioClip;
    }
}
