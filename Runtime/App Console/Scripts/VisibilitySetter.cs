using UnityEngine;

namespace i5.Toolkit.Core.AppConsole
{
    /// <summary>
    /// Binding component for handling user input and delegating it to the visibility manager
    /// </summary>
    public class VisibilitySetter : MonoBehaviour
    {
        public KeyCode activationKey = KeyCode.F5;

        public VisibilityManager consoleVisibilityManager;

        private void Update()
        {
            if (Input.GetKeyDown(activationKey))
            {
                consoleVisibilityManager.IsVisible = !consoleVisibilityManager.IsVisible;
            }
        }
    }
}