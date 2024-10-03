using System.Collections;
using MGEIP.GameData.SceneData;
using MGEIP.Service;
using MGIEP;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MGEIP.Scenario.Scenes
{
    public class QuestionScene : Scene
    {
        [SerializeField] protected string questionText;

        [SerializeField] protected GameObject dialogueBox;
        [SerializeField] protected TextMeshProUGUI dialogueText;
        [SerializeField] private AudioClipButton dialogueVOButton;

        public override void EnterScene()
        {
            base.EnterScene();

            GameUIService.SetQuestionSceneUIActive(true);

            dialogueVOButton.Button.onClick.AddListener(PlayDialogueVoiceOver);

            GameUIService.QuestionVOButton.Button.onClick.AddListener(PlayQuestionVoiceOver);
        }

        public virtual void InitializeQuestionScene(int scenarioNo, SceneData sceneData, Scenario scenario, GameService gameService)
        {
            this.scenarioNo = scenarioNo;
            this.sceneData = sceneData;
            this.scenario = scenario;
            this.gameService = gameService;
        }

        public virtual void SetQuestionButton()
        {
            GameUIService.OnPrevButtonClick += ExitToPrevScene;
        }

        public virtual void CompleteQuestionScene()
        {
            GameUIService.SetQuestionSceneNarrationBoxActive(false);
            GameUIService.SetQuestionSceneUIActive(false);

            GameUIService.OnPrevButtonClick -= ExitToPrevScene;
        }

        public override void ExitScene()
        {
            base.ExitScene();

            dialogueVOButton.Button.onClick.RemoveAllListeners();
            GameUIService.QuestionVOButton.Button.onClick.RemoveAllListeners();


            CompleteQuestionScene();
            scenario.IncreamentCurrentScene();
        }

        public override void ExitToPrevScene()
        {
            base.ExitToPrevScene();

            dialogueVOButton.Button.onClick.RemoveAllListeners();
            GameUIService.QuestionVOButton.Button.onClick.RemoveAllListeners();

            CompleteQuestionScene();
            scenario.DecrementCurrentScene();
        }

        public void PlayQuestionVoiceOver()
        {
            if (sceneData.HasCustomVO)
            {
                string questionClipName_1 = $"qt_{scenarioNo}_{sceneData.SceneNo}_1";
                string questionClipName_2 = $"kw_{scenario.EmotionIndex + 1}_{scenarioNo}";
                string questionClipName_3 = $"qt_{scenarioNo}_{sceneData.SceneNo}_2";

                StartCoroutine(PlayClipsSequentially(questionClipName_1, questionClipName_2, questionClipName_3));
            }
            else
            {
                string questionClipName = $"qt_{scenarioNo}_{sceneData.SceneNo}";
                SoundManagerService.Instance.OnPlayVoiceOver?.Invoke(questionClipName);
            }
        }

        private IEnumerator PlayClipsSequentially(params string[] clipNames)
        {
            foreach (var clipName in clipNames)
            {
                if (SoundManagerService.Instance.IsVoiceOverCancelled)
                    yield break;

                SoundManagerService.Instance.OnPlayVoiceOver?.Invoke(clipName);

                yield return new WaitWhile(() => SoundManagerService.Instance.IsPlayingVoiceOver());
            }
        }

        public void PlayDialogueVoiceOver()
        {
            if (isDialogueBoxActive)
            {
                string dialogueClipName = $"dt_{scenarioNo}_{sceneData.SceneNo}";
                dialogueVOButton.PlayAudioClip(dialogueClipName);
            }
        }
    }
}