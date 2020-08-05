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
    [SerializeField]
    private OpenIDConnectServiceConfiguration openIdConnectServiceConfiguration;

    protected override void RegisterServices()
    {
        OpenIDConnectService oidc = new OpenIDConnectService(openIdConnectServiceConfiguration);
        oidc.OidcProvider = new LearningLayersOIDCProvider();
        ServiceManager.RegisterService(oidc);
    }

    protected override void UnRegisterServices()
    {
        ServiceManager.RemoveService<OpenIDConnectService>();
    }
}
