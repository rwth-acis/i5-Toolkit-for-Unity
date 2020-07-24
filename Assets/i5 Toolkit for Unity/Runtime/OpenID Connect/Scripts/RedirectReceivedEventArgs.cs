using System;
using System.Collections.Generic;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    /// <summary>
    /// Event arguments for the redirect event
    /// </summary>
    public class RedirectReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// The parameters that were given in the redirect
        /// </summary>
        public Dictionary<string, string> RedirectParameters { get; private set; }
        /// <summary>
        /// The URI to which the redirect lead
        /// </summary>
        public string RedirectUri { get; private set; }

        /// <summary>
        /// Creates a new instance of the redirect event arguments
        /// </summary>
        /// <param name="requestParameters">The parameters that were given in the redirect</param>
        /// <param name="redirectUri">The URI to which the redirect lead</param>
        public RedirectReceivedEventArgs(Dictionary<string,string> requestParameters, string redirectUri)
        {
            RedirectParameters = requestParameters;
            RedirectUri = redirectUri;
        }
    }
}
