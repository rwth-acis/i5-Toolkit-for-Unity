using i5.Toolkit.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    /// <summary>
    /// Implementation of the OpenID Connect GitHub Provider
    /// </summary>
    public class GitHubOidcProvider : AbstractOidcProvider
    {
        /// <summary>
        /// Creates a new instance of the GitHub client
        /// </summary>
        public GitHubOidcProvider() : base()
        {
            serverName = "https://github.com/";
            authorizationEndpoint = "https://github.com/login/oauth/authorize";
            tokenEndpoint = "https://github.com/login/oauth/access_token";
            userInfoEndpoint = "https://api.github.com/user";
            RestConnector = new JsonEncodeUnityWebRequestRestConnector();
        }


        /// <summary>
        /// Gets the access token based on a previously retrieved authorization code
        /// </summary>
        /// <param name="code">The authorization code</param>
        /// <param name="redirectUri">The redirect URI which was used during the login</param>
        /// <returns>Returns the access token if it could be retrieved; otherwise it returns an empty string</returns>
        public override async Task<string> GetAccessTokenFromCodeAsync(string code, string redirectUri)
        {
            if (ClientData == null)
            {
                i5Debug.LogError("No client data supplied for the OpenID Connect Client.\n" +
                    "Initialize this provider with an OpenID Connect Data file.", this);
                return "";
            }

            string uri = tokenEndpoint + $"?client_id={ClientData.ClientId}" +
                $"&redirect_uri={redirectUri}" + $"&client_secret={ClientData.ClientSecret}&code={code}&grant_type=authorization_code";
            WebResponse<string> response = await RestConnector.PostAsync(uri, "");

            if (response.Successful)
            {
                string response_content = response.Content;
                GitHubAuthorizationFlowAnswer answer =
                    JsonSerializer.FromJson<GitHubAuthorizationFlowAnswer>(response_content);
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
        /// Gets information about the logged in user from the GitHub provider
        /// </summary>
        /// <param name="accessToken">The access token to authenticate the user</param>
        /// <returns>Returns information about the logged in user if the request was successful, otherwise null</returns>
        public override async Task<IUserInfo> GetUserInfoAsync(string accessToken)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", $"token {accessToken}");
            WebResponse<string> webResponse = await RestConnector.GetAsync(userInfoEndpoint + "?access_token=" + accessToken, headers);
            if (webResponse.Successful)
            {
                GitHubUserInfo userInfo = JsonSerializer.FromJson<GitHubUserInfo>(webResponse.Content);
                if (userInfo == null)
                {
                    i5Debug.LogError("Could not parse user info", this);
                }
                return userInfo;
            }
            else
            {
                i5Debug.LogError("Error fetching the user info: " + webResponse.ErrorMessage, this);
                return default;
            }
        }

        /// <summary>
        /// Opens the GitHUb login page in the system's default Web browser
        /// </summary>
        /// <param name="scopes">The OpenID Connect scopes that the user must agree to</param>
        /// <param name="redirectUri">The URI to which the browser should redirect after the successful login</param>
        public override void OpenLoginPage(string[] scopes, string redirectUri)
        {
            if (ClientData == null)
            {
                i5Debug.LogError("No client data supplied for the OpenID Connect Client.\n" +
                    "Initialize this provider with an OpenID Connect Data file.", this);
                return;
            }

            string responseType = AuthorizationFlow == AuthorizationFlow.AUTHORIZATION_CODE ? "code" : "token";
            string uriScopes = UriUtils.WordArrayToSpaceEscapedString(scopes);
            string uri = authorizationEndpoint + $"?client_id={ClientData.ClientId}&redirect_uri={redirectUri}" + $"&response_type={responseType}&scope={uriScopes}";
            Browser.OpenURL(uri);
        }
    } 
}