using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    /// <summary>
    /// The endpoints of a OIDC provider
    /// </summary>
    /// 
    [Serializable]
    public class EndpointsData
    {
        public string authorization_endpoint;
        public string token_endpoint;
        public string userinfo_endpoint;

        public EndpointsData(string authEndpoint, string tokenEndpoint, string userIEndpoint)
        {
            authorization_endpoint = authEndpoint;
            token_endpoint = tokenEndpoint;
            userinfo_endpoint = userIEndpoint;
        }
    }
}
