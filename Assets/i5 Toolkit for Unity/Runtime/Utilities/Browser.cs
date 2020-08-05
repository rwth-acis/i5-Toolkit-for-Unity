using UnityEngine;

namespace i5.Toolkit.Core.Utilities
{
    /// <summary>
    /// Implementation that opens the system's default browser
    /// </summary>
    public class Browser : IBrowser
    {
        /// <summary>
        /// Opens the provided URL in the system's default browser
        /// </summary>
        /// <param name="url">The url to open</param>
        public void OpenURL(string url)
        {
            Application.OpenURL(url);
        }
    }
}
