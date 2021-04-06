using System;
using System.Collections.Generic;

namespace i5.Toolkit.Core.DeepLinkAPI
{
    /// <summary>
    /// Arguments with further information about the received deep link
    /// </summary>
    public class DeepLinkArgs
    {
        /// <summary>
        /// The list of parameters that the deep link contained
        /// </summary>
        public Dictionary<string, string> Parameters { get; private set; }
        /// <summary>
        /// The full deep link uri
        /// </summary>
        public Uri DeepLink { get; private set; }
        /// <summary>
        /// The scheme of the deep link, e.g. i5://
        /// </summary>
        public string Scheme => DeepLink.Scheme;

        /// <summary>
        /// Creates a new instance of the deep link arguments
        /// </summary>
        /// <param name="parameters">The parameters of the deep link call</param>
        /// <param name="deepLink">The full deep link</param>
        public DeepLinkArgs(Dictionary<string,string> parameters, Uri deepLink)
        {
            Parameters = parameters;
            DeepLink = deepLink;
        }
    }
}
