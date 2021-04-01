using System;
using UnityEngine;

namespace i5.Toolkit.Core.Utilities.UnityAdapters
{
    public class ApplicationWrapper : IApplication
    {
        /// <summary>
        /// Connects to Application.absoluteURL
        /// </summary>
        public string AbsoluteURL => Application.absoluteURL;

        /// <summary>
        /// Connects to Application.deepLinkActivated
        /// </summary>
        public event Action<string> DeepLinkActivated
        {
            add
            {
                Application.deepLinkActivated += value;
            }
            remove
            {
                Application.deepLinkActivated -= value;
            }
        }
    }
}
