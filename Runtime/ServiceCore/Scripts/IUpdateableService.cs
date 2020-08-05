using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.ServiceCore
{
    /// <summary>
    /// A service which can execute code every frame
    /// </summary>
    public interface IUpdateableService : IService
    {
        /// <summary>
        /// If set to true, the update function is executed every frame; otherwise the service is disabled
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Called by the service manager and executed every frame
        /// </summary>
        void Update();
    }
}