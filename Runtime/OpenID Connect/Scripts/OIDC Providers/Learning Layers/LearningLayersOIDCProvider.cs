using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using i5.Toolkit.Core.Utilities;
using i5.Toolkit.Core.Utilities.ContentLoaders;
using UnityEngine;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    public class LearningLayersOIDCProvider : IOidcProvider
    {
        private const string authorizationEndpoint = "https://api.learning-layers.eu/o/oauth2/authorize";
        private const string tokenEndpoint = "https://api.learning-layers.eu/o/oauth2/token";
        private const string userInfoEndpoint = "https://api.learning-layers.eu/o/oauth2/userinfo";

        public AuthorizationFlow AuthorzationFlow { get; set; }

        public IContentLoader<string> ContentLoader { get; set; }

        public ClientData ClientData { get; set; }

        public LearningLayersOIDCProvider()
        {
            ContentLoader = new UnityWebRequestLoader();
        }

        public async Task<string> GetAccessTokenFromCodeAsync(string code, string redirectUri)
        {
            string uri = tokenEndpoint + $"?code={code}&client_id={ClientData.ClientId}" +
                $"&client_secret={ClientData.ClientId}&redirect_uri={redirectUri}&grant_type=authorization_code";
            WebResponse<string> response = await ContentLoader.LoadAsync(uri);
            if (response.Successful)
            {
                LearningLayersAuthorizationFlowAnswer answer =
                    JsonUtility.FromJson<LearningLayersAuthorizationFlowAnswer>(response.Content);
                if (answer == null)
                {
                    i5Debug.LogError("Could not parse access token in code flow answer", this);
                    return "";
                }
                return answer.access_token;
            }
            else
            {
                return "";
            }
        }

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

        public async Task<IUserInfo> GetUserInfoAsync(string accessToken)
        {
            WebResponse<string> webResponse = await ContentLoader.LoadAsync(userInfoEndpoint + "?access_token=" + accessToken);
            if (webResponse.Successful)
            {
                LearningLayersUserInfo userInfo = JsonUtility.FromJson<LearningLayersUserInfo>(webResponse.Content);
                if (userInfo == null)
                {
                    i5Debug.LogError("Could not parse user info", this);
                }
                return userInfo;
            }
            else
            {
                return default;
            }
        }

        public async Task<bool> CheckAccessTokenAsync(string accessToken)
        {
            IUserInfo userInfo = await GetUserInfoAsync(accessToken);
            return userInfo != null;
        }

        public void OpenLoginPage(string[] scopes, string redirectUri)
        {
            string responseType = AuthorzationFlow == AuthorizationFlow.AUTHORIZATION_CODE ? "code" : "implicit";
            string uriScopes = UriUtils.WordArrayToSpaceEscapedString(scopes);
            string uri = authorizationEndpoint + $"?response_type={responseType}&scope={uriScopes}" +
                $"&client_id={ClientData.ClientId}&redirect_uri={redirectUri}";
        }

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