using UnityEngine;

namespace i5.Toolkit.Core.AppConsole
{
    public abstract class ConsoleUIBehaviour : MonoBehaviour
    {
        [SerializeField] protected bool captureInBackground;

        protected ConsoleUI consoleUI;

        protected virtual void Awake()
        {
            consoleUI.CaptureInBackground = captureInBackground;
        }

        protected virtual void OnEnable()
        {
            consoleUI.OnEnable();
        }

        protected virtual void OnDisable()
        {
            consoleUI.OnDisable();
        }
    }

}