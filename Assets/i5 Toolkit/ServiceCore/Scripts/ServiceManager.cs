using i5.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.ServiceCore
{
    /// <summary>
    /// Manager which administers registered services
    /// These services need to implement the IService interface and do not need to inherit from MonoBehaviour
    /// </summary>
    public class ServiceManager : MonoBehaviour
    {
        private Dictionary<object, IService> registeredServices = new Dictionary<object, IService>();

        private List<IUpdateableService> updateableServices = new List<IUpdateableService>();

        private static ServiceManager instance;

        private static void EnsureInstance()
        {
            if (instance == null)
            {
                GameObject serviceManagerObj = ObjectPool<GameObject>.RequestResource(() => { return new GameObject(); });
                serviceManagerObj.name = "Service Manager";
                instance = serviceManagerObj.AddComponent<ServiceManager>();
            }
        }

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            IServiceManagerBootstrapper bootstrapper = GetComponent<IServiceManagerBootstrapper>();
            if (bootstrapper == null)
            {
                i5Debug.LogWarning("Service Manager does not have a bootstrapper.", this);
            }
            else
            {
                bootstrapper.InitializeServiceManager();
            }
        }

        public static void RegisterService<T>(T service) where T : IService
        {
            EnsureInstance();
            if (instance.registeredServices.ContainsKey(typeof(T)))
            {
                i5Debug.LogError("An instance of this service is already registered", instance);
            }
            instance.registeredServices.Add(typeof(T), service);

            if (service is IUpdateableService)
            {
                instance.updateableServices.Add((IUpdateableService)service);
            }
        }

        public static void RemoveService<T>(T service) where T : IService
        {
            EnsureInstance();
            instance.registeredServices.Remove(service);
            if (service is IUpdateableService)
            {
                instance.updateableServices.Remove((IUpdateableService)service);
            }
        }

        public static T GetService<T>() where T : IService
        {
            EnsureInstance();
            if (!instance.registeredServices.ContainsKey(typeof(T)))
            {
                throw new InvalidOperationException("Tried to get unregistered service");
            }
            return (T)instance.registeredServices[typeof(T)];
        }

        private void Update()
        {
            for (int i = 0; i < instance.updateableServices.Count; i++)
            {
                if (instance.updateableServices[i].Enabled)
                {
                    instance.updateableServices[i].Update();
                }
            }
        }

        private void OnDestroy()
        {
            foreach(KeyValuePair<object, IService> service in registeredServices)
            {
                service.Value.Cleanup();
            }
        }
    }
}