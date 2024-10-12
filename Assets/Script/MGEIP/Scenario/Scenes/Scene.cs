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
            GameUIService.StartSceneNarrationVOButton.Button.onClick.AddListener(PlayNarrationVoiceOver);
            GameUIService.StorySceneNarrationVOButton.Button.onClick.AddListener(PlayNarrationVoiceOver);
            GameUIService.QuestionSceneNarrationVOButton.Button.onClick.AddListener(PlayNarrationVoiceOver);
            GameUIService.EndSceneNarrationVOButton.Button.onClick.AddListener(PlayNarrationVoiceOver);
            GameUIService.PhotoCaptureSceneNarrationVOButton.Button.onClick.AddListener(PlayNarrationVoiceOver);
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

        public void PlayNarrationVoiceOver()
        {
            if (isNarrationBoxActive)
            {
                string narrationClipName = $"Scenarios/sc_{scenarioNo}/nt_{scenarioNo}_{sceneData.SceneNo}";
                GameUIService.StartSceneNarrationVOButton.PlayAudioClip(narrationClipName);
                GameUIService.StorySceneNarrationVOButton.PlayAudioClip(narrationClipName);
                GameUIService.QuestionSceneNarrationVOButton.PlayAudioClip(narrationClipName);
                GameUIService.EndSceneNarrationVOButton.PlayAudioClip(narrationClipName);
                GameUIService.PhotoCaptureSceneNarrationVOButton.PlayAudioClip(narrationClipName);
            }
        }
    }
}