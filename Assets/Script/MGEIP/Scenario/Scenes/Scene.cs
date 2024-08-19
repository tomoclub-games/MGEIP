using UnityEngine;

namespace MGEIP.Scenario.Scenes
{
    public abstract class Scene : MonoBehaviour
    {
        public abstract void EnterScene();
        public abstract void ExitScene();
    }
}