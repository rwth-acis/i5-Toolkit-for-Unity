using i5.Toolkit.Core.ServiceCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepLinkingBootstrapper : BaseServiceBootstrapper
{
    protected override void RegisterServices()
    {
        DeepLinkingService service = new DeepLinkingService();
        DeepLinkReceiver reciever = new DeepLinkReceiver();
        service.AddListenerClass(reciever);
        ServiceManager.RegisterService(service);
    }

    protected override void UnRegisterServices()
    {
        ServiceManager.RemoveService<DeepLinkingService>();
    }
}
