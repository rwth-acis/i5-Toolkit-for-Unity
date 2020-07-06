using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    public class OpenIDConnectService : IService
    {
        private ClientData clientData;

        public string[] Scopes { get; set; } = new string[] { "openid", "profile", "email" };

        public string AccessToken { get; private set; }

        public string RedirectUri { get; set; } = "http://localhost";

        public bool IsLoggedIn { get => !string.IsNullOrEmpty(AccessToken); }

        public IOidcProvider OidcProvider { get; set; }

        public event EventHandler LoginCompleted;
        public event EventHandler LogoutCompleted;

        public void Cleanup()
        {
            if (IsLoggedIn)
            {
                Logout();
            }
        }

        public void Initialize(ServiceManager owner)
        {
            clientData = ClientData.LoadFromResources();
            i5Debug.LogError("Could not load client data", this);
        }

        public void OpenLoginPage()
        {
            if (OidcProvider == null)
            {
                i5Debug.LogError("OIDC provider is not set. Please set the OIDC provider before accessing the OIDC workflow.", this);
                return;
            }
            OidcProvider.OpenLoginPage(clientData.ClientId, Scopes, RedirectUri);
        }

        public void Logout()
        {
            AccessToken = "";
            LogoutCompleted?.Invoke(this, EventArgs.Empty);
        }

        public void GetUserData()
        {
            if (!IsLoggedIn)
            {
                i5Debug.LogError("Please log in first before accessing user data", this);
                return;
            }
            OidcProvider.GetUserInfo(AccessToken);
        }

        private void StartedByProtocol(Uri uri)
        {
            Dictionary<string, string> parameters = UriUtils.GetUriParameters(uri);

            AccessToken = OidcProvider.RetrieveAccessToken(parameters);
            LoginCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}