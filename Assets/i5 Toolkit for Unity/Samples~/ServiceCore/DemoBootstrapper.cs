using i5.Toolkit.Core.ServiceCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.Examples.ServiceExample
{
    /// <summary>
    /// Bootstrapper which populates the service manager with the required services for the service demo
    /// </summary>
    public class DemoBootstrapper : BaseServiceBootstrapper
    {
        /// <summary>
        /// Initializes the service manager with the required services
        /// Called by the service manager if it is placed on the same GameObject at the start of the application
        /// </summary>
        protected override void RegisterServices()
        {
            DemoService ds = new DemoService("This is a demo message.");
            ServiceManager.RegisterService(ds);
            DemoUpdateService dus = new DemoUpdateService(1f);
            ServiceManager.RegisterService(dus);
            DemoAsyncService das = new DemoAsyncService();
            ServiceManager.RegisterService(das);
        }

        protected override void UnRegisterServices()
        {
            ServiceManager.RemoveService<DemoService>();
            ServiceManager.RemoveService<DemoUpdateService>();
            ServiceManager.RemoveService<DemoAsyncService>();
        }
    }
}