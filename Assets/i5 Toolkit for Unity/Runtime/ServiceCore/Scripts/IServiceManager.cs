using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using i5.Toolkit.Core.OpenIDConnectClient;

namespace i5.Toolkit.Core.ServiceCore
{
    /// <summary>
    /// Contract which defines how to interact with service manager implementations
    /// </summary>
    public interface IServiceManager
    {
      
        /// <summary>
        /// The runner object which provides MonoBehaviour events to the service manager
        /// This can also be used by services to access MonoBehaviour functionality, e.g. for running co-routines
        /// </summary>
        ServiceManagerRunner Runner { get; }

        /// <summary>
        /// Registers a new service
        /// </summary>
        /// <typeparam name="T">The type of service</typeparam>
        /// <param name="service">The service to register</param>
        void InstRegisterService<T>(T service) where T : IService;

        /// <summary>
        /// Instance method for registering a new provider
        /// </summary>
        /// <typeparam name="T">The type of service</typeparam>
        /// <param name="provider">The provider instance which should be registered at the ServiceManager</param>
        /// <param name="type">The type of provider which should be registered at the ServiceManager</param>
        void InstRegisterProvider<T>(T provider, ProviderTypes type) where T : IService;

        /// <summary>
        /// Removes a service from the service manager
        /// </summary>
        /// <typeparam name="T">The type of service</typeparam>
        void InstRemoveService<T>() where T : IService;

        /// <summary>
        /// Removes a provider with the given type from this ServiceManager instance
        /// </summary>
        /// <typeparam name="T">The type of service</typeparam>
        /// <param name="type">The provider type that should be removed</param>
        void InstRemoveProvider<T>(ProviderTypes type) where T : IService;

        /// <summary>
        /// Retrieves the reference to a registered service of the given type
        /// </summary>
        /// <typeparam name="T">The type of service</typeparam>
        /// <returns>Returns the reference to the service</returns>
        T InstGetService<T>() where T : IService;

        /// <summary>
        /// Gets the provider instance with the given type that is registered at this instance
        /// </summary>
        /// <typeparam name="T">The type of service</typeparam>
        /// <param name="type">The provider type</param>
        /// <returns>Returns the registered provider instance</returns>
        T InstGetProvider<T>(ProviderTypes type) where T : IService;

        /// <summary>
        /// Checks if a service of the given type has been registered at the service manager
        /// </summary>
        /// <typeparam name="T">The type of the service</typeparam>
        /// <returns>Returns true if a service with the given type exists at the service manager</returns>
        bool InstServiceExists<T>() where T : IService;

        /// <summary>
        /// Checks if a provider with the given type exists at this instance
        /// </summary>
        /// <typeparam name="T">The type of service</typeparam>
        /// <param name="type">The provider type</param>
        /// <returns>Returns true if a service of the given type was registered at this instance</returns>
        bool InstProviderExists<T>(ProviderTypes type) where T : IService;
    }
}
