using i5.Toolkit.Core.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.ServiceCore
{
    /// <summary>
    /// Manager which administers registered services
    /// These services need to implement the IService interface and do not need to inherit from MonoBehaviour
    /// </summary>
    public class ServiceManager : IServiceManager, IRunnerReceiver
    {
        private Dictionary<Type, IService> registeredServices = new Dictionary<Type, IService>();

        private List<IUpdateableService> updateableServices = new List<IUpdateableService>();

        private static ServiceManager instance;

        private bool applicationQuitting = false;

        private GameObject runnerObject;

        public static ServiceManager Instance
        {
            get
            {
                EnsureInstance();
                return instance;
            }
        }

        public ServiceManagerRunner Runner { get; private set; }

        private static void EnsureInstance()
        {
            if (instance == null)
            {
                instance = new ServiceManager();
            }
        }

        public ServiceManager()
        {
            CreateRunner();
            Application.quitting += OnApplicationQuitting;
        }

        private void CreateRunner()
        {
            runnerObject = ObjectPool<GameObject>.RequestResource(() => { return new GameObject(); });
            runnerObject.name = "Service Manager Runner";
#if !UNITY_EDITOR
            GameObject.DontDestroyOnLoad(runnerObject);
#endif
            Runner = runnerObject.AddComponent<ServiceManagerRunner>();
            Runner.Initialize(this);
        }

        public static void RegisterService<T>(T service) where T : IService
        {
            EnsureInstance();
            instance.InstRegisterService(service);
        }

        public void InstRegisterService<T>(T service) where T : IService
        {
            if (registeredServices.ContainsKey(typeof(T)))
            {
                i5Debug.LogError("An instance of this service is already registered", this);
                return;
            }
            registeredServices.Add(typeof(T), service);

            if (service is IUpdateableService)
            {
                updateableServices.Add((IUpdateableService)service);
            }

            service.Initialize(this);
        }

        public static void RemoveService<T>() where T : IService
        {
            EnsureInstance();
            instance.InstRemoveService<T>();
        }

        public void InstRemoveService<T>() where T : IService
        {
            if (registeredServices.ContainsKey(typeof(T)))
            {
                IService toRemove = registeredServices[typeof(T)];
                if (toRemove is IUpdateableService)
                {
                    updateableServices.Remove((IUpdateableService)toRemove);
                }
                toRemove.Cleanup();
                registeredServices.Remove(typeof(T));
            }
            else
            {
                throw new InvalidOperationException("Tried to remove unregistered service");
            }
        }

        public static T GetService<T>() where T : IService
        {
            EnsureInstance();
            return instance.InstGetService<T>();
        }

        public T InstGetService<T>() where T : IService
        {
            if (!registeredServices.ContainsKey(typeof(T)))
            {
                throw new InvalidOperationException("Tried to get unregistered service");
            }
            return (T)registeredServices[typeof(T)];
        }

        public static bool ServiceExists<T>() where T : IService
        {
            EnsureInstance();
            return instance.InstServiceExists<T>();
        }

        public bool InstServiceExists<T>() where T : IService
        {
            return registeredServices.ContainsKey(typeof(T));
        }

        public void Update()
        {
            for (int i = 0; i < updateableServices.Count; i++)
            {
                if (updateableServices[i].Enabled)
                {
                    updateableServices[i].Update();
                }
            }
        }

        public void OnRunnerDestroyed()
        {
            // make sure that the entire object is destroyed
            GameObject.Destroy(runnerObject);
            // then re-create it if the application is not quitting
            if (!applicationQuitting)
            {
                CreateRunner();
            }
        }

        private void OnApplicationQuitting()
        {
            foreach (KeyValuePair<Type, IService> service in registeredServices)
            {
                service.Value.Cleanup();
            }
            applicationQuitting = true;
        }
    }
}