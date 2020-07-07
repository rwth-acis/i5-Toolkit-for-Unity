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

        public event EventHandler LoginCompleted;
        public event EventHandler LogoutCompleted;

        private string redirectUri;
        private Thread serverThread;
        private bool serverActive = false;
        private HttpListener http;

        public void Cleanup()
        {
            StopServerImmediately();
            if (IsLoggedIn)
            {
                Logout();
            }
        }

        public async void Initialize(ServiceManager owner)
        {
            clientData = await ClientDataLoader.LoadClientDataAsync();
        }

        public void OpenLoginPage()
        {
            if (OidcProvider == null)
            {
                i5Debug.LogError("OIDC provider is not set. Please set the OIDC provider before accessing the OIDC workflow.", this);
                return;
            }

            if (string.IsNullOrEmpty(redirectUri))
            {
                redirectUri = GenerateRedirectUri();
            }

            StartServer();

            OidcProvider.OpenLoginPage(clientData.ClientId, Scopes, redirectUri);
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

        private static string GenerateRedirectUri(string protocol = "http")
        {
            string redirectUri = protocol + "://" + IPAddress.Loopback + ":" + GetUnusedPort() + "/";
            return redirectUri;
        }

        private static int GetUnusedPort()
        {
            TcpListener listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }

        private void StartServer()
        {
            serverThread = new Thread(Listen);
            serverActive = true;
            serverThread.Start();
        }

        private void StopServerImmediately()
        {
            if (serverThread != null)
            {
                serverThread.Abort();
                http.Stop();
                i5Debug.Log("HTTPListener stopped.", this);
            }
        }

        private async void Listen()
        {
            http = new HttpListener();
            if (string.IsNullOrEmpty(redirectUri))
            {
                redirectUri = GenerateRedirectUri();
            }
            http.Prefixes.Add(redirectUri);
            http.Start();
            i5Debug.Log("OIDC Redirect server now listening on address " + redirectUri, this);

            while(serverActive)
            {
                try
                {
                    HttpListenerContext context = http.GetContext();

                    if (context.Request.QueryString.Get("error") != null)
                    {
                        i5Debug.LogError("Error: " + context.Request.QueryString.Get("error"), this);
                        return;
                    }

                    if (OidcProvider.AuthorzationFlow == AuthorizationFlow.AUTHORIZATION_CODE)
                    {
                        AccessToken = await OidcProvider.AccessTokenFromCodeAsync(context.Request.QueryString.ToDictionary());
                    }
                    else
                    {
                        AccessToken = OidcProvider.GetAccessToken(context.Request.QueryString.ToDictionary());
                    }

                    string responseString = string.Format("<html><head></head><body>Please return to the app</body></html>");
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                    context.Response.ContentLength64 = buffer.Length;
                    var responseOutput = context.Response.OutputStream;
                    responseOutput.Write(buffer, 0, buffer.Length);
                    responseOutput.Close();
                    http.Stop();
                    serverActive = false;
                    i5Debug.Log("Server stopped", this);

                    LoginCompleted?.Invoke(this, EventArgs.Empty);
                }
                catch(Exception e)
                {
                    i5Debug.LogError("OIDC server encountered an error: " + e.ToString() + "\nShutting server down.", this);
                }
            }
        }
    }
}