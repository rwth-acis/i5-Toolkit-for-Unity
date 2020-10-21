using UnityEngine;

namespace i5.Toolkit.Core.AppConsole
{

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