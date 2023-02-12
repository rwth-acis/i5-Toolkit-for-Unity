using i5.Toolkit.Core.Utilities;
using i5.Toolkit.Core.Utilities.ContentLoaders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    /// <summary>
    /// Contract that specifies the capabilities of an OpenID Connect provider
    /// </summary>
    public interface IOidcProvider
    {
        /// <summary>
        /// Specifies how the REST API of the Web service is accessed
        /// </summary>
        IRestConnector RestConnector { get; set; }

        /// <summary>
        /// Gets the authorization flow that should be used for the provider
        /// </summary>
        AuthorizationFlow AuthorizationFlow { get; }

        /// <summary>
        /// Client data that are required to authorize the client at the provider
        /// </summary>
        ClientData ClientData { get; set; }

        /// <summary>
        /// Opens the provider's login page in the system's default Web browser
        /// </summary>
        /// <param name="scopes">The OpenID Connect scopes that the user must agree to</param>
        /// <param name="redirectUri">The URI to which the browser should redirect after the successful login</param>
        void OpenLoginPage(string[] scopes, string redirectUri);

        /// <summary>
        /// Extracts the authorization code from parameters of a Web answer
        /// </summary>
        /// <param name="redirectParameters">Parameters of a Web answer as a dictionary</param>
        /// <returns>The authorization code if it could be found, otherwise an empty string is returned</returns>
        string GetAuthorizationCode(Dictionary<string, string> redirectParameters);

        /// <summary>
        /// Checks if the provider included error messages in the parameters of a Web answer
        /// </summary>
        /// <param name="parameters">The parameters of a Web answer as a dictionary</param>
        /// <param name="errorMessage">The error message that the provider included, empty if no error exists</param>
        /// <returns>Returns true if the parameters contain an error message, otherwise false</returns>
        bool ParametersContainError(Dictionary<string, string> parameters, out string errorMessage);

        /// <summary>
        /// Gets the access token based on a previously retrieved authorization code
        /// </summary>
        /// <param name="code">The authorization code</param>
        /// <param name="redirectUri">The redirect URI which was used during the login</param>
        /// <returns>Returns the access token if it could be retrieved; otherwise it returns an empty string</returns>
        Task<string> GetAccessTokenFromCodeAsync(string code, string redirectUri);

        /// <summary>
        /// Gets the access token from a list of parameters in a Web answer
        /// </summary>
        /// <param name="redirectParameters">The parameters of the Web answer as a dictionary</param>
        /// <returns>Returns the access token if it exists in the parameters,
        /// otherwise an empty string is returned</returns>
        string GetAccessToken(Dictionary<string, string> redirectParameters);

        /// <summary>
        /// Checks if the access token is valid by checking it at the provider
        /// </summary>
        /// <param name="accessToken">The access token that should be checked</param>
        /// <returns>True if the access token is valid, otherwise false</returns>
        Task<bool> CheckAccessTokenAsync(string accessToken);

        /// <summary>
        /// Gets information about the logged in user from the provider
        /// </summary>
        /// <param name="accessToken">The access token to authenticate the user</param>
        /// <returns>Returns information about the logged in user if the request was successful, otherwise null</returns>
        Task<IUserInfo> GetUserInfoAsync(string accessToken);

        /// <summary>
        /// Sets the required endpoints
        /// </summary>
        /// <returns>Returns the fetched endpoints response</returns>
        Task<EndpointsData> InitializeEndpointsAsync();
    }
}