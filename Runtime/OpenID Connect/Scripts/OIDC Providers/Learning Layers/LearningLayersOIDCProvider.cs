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
    public class LearningLayersOidcProvider : IOidcProvider
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
        /// Gets or sets the used authorization flow
        /// </summary>
        public AuthorizationFlow AuthorizationFlow { get; set; }

        /// <summary>
        /// Specifies how the REST API of the Web service is accessed
        /// </summary>
        public IRestConnector RestConnector { get; set; }

        /// <summary>
        /// Client data that are required to authorize the client at the provider
        /// </summary>
        public ClientData ClientData { get; set; }

        /// <summary>
        /// Serializer that is responsible for parsing JSON data and converting to JSON
        /// </summary>
        public IJsonSerializer JsonSerializer { get; set; }

        /// <summary>
        /// The implementation that should accesss the browser
        /// </summary>
        public IBrowser Browser { get; set; }

        /// <summary>
        /// Creates a new instance of the learning layers client
        /// </summary>
        public LearningLayersOidcProvider()
        {
            RestConnector = new UnityWebRequestRestConnector();
            JsonSerializer = new JsonUtilityAdapter();
            Browser = new Browser();
        }

        /// <summary>
        /// Gets the access token based on a previously retrieved authorization code
        /// </summary>
        /// <param name="code">The authorization code</param>
        /// <param name="redirectUri">The redirect URI which was used during the login</param>
        /// <returns>Returns the access token if it could be retrieved; otherwise it returns an empty string</returns>
        public async Task<string> GetAccessTokenFromCodeAsync(string code, string redirectUri)
        {
            if (ClientData == null)
            {
                i5Debug.LogError("No client data supplied for the OpenID Connect Client.\n" +
                    "Initialize this provider with an OpenID Connect Data file.", this);
                return "";
            }

            WWWForm form = new WWWForm();
            form.AddField("client_id", ClientData.ClientId);
            form.AddField("client_secret", ClientData.ClientSecret);
            form.AddField("grant_type", "authorization_code");
            form.AddField("redirect_uri", redirectUri);
            form.AddField("code", code);

            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                { "Content-Type", "application/x-www-form-urlencoded" }
            };
            WebResponse<string> response = await RestConnector.PostAsync(tokenEndpoint, form.data, headers);
            if (response.Successful)
            {
                LearningLayersAuthorizationFlowAnswer answer =
                    JsonSerializer.FromJson<LearningLayersAuthorizationFlowAnswer>(response.Content);
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
        /// Gets the access token from a list of parameters in a Web answer
        /// </summary>
        /// <param name="redirectParameters">The parameters of the Web answer as a dictionary</param>
        /// <returns>Returns the access token if it exists in the parameters,
        /// otherwise an empty string is returned</returns>
        public string GetAccessToken(Dictionary<string, string> redirectParameters)
        {
            if (redirectParameters.ContainsKey("token"))
            {
                return redirectParameters["token"];
            }
            else
            {
                i5Debug.LogError("Redirect parameters did not contain access token", this);
                return "";
            }
        }

        /// <summary>
        /// Gets information about the logged in user from the learning layers provider
        /// </summary>
        /// <param name="accessToken">The access token to authenticate the user</param>
        /// <returns>Returns information about the logged in user if the request was successful, otherwise null</returns>
        public async Task<IUserInfo> GetUserInfoAsync(string accessToken)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                {"Authorization", $"Bearer {accessToken}" }
            };
            WebResponse<string> webResponse = await RestConnector.GetAsync(userInfoEndpoint, headers);
            if (webResponse.Successful)
            {
                LearningLayersUserInfo userInfo = JsonSerializer.FromJson<LearningLayersUserInfo>(webResponse.Content);
                if (userInfo == null)
                {
                    i5Debug.LogError("Could not parse user info", this);
                }
                return userInfo;
            }
            else
            {
                i5Debug.LogError($"Error fetching the user info: {webResponse.ErrorMessage}\n{webResponse.Content}", this);
                return default;
            }
        }

        /// <summary>
        /// Checks if the access token is valid by checking it at the Learning Layers provider
        /// </summary>
        /// <param name="accessToken">The access token that should be checked</param>
        /// <returns>True if the access token is valid, otherwise false</returns>
        public async Task<bool> CheckAccessTokenAsync(string accessToken)
        {
            IUserInfo userInfo = await GetUserInfoAsync(accessToken);
            return userInfo != null;
        }

        /// <summary>
        /// Opens the Learning Layers login page in the system's default Web browser
        /// </summary>
        /// <param name="scopes">The OpenID Connect scopes that the user must agree to</param>
        /// <param name="redirectUri">The URI to which the browser should redirect after the successful login</param>
        public void OpenLoginPage(string[] scopes, string redirectUri)
        {
            if (ClientData == null)
            {
                i5Debug.LogError("No client data supplied for the OpenID Connect Client.\n" +
                    "Initialize this provider with an OpenID Connect Data file.", this);
                return;
            }

            string responseType = AuthorizationFlow == AuthorizationFlow.AUTHORIZATION_CODE ? "code" : "token";
            string uriScopes = UriUtils.WordArrayToSpaceEscapedString(scopes);
            string uri = authorizationEndpoint + $"?response_type={responseType}&scope={uriScopes}" +
                $"&client_id={ClientData.ClientId}&redirect_uri={redirectUri}";
            Browser.OpenURL(uri);
        }

        /// <summary>
        /// Extracts the authorization code from parameters of a Web answer
        /// </summary>
        /// <param name="redirectParameters">Parameters of a Web answer as a dictionary</param>
        /// <returns>The authorization code if it could be found, otherwise an empty string is returned</returns>
        public string GetAuthorizationCode(Dictionary<string, string> redirectParameters)
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

        /// <summary>
        /// Checks if the provider included error messages in the parameters of a Web answer
        /// </summary>
        /// <param name="parameters">The parameters of a Web answer as a dictionary</param>
        /// <param name="errorMessage">The error message that the provider included, empty if no error exists</param>
        /// <returns>Returns true if the parameters contain an error message, otherwise false</returns>
        public bool ParametersContainError(Dictionary<string, string> parameters, out string errorMessage)
        {
            if (parameters.ContainsKey("error"))
            {
                errorMessage = parameters["error"];
                return true;
            }
            errorMessage = "";
            return false;
        }
    }
}