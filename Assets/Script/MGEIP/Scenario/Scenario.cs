using MGEIP.GameData.ScenarioData;
using MGEIP.GameData.SceneData;
using MGEIP.Scenario.Scenes;
using MGEIP.Service;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MGEIP.Scenario
{
    public class Scenario : MonoBehaviour
    {
        [SerializeField] private int scenarioNo;
        [SerializeField] private string scenarioName;
        [SerializeField] private int currentSceneIndex = 0;

        [SerializeField] private RectTransform scenarioAnchor;
        [SerializeField] private GameObject scenarioInfo;
        [SerializeField] private GameObject scenarioIndicator;
        [SerializeField] private Image scenarioIndicatorImage;
        [SerializeField] private TextMeshProUGUI scenarioNameText;
        [SerializeField] private Button scenarioButton;
        [SerializeField] private Button scenarioPlayButton;

        [SerializeField] private GameService gameService;
        [SerializeField] private List<Scene> scenes = new();

        public int ScenarioNo => scenarioNo;
        public GameObject ScenarioInfo => scenarioInfo; 
        public GameObject ScenarioIndicator => scenarioIndicator;
        public Image ScenarioIndicatorImage => scenarioIndicatorImage;
        public Button ScenarioButton => scenarioButton;
        public Button ScenarioPlayButton => scenarioPlayButton;
        public ScenarioArtDataContainer ScenarioArtDataContainer => gameService.GameDataContainer.ScenarioArtDataContainer;
        public SceneDataContainer SceneDataContainer => gameService.GameDataContainer.SceneDataContainer;
        private GameUIService GameUIService => gameService.GameUIService;

        public void SetScenarioInfo(int scenarioNo, string scenarioName, GameService gameService)
        {
            this.scenarioNo = scenarioNo;
            this.scenarioName = scenarioName;
            this.gameService = gameService;

            scenarioInfo.SetActive(false);
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
        }

        public void CreateScene()
        {
            foreach (SceneData sceneData in SceneDataContainer.SceneContent.Scenes)
            {
                if(sceneData.ScenarioNo == scenarioNo)
                {
                    if (sceneData.SceneType == SceneType.StartScene)
                    {
                        StartScene startScene = Instantiate<StartScene>(gameService.startScenePrefab);
                        startScene.InitializeStartScene(scenarioNo, scenarioName, sceneData, this, gameService);
                        startScene.transform.SetParent(GameUIService.sceneHolder.transform, false);
                        scenes.Add(startScene);
                    }
                    else if (sceneData.SceneType == SceneType.StoryScene)
                    {
                        StoryScene storyScene = Instantiate<StoryScene>(gameService.storyScenePrefab);
                        storyScene.InitializeStoryScene(scenarioNo, sceneData, this, gameService);
                        storyScene.transform.SetParent(GameUIService.sceneHolder.transform, false);
                        scenes.Add(storyScene);
                    }
                    else if (sceneData.SceneType == SceneType.MCQQuestion)
                    {
                        MCQQuestionScene mcqQuestionScene = Instantiate<MCQQuestionScene>(gameService.mcqQuestionScenePrefab);
                        mcqQuestionScene.InitializeQuestionScene(scenarioNo, sceneData, this, gameService);
                        mcqQuestionScene.SetMCQQuestionSceneInfo();
                        mcqQuestionScene.transform.SetParent(GameUIService.sceneHolder.transform, false);
                        scenes.Add(mcqQuestionScene);
                    }
                    else if (sceneData.SceneType == SceneType.CESliderQuestion)
                    {
                        CESliderQuestionScene ceSliderQuestionScene = Instantiate<CESliderQuestionScene>(gameService.ceSliderQuestionScenePrefab);
                        ceSliderQuestionScene.InitializeQuestionScene(scenarioNo, sceneData, this, gameService);
                        ceSliderQuestionScene.SetCESliderQuestionSceneInfo();
                        ceSliderQuestionScene.transform.SetParent(GameUIService.sceneHolder.transform, false);
                        scenes.Add(ceSliderQuestionScene);
                    }
                    else if (sceneData.SceneType == SceneType.AESliderQuestion)
                    {
                        AESliderQuestionScene aeSliderQuestionScene = Instantiate<AESliderQuestionScene>(gameService.aeSliderQuestionScenePrefab);
                        aeSliderQuestionScene.InitializeQuestionScene(scenarioNo, sceneData, this, gameService);
                        aeSliderQuestionScene.SetAESliderQuestionSceneInfo();
                        aeSliderQuestionScene.transform.SetParent(GameUIService.sceneHolder.transform, false);
                        scenes.Add(aeSliderQuestionScene);
                    }
                    else if (sceneData.SceneType == SceneType.EndScene)
                    {
                        EndScene endScene = Instantiate<EndScene>(gameService.endScenePrefab);
                        endScene.InitializeEndScene(scenarioNo, sceneData, this, gameService);
                        endScene.transform.SetParent(GameUIService.sceneHolder.transform, false);
                        scenes.Add(endScene);
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
                scenarioIndicatorImage.sprite = GameUIService.tickSprite;
                scenarioIndicator.SetActive(true);
                scenarioInfo.SetActive(false);
                scenarioButton.enabled = false;
                GameUIService.MapUI.SetActive(true);
            }
            else
            {
                SetSceneStatus(currentSceneIndex + 1);
            }
        }

        public void SetSceneStatus(int index)
        {
            currentSceneIndex = index;
            SetUIForScene(scenes[currentSceneIndex]);
            scenes[currentSceneIndex].EnterScene();
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
            scene.GetComponent<StoryScene>().SetStorySceneButtons(storySceneNextButton);
        }

        private void SetUIForQuestion(Scene scene)
        {
            Button QuestionConfirmButton = GameUIService.QuestionSceneConfirmButton;
            
            QuestionConfirmButton.onClick.RemoveAllListeners();
            scene.GetComponent<QuestionScene>().SetQuestionButton(QuestionConfirmButton);
        }

        private void SetUIForEndScene(Scene scene)
        {
            Button endSceneEndButton = GameUIService.EndSceneEndButton;

            endSceneEndButton.onClick.RemoveAllListeners();
            scene.GetComponent<EndScene>().SetEndSceneButtons(endSceneEndButton);
        }
    }
}