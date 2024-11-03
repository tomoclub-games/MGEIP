﻿using DG.Tweening;
using MGEIP.GameData.ScenarioData;
using MGEIP.GameData.SceneData;
using MGEIP.Scenario.Scenes;
using MGEIP.Service;
using MGIEP;
using MGIEP.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MGEIP.Scenario
{
    public class Scenario : MonoBehaviour
    {
        [SerializeField] private int scenarioNo;
        [SerializeField] private string scenarioName;
        [SerializeField] private int currentSceneIndex = 0;

        /*
                [SerializeField] private RectTransform scenarioAnchor;
                [SerializeField] private GameObject scenarioData;
                [SerializeField] private GameObject scenarioIndicator;
                [SerializeField] private Image scenarioIndicatorImage;
                [SerializeField] private TextMeshProUGUI scenarioNameText;
                [SerializeField] private Button scenarioButton;
                [SerializeField] private Button scenarioPlayButton;
        */
        [SerializeField] private GameService gameService;
        [SerializeField] private List<Scene> scenes = new();

        private ScenarioManager scenarioManager;
        private ScenarioData scenarioData;

        private DateTime scenarioStartTime;

        private string emotionKeyword;
        private int emotionIndex = -1;

        public bool isScenarioCompleted = false;

        public int ScenarioNo => scenarioNo;
        public string EmotionKeyword => emotionKeyword;
        public int EmotionIndex => emotionIndex;
        /*
        public GameObject ScenarioData => scenarioData; 
        public GameObject ScenarioIndicator => scenarioIndicator;
        public Image ScenarioIndicatorImage => scenarioIndicatorImage;
        public Button ScenarioButton => scenarioButton;
        public Button ScenarioPlayButton => scenarioPlayButton;
        */
        public ScenarioArtDataContainer ScenarioArtDataContainer => gameService.GameDataContainer.ScenarioArtDataContainer;
        public ResourceContainer ScenarioResourceContainer => gameService.GameDataContainer.ScenarioResourceContainer;

        public SceneDataContainer SceneDataContainer => gameService.GameDataContainer.SceneDataContainer;
        private GameUIService GameUIService => gameService.GameUIService;

        public void SetScenarioInfo(int scenarioNo, string scenarioName, GameService gameService, ScenarioManager scenarioManager)
        {
            this.scenarioNo = scenarioNo;
            this.scenarioName = scenarioName;
            this.gameService = gameService;
            this.scenarioManager = scenarioManager;

            scenarioData = new ScenarioData(scenarioNo, scenarioName);

            /*
            scenarioData.SetActive(false);
            scenarioIndicator.SetActive(true);
            scenarioNameText.SetText(scenarioName);

            foreach(ScenarioArt scenarioArt in ScenarioArtDataContainer.ScenarioArt)
            {
                if(scenarioArt.scenarioNo == scenarioNo)
                {
                    scenarioAnchor.anchorMin = scenarioArt.minAnchorValue;
                    scenarioAnchor.anchorMax = scenarioArt.maxAnchorValue;
                    scenarioButton.image.sprite = scenarioArt.scenarioIconSprite;
                }
            }
            */
        }

        public void CreateScene()
        {
            SoundManagerService.Instance.OnStopMusic?.Invoke();

            scenarioStartTime = DateTime.UtcNow;

            List<SceneData> sceneDataList = new List<SceneData>();
            List<ScenePrefab> scenePrefabList = new List<ScenePrefab>();

            foreach (SceneData sceneData in SceneDataContainer.SceneContent.Scenes)
            {
                if (sceneData.ScenarioNo == scenarioNo)
                    sceneDataList.Add(sceneData);
            }

            scenePrefabList = ScenarioResourceContainer.SceneContainer[scenarioNo - 1].ScenePrefab;

            foreach (SceneData sceneData in sceneDataList)
            {
                foreach (ScenePrefab scenePrefab in scenePrefabList)
                {
                    if (sceneData.SceneNo == scenePrefab.sceneNo)
                    {
                        Scene scene = Instantiate(scenePrefab.scene);
                        scene.gameObject.SetActive(false);
                        if (scene.SceneType == SceneType.StartScene)
                        {
                            StartScene startScene = scene.GetComponent<StartScene>();
                            startScene.InitializeStartScene(ScenarioNo, scenarioName, sceneData, this, gameService);
                            startScene.transform.SetParent(GameUIService.sceneHolder.transform, false);
                            scenes.Add(startScene);
                        }
                        else if (scene.SceneType == SceneType.StoryScene)
                        {
                            StoryScene storyScene = scene.GetComponent<StoryScene>();
                            storyScene.InitializeStoryScene(scenarioNo, sceneData, this, gameService);
                            storyScene.transform.SetParent(GameUIService.sceneHolder.transform, false);
                            scenes.Add(storyScene);
                        }
                        else if (sceneData.SceneType == SceneType.MCQQuestion)
                        {
                            MCQQuestionScene mcqQuestionScene = scene.GetComponent<MCQQuestionScene>();
                            mcqQuestionScene.InitializeQuestionScene(scenarioNo, sceneData, this, gameService, scenarioData);
                            mcqQuestionScene.SetMCQQuestionSceneInfo();
                            mcqQuestionScene.transform.SetParent(GameUIService.sceneHolder.transform, false);
                            scenes.Add(mcqQuestionScene);
                        }
                        else if (sceneData.SceneType == SceneType.CESliderQuestion)
                        {
                            CESliderQuestionScene ceSliderQuestionScene = scene.GetComponent<CESliderQuestionScene>();
                            ceSliderQuestionScene.InitializeQuestionScene(scenarioNo, sceneData, this, gameService, scenarioData);
                            ceSliderQuestionScene.SetCESliderQuestionSceneInfo();
                            ceSliderQuestionScene.transform.SetParent(GameUIService.sceneHolder.transform, false);
                            scenes.Add(ceSliderQuestionScene);
                        }
                        else if (sceneData.SceneType == SceneType.AESliderQuestion)
                        {
                            AESliderQuestionScene aeSliderQuestionScene = scene.GetComponent<AESliderQuestionScene>();
                            aeSliderQuestionScene.InitializeQuestionScene(scenarioNo, sceneData, this, gameService, scenarioData);
                            aeSliderQuestionScene.SetAESliderQuestionSceneInfo();
                            aeSliderQuestionScene.transform.SetParent(GameUIService.sceneHolder.transform, false);
                            scenes.Add(aeSliderQuestionScene);
                        }
                        else if (sceneData.SceneType == SceneType.PhotoCapture)
                        {
                            PhotoCaptureScene photoCaptureScene = scene.GetComponent<PhotoCaptureScene>();
                            photoCaptureScene.InitializePhotoCaptureScene(scenarioNo, sceneData, this, gameService);
                            photoCaptureScene.transform.SetParent(GameUIService.sceneHolder.transform, false);
                            scenes.Add(photoCaptureScene);
                        }
                        else if (sceneData.SceneType == SceneType.EndScene)
                        {
                            EndScene endScene = scene.GetComponent<EndScene>();
                            endScene.InitializeEndScene(scenarioNo, sceneData, this, gameService);
                            endScene.transform.SetParent(GameUIService.sceneHolder.transform, false);
                            scenes.Add(endScene);
                        }

                    }
                }
            }
        }

        public void IncreamentCurrentScene()
        {
            if (currentSceneIndex + 1 == scenes.Count)
            {
                for (int i = 0; i < scenes.Count; i++)
                {
                    GameObject.Destroy(scenes[i].gameObject);
                }
                scenes.Clear();

                // SoundManagerService.Instance.ReleaseAudio();

                SetScenarioComplete();

                // Close any open scenario infos
                GameUIService.ScenarioInfoCloseButtonClick();

                GameUIService.MapUI.SetActive(true);

                SoundManagerService.Instance.OnStopVoiceOver?.Invoke();
                SoundManagerService.Instance.OnPlayMusic?.Invoke("BGmusic");
            }
            else
            {
                /*SpriteRenderer spriteRenderer = scenes[currentSceneIndex].gameObject.GetComponent<SpriteRenderer>();
                spriteRenderer.DOFade(0, 2f).OnComplete(() =>
                {
                    scenes[currentSceneIndex].gameObject.SetActive(false);
                    SetSceneStatus(currentSceneIndex + 1);
                });*/

                scenes[currentSceneIndex].gameObject.SetActive(false);
                SetSceneStatus(currentSceneIndex + 1);
            }
        }

        public void DecrementCurrentScene()
        {
            if (currentSceneIndex == 0)
            {
                return;
            }
            else
            {
                /*SpriteRenderer spriteRenderer = scenes[currentSceneIndex].gameObject.GetComponent<SpriteRenderer>();
                spriteRenderer.DOFade(0, 0.5f).OnComplete(() =>
                {
                    scenes[currentSceneIndex].gameObject.SetActive(false);
                    SetSceneStatus(currentSceneIndex - 1);
                });*/

                scenes[currentSceneIndex].gameObject.SetActive(false);
                SetSceneStatus(currentSceneIndex - 1);
            }
        }

        public void SetSceneStatus(int index)
        {
            currentSceneIndex = index;
            SetUIForScene(scenes[currentSceneIndex]);
            scenes[currentSceneIndex].gameObject.SetActive(true);
            scenes[currentSceneIndex].EnterScene();

            /*SpriteRenderer spriteRenderer = scenes[currentSceneIndex].gameObject.GetComponent<SpriteRenderer>();
            // Make sure the scene is active before starting the fade.
            scenes[currentSceneIndex].gameObject.SetActive(true);

            // Set the initial alpha of the SpriteRenderer to 0
            Color spriteColor = spriteRenderer.color;
            spriteColor.a = 0;
            spriteRenderer.color = spriteColor;

            // Use DOFade to fade in the sprite
            spriteRenderer.DOFade(1, 2f).OnComplete(() =>
            {
                // Call the EnterScene method after the fade completes
                scenes[currentSceneIndex].EnterScene();
            });*/
        }

        public void SetScenarioComplete()
        {
            isScenarioCompleted = true;

            TimeSpan scenarioTimeSpan = DateTime.UtcNow - scenarioStartTime;
            scenarioData.scenarioDuration = scenarioTimeSpan.TotalSeconds;

            scenarioManager.SetScenarioComplete(ScenarioNo);

            DataHandler.Instance.AddCompletedScenario(scenarioData);

            DataHandler.Instance.OnDataUploaded += DataSentResult;
        }

        private void DataSentResult(bool dataSent)
        {
            if (dataSent)
                GameUIService.AnimateSaveToast();
            else
                GameUIService.AnimateSaveErrorToast();

            DataHandler.Instance.OnDataUploaded -= DataSentResult;
        }

        private void SetUIForScene(Scene scene)
        {
            if (scene.GetComponent<StartScene>() != null)
            {
                SetUIForStartScene(scene);

            }
            else if (scene.GetComponent<StoryScene>() != null)
            {
                SetUIForStoryScene(scene);
            }
            else if (scene.GetComponent<QuestionScene>() != null)
            {
                SetUIForQuestion(scene);
            }
            else if (scene.GetComponent<PhotoCaptureScene>() != null)
            {
                SetUIForPhotoCaptureScene(scene);
            }
            else if (scene.GetComponent<EndScene>() != null)
            {
                SetUIForEndScene(scene);
            }
        }

        private void SetUIForStartScene(Scene scene)
        {
            Button startScenePlayButton = GameUIService.StartScenePlayButton;

            startScenePlayButton.onClick.RemoveAllListeners();
            scene.GetComponent<StartScene>().SetStartSceneButtons(startScenePlayButton);
        }

        private void SetUIForStoryScene(Scene scene)
        {
            Button storySceneNextButton = GameUIService.StorySceneNextButton;

            storySceneNextButton.onClick.RemoveAllListeners();
            GameUIService.StoryScenePrevButton.onClick.RemoveAllListeners();
            scene.GetComponent<StoryScene>().SetStorySceneButtons(storySceneNextButton, GameUIService.StoryScenePrevButton);
        }

        private void SetUIForQuestion(Scene scene)
        {
            scene.GetComponent<QuestionScene>().SetQuestionButton();
        }

        private void SetUIForPhotoCaptureScene(Scene scene)
        {
            GameUIService.PhotoCaptureSceneNextButton.onClick.RemoveAllListeners();
            GameUIService.PhotoCaptureScenePrevButton.onClick.RemoveAllListeners();
            scene.GetComponent<PhotoCaptureScene>().SetPhotoCaptureSceneButtons(GameUIService.PhotoCaptureSceneNextButton, GameUIService.PhotoCaptureScenePrevButton);
        }

        private void SetUIForEndScene(Scene scene)
        {
            Button endSceneEndButton = GameUIService.EndSceneEndButton;

            endSceneEndButton.onClick.RemoveAllListeners();
            GameUIService.EndScenePrevButton.onClick.RemoveAllListeners();
            scene.GetComponent<EndScene>().SetEndSceneButtons(endSceneEndButton, GameUIService.EndScenePrevButton);
        }

        public void SetEmotionKeyword(int _index, string _keyword)
        {
            emotionIndex = _index;
            emotionKeyword = _keyword;
        }
    }
}