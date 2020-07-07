using System.Collections.Generic;
using System.Threading.Tasks;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    public interface IOidcProvider
    {
        AuthorizationFlow AuthorzationFlow { get; }

        void OpenLoginPage(string clientId, string[] scopes, string redirectUri);

        Task<string> AccessTokenFromCodeAsync(Dictionary<string, string> redirectParameters);

        string GetAccessToken(Dictionary<string, string> redirectParameters);

        bool IsAccessTokenValid(string accessToken);

        Task<IUserInfo> GetUserInfoAsync(string accessToken);
    }
}