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
            GameUIService.OnNarrationVOClick += PlayNarrationVoiceOver;
        }

        public virtual void ExitScene()
        {
            GameUIService.OnNarrationVOClick -= PlayNarrationVoiceOver;
        }

        public virtual void ExitToPrevScene()
        {
            GameUIService.OnNarrationVOClick -= PlayNarrationVoiceOver;
        }

        public void PlayNarrationVoiceOver()
        {
            if (isNarrationBoxActive)
            {
                string narrationClipName = $"nt_{scenarioNo}_{sceneData.SceneNo}";
                SoundManagerService.Instance.OnPlayVoiceOver?.Invoke(narrationClipName);
            }
        }
    }
}