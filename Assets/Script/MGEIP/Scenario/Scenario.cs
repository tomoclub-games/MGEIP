using MGEIP.GameData.SceneData;
using MGEIP.Scenario.Scenes;
using MGEIP.Service;
using System.Collections.Generic;
using UnityEngine;

namespace MGEIP.Scenario
{
    public class Scenario : MonoBehaviour
    {
        public int scenarioNo;
        public string scenarioName;

        public List<Scene> scenes = new();

        public SceneDataContainer SceneDataContainer => gameService.GameDataContainer.SceneDataContainer;

        private GameService gameService;

        public void SetScenarioInfo(int scenarioNo, string scenarioName, GameService gameService)
        {
            this.scenarioNo = scenarioNo;
            this.scenarioName = scenarioName;
            this.gameService = gameService;

            CreateScene();
        }

        private void CreateScene()
        {
            foreach (SceneData sceneData in SceneDataContainer.SceneContent.Scenes)
            {
                if(sceneData.ScenarioNo == scenarioNo)
                {
                    if (sceneData.SceneType == SceneType.StartScene)
                    {
                        StartScene startScene = Instantiate<StartScene>(gameService.startScenePrefab);
                        startScene.InitializeStartScene(scenarioNo, this, gameService);
                        startScene.SetStartSceneInfo(scenarioName);
                        startScene.transform.SetParent(this.transform, false);
                        scenes.Add(startScene);
                    }
                    else if (sceneData.SceneType == SceneType.StoryScene)
                    {
                        StoryScene storyScene = Instantiate<StoryScene>(gameService.storyScenePrefab);
                        storyScene.InitializeStoryScene(scenarioNo, this, gameService);
                        storyScene.SetStorySceneInfo(sceneData.DialogueBox, sceneData.NarrationBox, sceneData.DialogueText, sceneData.NarrationText);
                        storyScene.transform.SetParent(this.transform, false);
                        scenes.Add(storyScene);
                    }
                    else if (sceneData.SceneType == SceneType.MCQQuestion)
                    {
                        MCQQuestionScene mcqQuestionScene = Instantiate<MCQQuestionScene>(gameService.mcqQuestionScenePrefab);
                        mcqQuestionScene.InitializeMCQQuestionScene(scenarioNo, this, gameService);
                        mcqQuestionScene.SetMCQQuestionSceneInfo(sceneData.DialogueBox, sceneData.DialogueText, sceneData.QuestionText, sceneData.Option1, sceneData.Option2, sceneData.Option3, sceneData.Option4);
                        mcqQuestionScene.transform.SetParent(this.transform, false);
                        scenes.Add(mcqQuestionScene);
                    }
                    else if (sceneData.SceneType == SceneType.CESliderQuestion)
                    {
                        CESliderQuestionScene ceSliderQuestionScene = Instantiate<CESliderQuestionScene>(gameService.ceSliderQuestionScenePrefab);
                        ceSliderQuestionScene.InitializeCESliderQuestionScene(scenarioNo, this, gameService);
                        ceSliderQuestionScene.SetCESliderQuestionSceneInfo(sceneData.DialogueBox, sceneData.DialogueText, sceneData.QuestionText);
                        ceSliderQuestionScene.transform.SetParent(this.transform, false);
                        scenes.Add(ceSliderQuestionScene);
                    }
                    else if (sceneData.SceneType == SceneType.AESliderQuestion)
                    {
                        AESliderQuestionScene aeSliderQuestionScene = Instantiate<AESliderQuestionScene>(gameService.aeSliderQuestionScenePrefab);
                        aeSliderQuestionScene.InitializeAESliderQuestionScene(scenarioNo, this, gameService);
                        aeSliderQuestionScene.SetAESliderQuestionSceneInfo(sceneData.DialogueBox, sceneData.DialogueText, sceneData.QuestionText);
                        aeSliderQuestionScene.transform.SetParent(this.transform, false);
                        scenes.Add(aeSliderQuestionScene);
                    }
                    else if (sceneData.SceneType == SceneType.EndScene)
                    {
                        EndScene endScene = Instantiate<EndScene>(gameService.endScenePrefab);
                        endScene.InitializeEndScene(scenarioNo, this, gameService);
                        endScene.SetEndSceneInfo(sceneData.DialogueBox, sceneData.NarrationBox, sceneData.DialogueText, sceneData.NarrationText);
                        endScene.transform.SetParent(this.transform, false);
                        scenes.Add(endScene);
                    }
                }
            }
        }
    }
}