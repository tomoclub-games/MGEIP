using UnityEngine;

namespace MGEIP.Service
{
    public class MouseCursorService : MonoBehaviour
    {
        public static MouseCursorService Instance;

        [SerializeField] private Texture2D pressedState;

        [SerializeField] private Vector2 hotspot = Vector2.zero;
        [SerializeField] private CursorMode cursorMode = CursorMode.Auto;

        private void Awake()
        {
            if (Instance == null)
            {
                DontDestroyOnLoad(gameObject);
                Instance = this;
            }
        }

        private void Start()
        {
            Cursor.SetCursor(null, hotspot, cursorMode);
        }

        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                Cursor.SetCursor(pressedState, hotspot, cursorMode);
            }

            if(Input.GetMouseButtonUp(0))
            {
                Cursor.SetCursor(null, hotspot, cursorMode);
            }
        }
    }
}