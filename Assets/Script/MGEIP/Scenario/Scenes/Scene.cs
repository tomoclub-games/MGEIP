using MGEIP.GameData.SceneData;
using UnityEngine;

namespace MGEIP.Scenario.Scenes
{
    public class Scene : MonoBehaviour
    {
        public SceneType SceneType;
        public virtual void EnterScene() { }
        public virtual void ExitScene() { }
    }
}