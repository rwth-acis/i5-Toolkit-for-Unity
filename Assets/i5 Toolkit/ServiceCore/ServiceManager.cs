using i5.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.ServiceCore
{
    public class ServiceManager : MonoBehaviour
    {
        private static Dictionary<object, IService> registeredServices = new Dictionary<object, IService>();

        private static List<IUpdateableService> updateableServices = new List<IUpdateableService>();

        private static ServiceManager instance;

        public static ServiceManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject serviceManagerObj = ObjectPool<GameObject>.RequestResource(() => { return new GameObject(); });
                    serviceManagerObj.name = "Service Manager";
                    instance = serviceManagerObj.AddComponent<ServiceManager>();
                }
                return instance;
            }
        }

        public static void RegisterService<T>(T service) where T : IService
        {
            registeredServices.Add(typeof(T), service);

            if (service.GetType() == typeof(IUpdateableService))
            {
                updateableServices.Add((IUpdateableService)service);
            }
        }

        public static void RemoveService<T>(T service) where T : IService
        {
            registeredServices.Remove(service);
            if (service.GetType() == typeof(IUpdateableService))
            {
                updateableServices.Remove((IUpdateableService)service);
            }
        }

        public static T GetService<T>(T service) where T : IService
        {
            if (!registeredServices.ContainsKey(typeof(T)))
            {
                throw new InvalidOperationException("Tried to get unregistered service");
            }
            return (T)registeredServices[typeof(T)];
        }
    }
}