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
        /// The endpoint for the log in
        /// </summary>
        private const string authorizationEndpoint = "https://auth.las2peer.org/auth/realms/main/protocol/openid-connect/auth";
        /// <summary>
        /// The end point where the access token can be requested
        /// </summary>
        private const string tokenEndpoint = "https://auth.las2peer.org/auth/realms/main/protocol/openid-connect/token";
        /// <summary>
        /// The end point where user information can be requested
        /// </summary>
        private const string userInfoEndpoint = "https://auth.las2peer.org/auth/realms/main/protocol/openid-connect/userinfo";

        /// <summary>
        /// Creates a new instance of the Learning Layers client
        /// </summary>
        public LearningLayersOidcProvider() : base() { }
    }
}