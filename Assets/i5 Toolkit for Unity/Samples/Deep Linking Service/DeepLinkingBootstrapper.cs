using i5.Toolkit.Core.ServiceCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepLinkingBootstrapper : BaseServiceBootstrapper
{
    protected override void RegisterServices()
    {
        DeepLinkReceiver receiver = new DeepLinkReceiver();
        DeepLinkingService service = new DeepLinkingService(new object[] { receiver });
        ServiceManager.RegisterService(service);
    }

    protected override void UnRegisterServices()
    {
        ServiceManager.RemoveService<DeepLinkingService>();
    }
}
