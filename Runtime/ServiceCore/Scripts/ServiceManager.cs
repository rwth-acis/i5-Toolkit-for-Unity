using i5.Toolkit.Core.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        private GameObject runnerObject;

        private static bool applicationQuitting;

        /// <summary>
        /// The instance of the service manager
        /// </summary>
        public static ServiceManager Instance
        {
            get
            {
                EnsureInstance();
                return instance;
            }
        }

        /// <summary>
        /// The runner in the scene
        /// This runner object provides MonoBehaviour events to the service manager
        /// It can be accessed by servies to run MonoBehaviour functionality, e.g. co-routines
        /// </summary>
        public ServiceManagerRunner Runner { get; private set; }

        private static void EnsureInstance()
        {
            if (instance == null)
            {
                instance = new ServiceManager();
            }
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            Application.quitting += OnApplicationQuitting;
        }

        /// <summary>
        /// Creates a new instance of a ServiceManager
        /// </summary>
        public ServiceManager()
        {
            if (!applicationQuitting)
            {
                CreateRunner();
            }
        }

        private void CreateRunner()
        {
            if (applicationQuitting)
            {
                return;
            }

            // create a new runner object and make it persistent
            runnerObject = ObjectPool<GameObject>.RequestResource(() => { return new GameObject(); });
            runnerObject.name = "Service Manager Runner";
            PersistenceScene.MarkPersistent(runnerObject);
            Runner = runnerObject.AddComponent<ServiceManagerRunner>();
            Runner.Initialize(this);
        }

        /// <summary>
        /// Registers a new service at the service manager
        /// </summary>
        /// <typeparam name="T">The type of service</typeparam>
        /// <param name="service">The service instance which should be registered at the ServiceManager</param>
        public static void RegisterService<T>(T service) where T : IService
        {
            EnsureInstance();
            instance.InstRegisterService(service);
        }

        /// <summary>
        /// Instance method for registering a new service
        /// </summary>
        /// <typeparam name="T">The type of service</typeparam>
        /// <param name="service">The service instance which should be registered</param>
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

        /// <summary>
        /// Removes a provider with the given type from the ServiceManager
        /// </summary>
        /// <typeparam name="T">The type of service</typeparam>
        public static void RemoveService<T>() where T : IService
        {
            EnsureInstance();
            instance.InstRemoveService<T>();
        }

        /// <summary>
        /// Removes a service with the given type from this ServiceManager instance
        /// </summary>
        /// <typeparam name="T">The type of service</typeparam>
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

        /// <summary>
        /// Gets the service instance with the given type
        /// </summary>
        /// <typeparam name="T">The type of service</typeparam>
        /// <returns>Returns the registered service instance</returns>
        public static T GetService<T>() where T : IService
        {
            EnsureInstance();
            return instance.InstGetService<T>();
        }

        /// <summary>
        /// Gets the service instance with the given type that is registered at this instance
        /// </summary>
        /// <typeparam name="T">The type of service</typeparam>
        /// <returns>Returns the registered service instance</returns>
        public T InstGetService<T>() where T : IService
        {
            if (!registeredServices.ContainsKey(typeof(T)))
            {
                throw new InvalidOperationException("Tried to get unregistered service");
            }
            return (T)registeredServices[typeof(T)];
        }

        /// <summary>
        /// Checks if a service with the given type exists at the ServiceManager
        /// </summary>
        /// <typeparam name="T">The type of service</typeparam>
        /// <returns>Returns true if a service of the given type was registered</returns>
        public static bool ServiceExists<T>() where T : IService
        {
            EnsureInstance();
            return instance.InstServiceExists<T>();
        }

        /// <summary>
        /// Checks if a service with the given type exists at this instance
        /// </summary>
        /// <typeparam name="T">The type of service</typeparam>
        /// <returns>Returns true if a service of the given type was registered at this instance</returns>
        public bool InstServiceExists<T>() where T : IService
        {
            return registeredServices.ContainsKey(typeof(T));
        }

        /// <summary>
        /// Called by the update runner
        /// Updates the updateable services
        /// </summary>
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

        /// <summary>
        /// Called if the runner object is destroyed
        /// Makes sure that the runner persists if it was destroyed by some other script
        /// </summary>
        public void OnRunnerDestroyed()
        {
            // make sure that the entire object is destroyed
            GameObject.Destroy(runnerObject);
            // then re-create it
            if (!applicationQuitting)
            {
                CreateRunner();
            }
        }

        // called when the application is quitting
        private static void OnApplicationQuitting()
        {
            applicationQuitting = true;
            // if there is an instance: clean it up
            // if there is no instance: do not create a new one since this will leak a runner into the scene
            if (instance != null)
            {
                foreach (KeyValuePair<Type, IService> service in instance.registeredServices)
                {
                    service.Value.Cleanup();
                }
            }
        }
    }
}