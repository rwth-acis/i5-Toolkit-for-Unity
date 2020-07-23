using i5.Toolkit.Core.OpenIDConnectClient;
using i5.Toolkit.Core.ServiceCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningLayersBootstrapper : MonoBehaviour, IServiceManagerBootstrapper
{
    [SerializeField]
    private ClientDataObject clientDataObject;

    public void InitializeServiceManager()
    {
        OpenIDConnectService oidc = new OpenIDConnectService(clientDataObject.clientData);
        oidc.OidcProvider = new LearningLayersOIDCProvider();
        ServiceManager.RegisterService(oidc);
    }
}
