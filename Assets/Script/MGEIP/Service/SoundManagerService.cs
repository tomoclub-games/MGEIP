using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace MGIEP
{
    public class SoundManagerService : MonoBehaviour
    {
        public static SoundManagerService Instance;

        [SerializeField] private AudioSource voiceOverSource;

        private bool isVoiceOverCancelled;

        private IList<AudioClip> loadedClips = new List<AudioClip>();
        private Stack<AsyncOperationHandle<IList<AudioClip>>> handles = new();

        public UnityAction<string> OnPlayVoiceOver;
        public UnityAction OnStopVoiceOver;

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
        }

        private void OnDestroy()
        {
            OnPlayVoiceOver -= PlayVoiceOver;
            OnStopVoiceOver -= StopVoiceOver;
        }

        #region Voice Over

        public void PlayVoiceOver(string _audioClipName)
        {
            isVoiceOverCancelled = false;

            AudioClip audioClip = FindVoiceOverByName(_audioClipName);

            if (audioClip == null)
            {
                Debug.LogWarning("Audio clip not found!");
                return;
            }
            else
            {
                Debug.Log("Playing audio clip : " + audioClip.name);
            }

            voiceOverSource.clip = audioClip;
            voiceOverSource.Play();
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

        public AudioClip FindVoiceOverByName(string clipName)
        {
            if (loadedClips != null)
            {
                return loadedClips.FirstOrDefault(clip => clip.name.Equals(clipName, System.StringComparison.OrdinalIgnoreCase));
            }
            return null;
        }

        #endregion

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
                foreach (AudioClip audioClip in latestHandle.Result)
                {
                    loadedClips.Remove(audioClip);
                }

                Addressables.Release(latestHandle);
                Debug.Log("Released audio clips");
            }
        }

        #endregion
    }
}
