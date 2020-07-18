using i5.Toolkit.Core.OpenIDConnectClient;
using i5.Toolkit.Core.Utilities.ContentLoaders;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.TestUtilities
{
    public class FakeOIDCProvider : IOidcProvider
    {
        public const string accessToken = "abcd-efgh-ijkl-mnop";
        public const string authorizationCode = "12345";

        public AuthorizationFlow AuthorzationFlow => throw new System.NotImplementedException();

        public IContentLoader<string> ContentLoader
        {
            get; set;
        }

        public ClientData ClientData { get; set; }

        public async Task<string> GetAccessTokenFromCodeAsync(string code, string redirectUri)
        {
            await Task.Delay(1);
            return accessToken;
        }

        public async Task<bool> CheckAccessTokenAsync(string accessToken)
        {
            await Task.Delay(1);
            return true;
        }

        public string GetAccessToken(Dictionary<string, string> redirectParameters)
        {
            return accessToken;
        }

        public async Task<IUserInfo> GetUserInfoAsync(string accessToken)
        {
            await Task.Delay(1);
            FakeUserInfo userInfo = new FakeUserInfo();
            return userInfo;
        }

        public bool IsAccessTokenValid(string accessToken)
        {
            return accessToken.Equals(FakeOIDCProvider.accessToken);
        }

        public void OpenLoginPage(string[] scopes, string redirectUri)
        {
            // do nothing
        }

        public string GetAuthorizationCode(Dictionary<string, string> redirectParameters)
        {
            return authorizationCode;
        }
    }
}