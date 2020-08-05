using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.ServiceCore
{
    /// <summary>
    /// Contract which defines the capabilities of a service
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// Initializes the service
        /// </summary>
        /// <param name="owner">The IServiceManager which owns this service</param>
        void Initialize(IServiceManager owner);

        /// <summary>
        /// Cleans up the service when it is unregistered at the service manager
        /// </summary>
        void Cleanup();
    }
}