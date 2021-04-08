using i5.Toolkit.Core.OpenIDConnectClient;
using i5.Toolkit.Core.ServiceCore;
using UnityEngine;

namespace i5.Toolkit.Core.Examples.OpenIDConnectClient
{
    /// <summary>
    /// Bootstrapper for initializing the OpenID Connect service for the learning layers provider
    /// </summary>
    public class LearningLayersBootstrapper : BaseServiceBootstrapper
    {
        [SerializeField]
        private ClientDataObject learningLayersClientData;

        protected override void RegisterServices()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            oidc.OidcProvider = new LearningLayersOidcProvider();
            oidc.OidcProvider.ClientData = learningLayersClientData.clientData;
            // this example shows how the service can be used on an app for multiple platforms
#if !UNITY_EDITOR
            oidc.RedirectURI = "i5:/";
#else
            oidc.RedirectURI = "https://www.google.com";
#endif
            ServiceManager.RegisterService(oidc);
        }

        protected override void UnRegisterServices()
        {
            ServiceManager.RemoveService<OpenIDConnectService>();
        }
    }
}