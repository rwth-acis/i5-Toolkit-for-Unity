using i5.Toolkit.Core.OpenIDConnectClient;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.TestUtilities
{
    public class FakeOIDCProvider : IOidcProvider
    {
        public const string accessToken = "abcd-efgh-ijkl-mnop";

        public AuthorizationFlow AuthorzationFlow => throw new System.NotImplementedException();

        public async Task<string> AccessTokenFromCodeAsync(Dictionary<string, string> redirectParameters)
        {
            await Task.Delay(1);
            return accessToken;
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

        public void OpenLoginPage(string clientId, string[] scopes, string redirectUri)
        {
            // do nothing
        }
    }
}