using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.ServiceCore
{
    public abstract class BaseServiceManager : MonoBehaviour
    {
        public abstract void InstRegisterService<T>(T service) where T : IService;

        public abstract void InstRemoveService<T>() where T : IService;

        public abstract T InstGetService<T>() where T : IService;

        public abstract bool InstServiceExists<T>() where T : IService;
    }
}