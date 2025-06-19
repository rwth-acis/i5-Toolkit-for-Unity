using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.OpenIDConnectClient;

namespace i5.Toolkit.Core.Examples.OpenIDConnectClient
{
    public class GitHubOIDCBootstrapper : BaseServiceBootstrapper
    {
        [SerializeField] private ClientDataObject deepLinkClient;
        [SerializeField] private ClientDataObject serverClient;

        protected override void RegisterServices()
        {
            OpenIDConnectService gitHubOidc = new OpenIDConnectService();
            gitHubOidc.OidcProvider = new GitHubOidcProvider();
            // this example shows how the service can be used on an app for multiple platforms
#if !UNITY_EDITOR
            gitHubOidc.RedirectURI = "i5:/";
#else
            gitHubOidc.RedirectURI = "https://www.google.com";
#endif

// GitHub only allows one redirect URI per client in its client registration
// therefore, you need to create multiple ones if you need deep linking and the server redirect
// this example also shows how to already set up the client data 
#if !UNITY_EDITOR
            gitHubOidc.OidcProvider.ClientData = deepLinkClient.clientData;
#else
            gitHubOidc.OidcProvider.ClientData = serverClient.clientData;
#endif

            ServiceManager.RegisterService(gitHubOidc);
        }

        protected override void UnRegisterServices()
        {
        }
    }
}