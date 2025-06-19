using i5.Toolkit.Core.DeepLinkAPI;
using i5.Toolkit.Core.ServiceCore;
using System;
using System.Threading.Tasks;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
	public interface IOpenIDConnectService
	{
		string AccessToken { get; }
		IDeepLinkingService DeepLinker { get; set; }
		bool IsLoggedIn { get; }
		IOidcProvider OidcProvider { get; set; }
		string RedirectURI { get; set; }
		string[] Scopes { get; set; }
		IRedirectServerListener ServerListener { get; set; }

		event EventHandler LoginCompleted;
		event EventHandler LogoutCompleted;

		Task<bool> CheckAccessToken();
		void Cleanup();
		Task<IUserInfo> GetUserDataAsync();
		void HandleActivation(DeepLinkArgs deepLinkArgs);
		void Initialize(IServiceManager owner);
		void LoginWithAccessToken(string accessToken);
		void Logout();
		Task OpenLoginPageAsync();
		void ServerListener_RedirectReceived(object sender, RedirectReceivedEventArgs e);
	}
}