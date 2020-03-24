using i5.Toolkit.ServiceCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoBootstrapper : MonoBehaviour
{
    private void Start()
    {
        DemoService ds = new DemoService("This is a demo message.");
        ServiceManager.RegisterService(ds);
        DemoUpdateService dus = new DemoUpdateService(1f);
        ServiceManager.RegisterService(dus);
        DemoAsyncService das = new DemoAsyncService();
        ServiceManager.RegisterService(das);
    }
}
