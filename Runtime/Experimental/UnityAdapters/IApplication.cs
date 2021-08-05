using System;

namespace i5.Toolkit.Core.Experimental.UnityAdapters
{
    /// <summary>
    /// Interface for the application run-time data
    /// </summary>
    public interface IApplication
    {
        /// <summary>
        /// Event which is raised if a deep link is received
        /// </summary>
        event EventHandler<string> DeepLinkActivated;
        /// <summary>
        /// The URL of the document. For non-Web-apps: deep link URL which activated the app
        /// </summary>
        string AbsoluteURL { get; }
    }
}
