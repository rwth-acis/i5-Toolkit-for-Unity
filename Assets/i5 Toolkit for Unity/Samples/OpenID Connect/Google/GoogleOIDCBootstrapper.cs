using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.OpenIDConnectClient;

namespace i5.Toolkit.Core.Examples.OpenIDConnectClient
{
    public class GoogleOIDCBootstrapper : BaseServiceBootstrapper
    {
        [SerializeField] private ClientDataObject deepLinkClient;
        [SerializeField] private ClientDataObject serverClient;

        protected override void RegisterServices()
        {
            OpenIDConnectService googleOidc = new OpenIDConnectService();
            googleOidc.OidcProvider = new GoogleOidcProvider();
            // this example shows how the service can be used on an app for multiple platforms
#if !UNITY_EDITOR
            googleOidc.RedirectURI = "com.i5.i5-toolkit-for-unity:/";
#else
            googleOidc.RedirectURI = "https://www.google.com";
#endif

            googleOidc.ServerListener.ListeningUri = "http://127.0.0.1:52229/";

            // GitHub only allows one redirect URI per client in its client registration
            // therefore, you need to create multiple ones if you need deep linking and the server redirect
            // this example also shows how to already set up the client data 
#if !UNITY_EDITOR
            googleOidc.OidcProvider.ClientData = deepLinkClient.clientData;
#else
            googleOidc.OidcProvider.ClientData = serverClient.clientData;
#endif

            ServiceManager.RegisterService(googleOidc);
        }

        protected override void UnRegisterServices()
        {
        }
    }
}