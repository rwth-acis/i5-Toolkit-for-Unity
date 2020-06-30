using i5.Toolkit.Core.ServiceCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.TestUtilities
{
    public class TestBootstrapper : MonoBehaviour, IServiceManagerBootstrapper
    {
        public void InitializeServiceManager()
        {
            ServiceManager.RegisterService(new TestService());
        }
    }
}