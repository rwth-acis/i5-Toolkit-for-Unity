using UnityEngine;

namespace i5.Toolkit.Core.AppConsole
{
    /// <summary>
    /// MonoBehaviour to connect console UI to Unity
    /// </summary>
    public abstract class ConsoleUIBehaviour : MonoBehaviour
    {
        [Tooltip("If set to true, the console captures messages, even if the GameObject is deactivated")]
        [SerializeField] protected bool captureInBackground;

        protected ConsoleUI consoleUI;

        // initializes the background capture
        protected virtual void Awake()
        {
            consoleUI.CaptureInBackground = captureInBackground;
        }

        // forwards the OnEnable event to the console UI
        protected virtual void OnEnable()
        {
            consoleUI.OnEnable();
        }

        // forwards the OnDisable event to the console UI
        protected virtual void OnDisable()
        {
            consoleUI.OnDisable();
        }
    }

}