using MGEIP.Service;
using UnityEngine;

namespace MGEIP.Scenario.Scenes
{
    public class StartScene : Scene
    {
        [SerializeField] private int scenarioNo;
        [SerializeField] private Scenario scenario;
        [SerializeField] private GameService gameService;

        [SerializeField] private string scenarioName;

        public override void ExitScene()
        {
            throw new System.NotImplementedException();
        }

        public override void EnterScene()
        {
            throw new System.NotImplementedException();
        }

        public void InitializeStartScene(int scenarioNo, Scenario scenario, GameService gameService)
        {
            this.scenarioNo = scenarioNo;
            this.scenario = scenario;
            this.gameService = gameService;
        }

        public void SetStartSceneInfo(string scenarioName)
        {
            this.scenarioName = scenarioName;
        }
    }
}