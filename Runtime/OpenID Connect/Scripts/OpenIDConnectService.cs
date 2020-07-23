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
    public class OpenIDConnectService : IUpdateableService
    {
        private ClientData clientData;

        public IClientDataLoader ClientDataLoader { get; set; } = new ClientDataResourcesLoader();

        public string[] Scopes { get; set; } = new string[] { "openid", "profile", "email" };

        public string AccessToken { get; private set; }

        public bool IsLoggedIn { get => !string.IsNullOrEmpty(AccessToken); }

        public IOidcProvider OidcProvider { get; set; }

        public IRedirectServerListener ServerListener { get; set; }
        public bool Enabled { get; set; }

        public event EventHandler LoginCompleted;
        public event EventHandler LogoutCompleted;

        private RedirectReceivedEventArgs eventArgs;

        public OpenIDConnectService()
        {
            ServerListener = new RedirectServerListener();
        }

        public OpenIDConnectService(ClientData clientData) : this()
        {
            this.clientData = clientData;
        }

        public async void Initialize(BaseServiceManager owner)
        {
            if (clientData == null)
            {
                clientData = await ClientDataLoader.LoadClientDataAsync();
            }

            if (clientData == null)
            {
                i5Debug.LogError("No client data supplied for the OpenID Connect Client.\n" +
                    "Create a JSON file in the resources or reference a OpenID Connect Data file.", this);
            }
        }

        public void Cleanup()
        {
            if (ServerListener.ServerActive)
            {
                ServerListener.StopServerImmediately();
            }

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

        private void ServerListener_RedirectReceived(object sender, RedirectReceivedEventArgs e)
        {
            eventArgs = e;
            Enabled = true;
        }

        public void Logout()
        {
            AccessToken = "";
            LogoutCompleted?.Invoke(this, EventArgs.Empty);
        }

        public async Task<bool> CheckAccessToken()
        {
            if (!IsLoggedIn)
            {
                i5Debug.LogWarning("Access token not valid because user is not logged in.", this);
                return false;
            }
            IUserInfo userInfo = await GetUserDataAsync();
            return userInfo != null;
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

        public async void Update()
        {
            if (eventArgs == null)
            {
                return;
            }

            // disable immediately again so that we do not execute this part here multiple times
            // as long as the first operation takes to finish
            Enabled = false;

            if (OidcProvider.ParametersContainError(eventArgs.RedirectParameters, out string errorMessage))
            {
                i5Debug.LogError("Error: " + errorMessage, this);
                return;
            }

            if (OidcProvider.AuthorzationFlow == AuthorizationFlow.AUTHORIZATION_CODE)
            {
                string authorizationCode = OidcProvider.GetAuthorizationCode(eventArgs.RedirectParameters);
                AccessToken = await OidcProvider.GetAccessTokenFromCodeAsync(authorizationCode, eventArgs.RedirectUri);
            }
            else
            {
                AccessToken = OidcProvider.GetAccessToken(eventArgs.RedirectParameters);
            }
            eventArgs = null;
            LoginCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}