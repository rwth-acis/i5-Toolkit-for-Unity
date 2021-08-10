using System;
using UnityEngine;

namespace i5.Toolkit.Core.Experimental.UnityAdapters
{
    /// <summary>
    /// Adapter for Unity's Application
    /// </summary>
    public class ApplicationAdapter : IApplication
    {
        /// <summary>
        /// Connects to Application.absoluteURL
        /// </summary>
        public string AbsoluteURL => Application.absoluteURL;

        /// <summary>
        /// Connects to Application.deepLinkActivated
        /// </summary>
        public event EventHandler<string> DeepLinkActivated;

        /// <summary>
        /// Creates a new instance of the ApplicationWrapper
        /// </summary>
        public ApplicationAdapter()
        {
            Application.deepLinkActivated += OnDeepLinkActivated;
        }

        /// <summary>
        /// Called if the <see cref="Application.deepLinkActivated"/> event was raised
        /// </summary>
        /// <param name="obj"></param>
        private void OnDeepLinkActivated(string obj)
        {
            DeepLinkActivated?.Invoke(null, obj);
        }
    }
}
