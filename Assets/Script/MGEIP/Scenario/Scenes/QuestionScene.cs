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

        private Coroutine audioSequenceCoroutine;

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
            Debug.Log("PlayQuestionVO : Requested!");

            if (sceneData.HasCustomVO)
            {
                Debug.Log("PlayQuestionVO : HAS CUSTOM VO");

                string questionClipName_1 = $"qt_{scenarioNo}_{sceneData.SceneNo}_1";
                string questionClipName_2 = $"kw_{scenario.EmotionIndex + 1}_{scenarioNo}";
                string questionClipName_3 = $"qt_{scenarioNo}_{sceneData.SceneNo}_2";

                if (audioSequenceCoroutine != null)
                    StopCoroutine(audioSequenceCoroutine);

                audioSequenceCoroutine = StartCoroutine(PlayClipsSequentially(questionClipName_1, questionClipName_2, questionClipName_3));
            }
            else
            {
                string questionClipName = $"qt_{scenarioNo}_{sceneData.SceneNo}";
                GameUIService.QuestionVOButton.PlayAudioClip(questionClipName);
            }
        }

        private IEnumerator PlayClipsSequentially(params string[] clipNames)
        {
            SoundManagerService.Instance.OnStopVoiceOver += StopSequentialClipsCoroutine;

            foreach (var clipName in clipNames)
            {
                GameUIService.QuestionVOButton.PlayAudioClip(clipName);

                yield return new WaitWhile(() => SoundManagerService.Instance.IsPlayingVoiceOver());
            }
        }

        public void StopSequentialClipsCoroutine()
        {
            if (audioSequenceCoroutine != null)
                StopCoroutine(audioSequenceCoroutine);

            SoundManagerService.Instance.OnStopVoiceOver -= StopSequentialClipsCoroutine;
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