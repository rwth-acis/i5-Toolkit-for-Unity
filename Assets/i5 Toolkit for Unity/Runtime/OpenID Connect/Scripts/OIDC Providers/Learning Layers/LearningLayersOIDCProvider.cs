using i5.Toolkit.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    /// <summary>
    /// Implementation of the OpenID Connect Learning Layers Provider
    /// More information can be found here: https://auth.las2peer.org/auth/
    /// </summary>
    public class LearningLayersOidcProvider : AbstractOidcProvider
    {
        /// <summary>
        /// Creates a new instance of the Learning Layers client
        /// </summary>
        public LearningLayersOidcProvider() : base()
        {
            serverName = "https://auth.las2peer.org/auth/realms/main";
        }
    }
}