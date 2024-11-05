using MGEIP.GameData.SceneData;
using MGEIP.Service;
using MGIEP;
using UnityEngine;

namespace MGEIP.Scenario.Scenes
{
    public class Scene : MonoBehaviour
    {
        public SceneType SceneType;

        [SerializeField] protected int scenarioNo;
        [SerializeField] protected Scenario scenario;
        [SerializeField] protected GameService gameService;

        [SerializeField] protected bool isNarrationBoxActive;
        [SerializeField] protected string narrationText;

        [SerializeField] protected bool isDialogueBoxActive;
        [SerializeField] protected string dialogue;

        [SerializeField] protected string scenarioName;

        protected GameUIService GameUIService => gameService.GameUIService;
        protected SceneData sceneData;

        public virtual void EnterScene()
        {
            GameUIService.StartSceneNarrationVOButton.Button.onClick.AddListener(() => PlayNarrationVoiceOver(SceneType.StartScene));
            GameUIService.StorySceneNarrationVOButton.Button.onClick.AddListener(() => PlayNarrationVoiceOver(SceneType.StoryScene));
            GameUIService.QuestionSceneNarrationVOButton.Button.onClick.AddListener(() => PlayNarrationVoiceOver(SceneType.AESliderQuestion));
            GameUIService.EndSceneNarrationVOButton.Button.onClick.AddListener(() => PlayNarrationVoiceOver(SceneType.EndScene));
            GameUIService.PhotoCaptureSceneNarrationVOButton.Button.onClick.AddListener(() => PlayNarrationVoiceOver(SceneType.PhotoCapture));

            SoundManagerService.Instance.OnStopVoiceOver?.Invoke();
        }

        public virtual void ExitScene()
        {
            GameUIService.StartSceneNarrationVOButton.Button.onClick.RemoveAllListeners();
            GameUIService.StorySceneNarrationVOButton.Button.onClick.RemoveAllListeners();
            GameUIService.QuestionSceneNarrationVOButton.Button.onClick.RemoveAllListeners();
            GameUIService.EndSceneNarrationVOButton.Button.onClick.RemoveAllListeners();
            GameUIService.PhotoCaptureSceneNarrationVOButton.Button.onClick.RemoveAllListeners();
        }

        public virtual void ExitToPrevScene()
        {
            GameUIService.StartSceneNarrationVOButton.Button.onClick.RemoveAllListeners();
            GameUIService.StorySceneNarrationVOButton.Button.onClick.RemoveAllListeners();
            GameUIService.QuestionSceneNarrationVOButton.Button.onClick.RemoveAllListeners();
            GameUIService.EndSceneNarrationVOButton.Button.onClick.RemoveAllListeners();
            GameUIService.PhotoCaptureSceneNarrationVOButton.Button.onClick.RemoveAllListeners();
        }

        public void PlayNarrationVoiceOver(SceneType _sceneType)
        {
            if (!isNarrationBoxActive)
                return;

            string narrationClipName = $"Scenarios/sc_{scenarioNo}/nt_{scenarioNo}_{sceneData.SceneNo}";

            switch (_sceneType)
            {
                case SceneType.StartScene:
                    GameUIService.StartSceneNarrationVOButton.PlayAudioClip(narrationClipName);
                    break;
                case SceneType.StoryScene:
                    GameUIService.StorySceneNarrationVOButton.PlayAudioClip(narrationClipName);
                    break;
                case SceneType.MCQQuestion:
                    GameUIService.QuestionSceneNarrationVOButton.PlayAudioClip(narrationClipName);
                    break;
                case SceneType.CESliderQuestion:
                    GameUIService.QuestionSceneNarrationVOButton.PlayAudioClip(narrationClipName);
                    break;
                case SceneType.AESliderQuestion:
                    GameUIService.QuestionSceneNarrationVOButton.PlayAudioClip(narrationClipName);
                    break;
                case SceneType.EndScene:
                    GameUIService.EndSceneNarrationVOButton.PlayAudioClip(narrationClipName);
                    break;
                case SceneType.PhotoCapture:
                    GameUIService.PhotoCaptureSceneNarrationVOButton.PlayAudioClip(narrationClipName);
                    break;
            }
        }
    }
}