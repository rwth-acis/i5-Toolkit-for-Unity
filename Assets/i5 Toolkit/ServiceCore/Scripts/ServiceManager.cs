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
        private Dictionary<Type, IService> registeredServices = new Dictionary<Type, IService>();

        private List<IUpdateableService> updateableServices = new List<IUpdateableService>();

        // keep track of a list of services that should be removed and remove them at the end of the frame
        // this way, we are not modifying the list of services in the global cleanup at the end
        private List<Type> serviceTypesToRemove = new List<Type>();

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

            service.Initialize(instance);
        }

        public static void RemoveService<T>() where T : IService
        {
            EnsureInstance();
            if (instance.registeredServices.ContainsKey(typeof(T)))
            {
                instance.serviceTypesToRemove.Add(typeof(T));
            }
            else
            {
                throw new InvalidOperationException("Tried to remove unregistered service");
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

        public static bool ServiceExists<T>() where T : IService
        {
            EnsureInstance();
            return instance.registeredServices.ContainsKey(typeof(T));
        }

        private void Update()
        {
            for (int i = 0; i < updateableServices.Count; i++)
            {
                if (updateableServices[i].Enabled)
                {
                    updateableServices[i].Update();
                }
            }

            if (serviceTypesToRemove.Count > 0)
            {
                for (int i = 0; i < serviceTypesToRemove.Count; i++)
                {
                    registeredServices.Remove(serviceTypesToRemove[i]);
                }
                serviceTypesToRemove.Clear();
            }
        }

        private void OnDestroy()
        {
            foreach (KeyValuePair<Type, IService> service in registeredServices)
            {
                service.Value.Cleanup();
            }
        }
    }
}