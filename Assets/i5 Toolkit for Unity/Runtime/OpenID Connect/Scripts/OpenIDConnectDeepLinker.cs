using i5.Toolkit.Core.DeepLinkAPI;
using i5.Toolkit.Core.Experimental.UnityAdapters;
using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities;
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
		private List<OpenIDConnectService> openIDConnectServices = new List<OpenIDConnectService>();

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
		/// In this implementation, only OpenIDConnectServices can register themselves.
		/// </summary>
		/// <param name="listener">The listener for deep links to register - must be an OpenIDConnectService</param>
		public void AddDeepLinkListener(object listener)
		{
			if (listener is not OpenIDConnectService openIDConnectService)
			{
				i5Debug.LogWarning("The OpenIDConnect Deep Linker can only receive OpenIDConnectServices.", this);
				return;
			}

			openIDConnectServices.Add(openIDConnectService);
		}

		/// <summary>
		/// Removes an object as a deep link receiver
		/// In this implementation, only OpenIDConnectServices are handled.
		/// </summary>
		/// <param name="listener">The listener for the deep link which should be unregistered - must be an OpenIDConnectService</param>
		public void RemoveDeepLinkListener(object listener)
		{
			if (listener is not OpenIDConnectService openIDConnectService)
			{
				i5Debug.LogWarning("The OpenIDConnect Deep Linker can only receive OpenIDConnectServices.", this);
				return;
			}

			openIDConnectServices.Remove(openIDConnectService);
		}

		// Called if a deep link was found
		private void OnDeepLinkActivated(object sender, string deepLink)
		{
			i5Debug.Log("Got deep link for " + deepLink, this);

			Uri uri = new Uri(deepLink);
			Dictionary<string, string> fragments = UriUtils.GetUriParameters(uri);
			DeepLinkArgs args = new DeepLinkArgs(fragments, uri);

			foreach (OpenIDConnectService registeredService in openIDConnectServices)
			{
				registeredService.HandleActivation(args);
			}
		}
	}
}
