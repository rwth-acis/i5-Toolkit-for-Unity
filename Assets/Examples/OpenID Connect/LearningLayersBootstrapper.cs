using i5.Toolkit.Core.OpenIDConnectClient;
using i5.Toolkit.Core.ServiceCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bootstrapper for initializing the OpenID Connect service for the learning layers provider
/// </summary>
public class LearningLayersBootstrapper : BaseServiceBootstrapper
{
    protected override void RegisterServices()
    {
        OpenIDConnectService oidc = new OpenIDConnectService();
        oidc.OidcProvider = new LearningLayersOIDCProvider();
        oidc.RedirectURI = "i5:/";
        ServiceManager.RegisterService(oidc);
    }

    protected override void UnRegisterServices()
    {
        ServiceManager.RemoveService<OpenIDConnectService>();
    }
}
