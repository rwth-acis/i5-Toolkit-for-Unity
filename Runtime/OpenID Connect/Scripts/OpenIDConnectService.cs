using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    public class OpenIDConnectService : IService
    {
        private ClientData clientData;

        public IClientDataLoader ClientDataLoader { get; set; } = new ClientDataResourcesLoader();

        public string[] Scopes { get; set; } = new string[] { "openid", "profile", "email" };

        public string AccessToken { get; private set; }

        public bool IsLoggedIn { get => !string.IsNullOrEmpty(AccessToken); }

        public IOidcProvider OidcProvider { get; set; }

        public IRedirectServerListener ServerListener { get; set; }

        public event EventHandler LoginCompleted;
        public event EventHandler LogoutCompleted;

        public OpenIDConnectService()
        {
            ServerListener = new RedirectServerListener();
        }

        public async void Initialize(BaseServiceManager owner)
        {
            clientData = await ClientDataLoader.LoadClientDataAsync();
        }

        public void Cleanup()
        {
            ServerListener.StopServerImmediately();
            if (IsLoggedIn)
            {
                Logout();
            }
        }

        public void OpenLoginPage()
        {
            if (OidcProvider == null)
            {
                i5Debug.LogError("OIDC provider is not set. Please set the OIDC provider before accessing the OIDC workflow.", this);
                return;
            }
            if (ServerListener == null)
            {
                i5Debug.LogError("Redirect server listener is not set. Please set it before accessing the OIDC workflow.", this);
                return;
            }

            OidcProvider.ClientData = clientData;

            // TODO: support custom Uri schema
            string redirectUri = ServerListener.GenerateRedirectUri();
            ServerListener.RedirectReceived += ServerListener_RedirectReceived;
            ServerListener.StartServer();

            OidcProvider.OpenLoginPage(Scopes, redirectUri);
        }

        private async void ServerListener_RedirectReceived(object sender, RedirectReceivedEventArgs e)
        {
            if (e.RedirectParameters.ContainsKey("error"))
            {
                i5Debug.LogError("Error: " + e.RedirectParameters["error"], this);
                return;
            }

            if (OidcProvider.AuthorzationFlow == AuthorizationFlow.AUTHORIZATION_CODE)
            {
                string authorizationCode = OidcProvider.GetAuthorizationCode(e.RedirectParameters);
                AccessToken = await OidcProvider.GetAccessTokenFromCodeAsync(authorizationCode, e.RedirectUri);
            }
            else
            {
                AccessToken = OidcProvider.GetAccessToken(e.RedirectParameters);
            }
        }

        public void Logout()
        {
            AccessToken = "";
            LogoutCompleted?.Invoke(this, EventArgs.Empty);
        }

        public async Task<IUserInfo> GetUserDataAsync()
        {
            if (!IsLoggedIn)
            {
                i5Debug.LogError("Please log in first before accessing user data", this);
                return null;
            }
            return await OidcProvider.GetUserInfoAsync(AccessToken);
        }
    }
}