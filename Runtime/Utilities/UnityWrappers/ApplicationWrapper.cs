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
        public event EventHandler<string> DeepLinkActivated;

        public ApplicationWrapper()
        {
            Application.deepLinkActivated += OnDeepLinkActivated;
        }


        private void OnDeepLinkActivated(string obj)
        {
            DeepLinkActivated?.Invoke(null, obj);
        }
    }
}
