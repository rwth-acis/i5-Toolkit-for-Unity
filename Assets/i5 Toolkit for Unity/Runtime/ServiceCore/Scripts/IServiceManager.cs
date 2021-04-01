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
        /// Removes a service from the service manager
        /// </summary>
        /// <typeparam name="T">The type of service</typeparam>
        void InstRemoveService<T>() where T : IService;

        /// <summary>
        /// Retrieves the reference to a registered service of the given type
        /// </summary>
        /// <typeparam name="T">The type of service</typeparam>
        /// <returns>Returns the reference to the service</returns>
        T InstGetService<T>() where T : IService;

        /// <summary>
        /// Checks if a service of the given type has been registered at the service manager
        /// </summary>
        /// <typeparam name="T">The type of the service</typeparam>
        /// <returns>Returns true if a service with the given type exists at the service manager</returns>
        bool InstServiceExists<T>() where T : IService;
    }
}
