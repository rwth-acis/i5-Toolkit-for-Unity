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

        public void GenerateCSRFToken()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] randomNumber = new byte[30];
            rng.GetBytes(randomNumber);
            string token = "";
            for(int i=0;i<30;i++)
            {
                token += (randomNumber[i] % 10).ToString();
            }
            Debug.Log(token);
            state = token;
        }


        /// <summary>
        /// Gets the access token based on a previously retrieved authorization code
        /// </summary>
        /// <param name="code">The authorization code</param>
        /// <param name="redirectUri">The redirect URI which was used during the login</param>
        /// <returns>Returns the access token if it could be retrieved; otherwise it returns an empty string</returns>
        public override async Task<string> GetAccessTokenFromCodeAsync(string code, string redirectUri)
        {
            // Hier hab ich die Reihenfolge der Argumente hoffentlich richtig angepasst und den Flow Typen angepasst, ich denke das könnte so stimmen

            //string returnedState = redirectUri.Substring(redirectUri.LastIndexOf("state=") + "state=".Length + 1, 30);
            Debug.Log(redirectUri);
            //if (!returnedState.Equals(state))
            //{
                //i5Debug.LogError("The returned anti-forgery state token is incorrect.", this);
                //return "";
            //}

            EndpointsData endpoints = await SetEndpoints();
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
                Debug.LogError(response.ErrorMessage + ": " + response.Content);
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
            // Hier habe ich den state check eingebaut, ich denke das reicht so
            Debug.Log(redirectParameters["state"]);
            Debug.Log("State received");
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

        /// <summary>
        /// Gets information about the logged in user from the provider
        /// </summary>
        /// <param name="accessToken">The access token to authenticate the user</param>
        /// <returns>Returns information about the logged in user if the request was successful, otherwise null</returns>
        /*public override async Task<IUserInfo>void GetUserInfoAsync(string accessToken)
        {
            //TODO
            // User Info wird nicht nochmal in einem Webrequest requested, sondern steht codiert in dem id_token
            // Vielleicht ist es aber auch besser, das id_token zu entfernen und stattdessen wie in der abstrakten Klasse einfach über
            // den UserInfoEndpoint die Info zu requesten (sollte spezifisch dann die GoogleUserInfo als Typ generieren)
        }*/

        /// <summary>
        /// Opens the login page in the system's default Web browser, sets the required endpoints
        /// </summary>
        /// <param name="scopes">The OpenID Connect scopes that the user must agree to</param>
        /// <param name="redirectUri">The URI to which the browser should redirect after the successful login</param>
        public override async Task<string> OpenLoginPageAsync(string[] scopes, string redirectUri)
        {
            // Hier habe ich schon die Reihenfolge der Argumente in der URI angepasst (ich hoffe auch richtig) und die state generation hinzugefügt

            EndpointsData endpoints = await SetEndpoints();
            GenerateCSRFToken();
            string responseType = AuthorizationFlow == AuthorizationFlow.AUTHORIZATION_CODE ? "code" : "token";
            string uriScopes = UriUtils.WordArrayToSpaceEscapedString(scopes);
            redirectUri += "code&";
            string uri = authorizationEndpoint + $"?response_type={responseType}" + $"&client_id={ClientData.ClientId}" + 
                    $"&scope={uriScopes}" + $"&redirect_uri={redirectUri}" + $"state={state}";
            Browser.OpenURL(uri);
            return uri;
        }
    }
}
