using i5.Toolkit.ServiceCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.TestUtilities
{
    public class TestBootstrapper : MonoBehaviour, IServiceManagerBootstrapper
    {
        public void InitializeServiceManager()
        {
            ServiceManager.RegisterService(new TestService());
        }
    }
}