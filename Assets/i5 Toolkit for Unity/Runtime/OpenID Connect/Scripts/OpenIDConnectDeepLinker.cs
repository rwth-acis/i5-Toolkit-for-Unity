using i5.Toolkit.Core.DeepLinkAPI;
using i5.Toolkit.Core.Experimental.UnityAdapters;
using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities;
using i5.Toolkit.Core.VerboseLogging;
using System;
using System.Collections.Generic;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
	/// <summary>
	/// Specialized deep link service which is tailored to the OpenIDConnectService
	/// Avoids using reflection compared to the default DeepLinkingService implementation
	/// </summary>
	public class OpenIDConnectDeepLinker : IDeepLinkingService
	{
		/// <summary>
		/// Reference to an application adapter.
		/// In production, this is initialized with an adapter to Unity's Application/>.
		/// </summary>
		public IApplication ApplicationAPI { get; set; } = new ApplicationAdapter();

		// multiple OIDC services can register themselves with this service
		private List<IOpenIDConnectService> openIDConnectServices = new List<IOpenIDConnectService>();

		public int RegisteredOpenIDConnectServices { get => openIDConnectServices.Count; }

		/// <summary>
		/// Initializes the service
		/// Registers to receive deep links
		/// </summary>
		/// <param name="owner">The service manager at which the service is registered</param>
		public void Initialize(IServiceManager owner)
		{
			ApplicationAPI.DeepLinkActivated += OnDeepLinkActivated;
			if (!string.IsNullOrEmpty(ApplicationAPI.AbsoluteURL))
			{
				OnDeepLinkActivated(null, ApplicationAPI.AbsoluteURL);
			}
		}

		/// <summary>
		/// Cleans up the service
		/// Unregisters from deep links
		/// </summary>
		public void Cleanup()
		{
			ApplicationAPI.DeepLinkActivated -= OnDeepLinkActivated;
			openIDConnectServices.Clear();
		}

		/// <summary>
		/// Registers a new listener for deep links
		/// In this implementation, only IOpenIDConnectServices can register themselves.
		/// </summary>
		/// <param name="listener">The listener for deep links to register - must be an IOpenIDConnectService</param>
		public void AddDeepLinkListener(object listener)
		{
			if (listener is not IOpenIDConnectService openIDConnectService)
			{
				i5Debug.LogError("The OpenIDConnect Deep Linker can only handle objects which implement IOpenIDConnectService.", this);
				return;
			}

			if (!openIDConnectServices.Contains(openIDConnectService))
			{
				openIDConnectServices.Add(openIDConnectService);
			}
		}

		/// <summary>
		/// Removes an object as a deep link receiver
		/// In this implementation, only objects implementing IOpenIDConnectService are handled.
		/// </summary>
		/// <param name="listener">The listener for the deep link which should be unregistered - must be an OpenIDConnectService</param>
		public void RemoveDeepLinkListener(object listener)
		{
			if (listener is not IOpenIDConnectService openIDConnectService)
			{
				i5Debug.LogError("The OpenIDConnect Deep Linker can only receive objects implementing IOpenIDConnectService.", this);
				return;
			}

			openIDConnectServices.Remove(openIDConnectService);
		}

		// Called if a deep link was found
		private void OnDeepLinkActivated(object sender, string deepLink)
		{
			AppLog.LogTrace("Got deep link for " + deepLink, this);

			Uri uri = new Uri(deepLink);

			if (uri.Authority.ToLower() == "login" || string.IsNullOrEmpty(uri.Authority))
			{
				AppLog.LogDebug("Processing deep link to handle login: " + deepLink, this);
				Dictionary<string, string> fragments = UriUtils.GetUriParameters(uri);
				DeepLinkArgs args = new DeepLinkArgs(fragments, uri);

				foreach (IOpenIDConnectService registeredService in openIDConnectServices)
				{
					registeredService.HandleActivation(args);
				}
			}
			else
			{
				AppLog.LogTrace("Ignoring deep link since it is not for the login.", this);
			}
		}
	}
}
