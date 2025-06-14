using i5.Toolkit.Core.DeepLinkAPI;
using i5.Toolkit.Core.Experimental.UnityAdapters;
using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities;
using System;
using System.Collections.Generic;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
	public class OpenIDConnectDeepLinker : IDeepLinkingService
	{
		/// <summary>
		/// Reference to an application adapter.
		/// In production, this is initialized with an adapter to Unity's Application/>.
		/// </summary>
		public IApplication ApplicationAPI { get; set; } = new ApplicationAdapter();

		private List<OpenIDConnectService> openIDConnectServices = new List<OpenIDConnectService>();

		public void Initialize(IServiceManager owner)
		{
			ApplicationAPI.DeepLinkActivated += OnDeepLinkActivated;
			if (!string.IsNullOrEmpty(ApplicationAPI.AbsoluteURL))
			{
				OnDeepLinkActivated(null, ApplicationAPI.AbsoluteURL);
			}
		}

		public void Cleanup()
		{
			ApplicationAPI.DeepLinkActivated -= OnDeepLinkActivated;
			openIDConnectServices.Clear();
		}

		public void AddDeepLinkListener(object listener)
		{
			if (listener is not OpenIDConnectService openIDConnectService)
			{
				i5Debug.LogWarning("The OpenIDConnect Deep Linker can only receive OpenIDConnectServices.", this);
				return;
			}

			openIDConnectServices.Add(openIDConnectService);
		}

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
