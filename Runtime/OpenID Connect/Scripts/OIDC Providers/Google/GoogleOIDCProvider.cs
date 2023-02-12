using i5.Toolkit.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Security.Cryptography;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    /// <summary>
    /// Implementation of the OpenID Connect Learning Layers Provider
    /// More information can be found here: https://auth.las2peer.org/auth/
    /// </summary>
    public class GoogleOidcProvider : AbstractOidcProvider
    {
        /// <summary>
        /// The generated valid session token
        /// </summary>
        private string state;

        /// <summary>
        /// Creates a new instance of the Learning Layers client
        /// </summary>
        public GoogleOidcProvider() : base()
        {
            serverName = "https://accounts.google.com";
        }

        /// <summary>
        /// Creates a sequence of 30 random numbers which is used as the session token
        /// </summary>
        public void GenerateCSRFToken()
        {
#if (UNITY_STANDALONE_WIN || UNITY_EDITOR)
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] randomNumber = new byte[30];
            rng.GetBytes(randomNumber);
            string token = "";
            for(int i=0;i<30;i++)
            {
                token += (randomNumber[i] % 10).ToString();
            }
            state = token;

#else
            string token = "";
            for(int i=0;i<30;i++)
            {
                token += (UnityEngine.Random.Range(0f, 9f)).ToString();
            }
            state = token;
#endif
        }

        /// <summary>
        /// Gets the access token based on a previously retrieved authorization code
        /// </summary>
        /// <param name="code">The authorization code</param>
        /// <param name="redirectUri">The redirect URI which was used during the login</param>
        /// <returns>Returns the access token if it could be retrieved; otherwise it returns an empty string</returns>
        public override async Task<string> GetAccessTokenFromCodeAsync(string code, string redirectUri)
        {
            redirectUri += "code?";

            EndpointsData endpoints = await InitializeEndpointsAsync();
            if (ClientData == null)
            {
                i5Debug.LogError("No client data supplied for the OpenID Connect Client.\n" +
                    "Initialize this provider with an OpenID Connect Data file.", this);
                return "";
            }

            WWWForm form = new WWWForm();
            form.AddField("code", code);
            form.AddField("client_id", ClientData.ClientId);
            form.AddField("client_secret", ClientData.ClientSecret);
            form.AddField("redirect_uri", redirectUri);
            form.AddField("grant_type", "authorization_code");

            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                { "Content-Type", "application/x-www-form-urlencoded" }
            };
            WebResponse<string> response = await RestConnector.PostAsync(tokenEndpoint, form.data, headers);
            if (response.Successful)
            {
                GoogleAuthorizationFlowAnswer answer =
                    JsonSerializer.FromJson<GoogleAuthorizationFlowAnswer>(response.Content);
                if (answer == null)
                {
                    i5Debug.LogError("Could not parse access token in code flow answer", this);
                    return "";
                }
                return answer.access_token;
            }
            else
            {
                i5Debug.LogError(response.ErrorMessage + ": " + response.Content, this);
                return "";
            }
        }

        /// <summary>
        /// Extracts the authorization code from parameters of a Web answer
        /// </summary>
        /// <param name="redirectParameters">Parameters of a Web answer as a dictionary</param>
        /// <returns>The authorization code if it could be found, otherwise an empty string is returned</returns>
        public override string GetAuthorizationCode(Dictionary<string, string> redirectParameters)
        {
            if (redirectParameters.ContainsKey("state") && redirectParameters["state"].Equals(state))
            {
                if (redirectParameters.ContainsKey("code"))
                {
                    return redirectParameters["code"];
                }
                else
                {
                    i5Debug.LogError("Redirect parameters did not contain authorization code", this);
                    return "";
                }
            }
            else
            {
                i5Debug.LogError("Invalid state parameter", this);
                return "";
            }
        }

        /// <summary>
        /// Opens the login page in the system's default Web browser, sets the required endpoints
        /// </summary>
        /// <param name="scopes">The OpenID Connect scopes that the user must agree to</param>
        /// <param name="redirectUri">The URI to which the browser should redirect after the successful login</param>
        public override void OpenLoginPage(string[] scopes, string redirectUri)
        {
            GenerateCSRFToken();
            string responseType = AuthorizationFlow == AuthorizationFlow.AUTHORIZATION_CODE ? "code" : "token";
            string uriScopes = UriUtils.WordArrayToSpaceEscapedString(scopes);
            redirectUri += "code?";
            string uri = authorizationEndpoint + $"?client_id={ClientData.ClientId}" + $"&response_type={responseType}" +
                    $"&redirect_uri={redirectUri}" + $"&scope={uriScopes}" + $"&state={state}";
            Browser.OpenURL(uri);
        }
    }
}
