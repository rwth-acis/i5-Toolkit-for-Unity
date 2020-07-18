using i5.Toolkit.Core.ServiceCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.TestUtilities
{
    public class FakeServiceManager : BaseServiceManager
    {
        private Dictionary<Type, IService> registeredServices = new Dictionary<Type, IService>();

        public override void InstRegisterService<T>(T service)
        {
            registeredServices.Add(typeof(T),service);
        }

        public override void InstRemoveService<T>()
        {
            registeredServices.Remove(typeof(T));
        }

        public override T InstGetService<T>()
        {
            return (T)registeredServices[typeof(T)];
        }

        public override bool InstServiceExists<T>()
        {
            return registeredServices.ContainsKey(typeof(T));
        }
    }
}