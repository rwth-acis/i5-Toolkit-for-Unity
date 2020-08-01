using i5.Toolkit.Core.OpenIDConnectClient;
using i5.Toolkit.Core.ServiceCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningLayersBootstrapper : MonoBehaviour, IServiceManagerBootstrapper
{
    [SerializeField]
    private OpenIDConnectServiceConfiguration openIdConnectServiceConfiguration;

    public void InitializeServiceManager()
    {
        OpenIDConnectService oidc = new OpenIDConnectService(openIdConnectServiceConfiguration);
        oidc.OidcProvider = new LearningLayersOIDCProvider();
        ServiceManager.RegisterService(oidc);
    }
}
