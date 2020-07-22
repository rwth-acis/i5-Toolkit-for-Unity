using i5.Toolkit.Core.Utilities.ContentLoaders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    public interface IOidcProvider
    {
        IContentLoader<string> ContentLoader { get; set; }
        AuthorizationFlow AuthorzationFlow { get; }

        ClientData ClientData { get; set; }

        void OpenLoginPage(string[] scopes, string redirectUri);

        string GetAuthorizationCode(Dictionary<string, string> redirectParameters);

        bool ParametersContainError(Dictionary<string, string> parameters, out string errorMessage);

        Task<string> GetAccessTokenFromCodeAsync(string code, string redirectUri);

        string GetAccessToken(Dictionary<string, string> redirectParameters);

        Task<bool> CheckAccessTokenAsync(string accessToken);

        Task<IUserInfo> GetUserInfoAsync(string accessToken);
    }
}