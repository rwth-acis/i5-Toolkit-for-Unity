using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    public interface IOidcProvider
    {
        void OpenLoginPage(string clientId, string[] scopes, string redirectUri);

        string RetrieveAccessToken(Dictionary<string, string> arguments);

        bool IsAccessTokenValid(string accessToken);

        IUserInfo GetUserInfo(string accessToken);
    }
}