using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.Examples.ServiceExample
{
    /// <summary>
    /// Demo of an upadteable service
    /// Logs the elapsed application's runtime in seconds in regular intervals
    /// </summary>
    public class DemoUpdateService : IUpdateableService
    {
        float updateInterval;
        float time = 0;

        /// <summary>
        /// Every updateable service has an Enabled property
        /// It this property is set to false, the Update() method is not executed
        /// It is recommended to set this to true by default
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Initializes the service before it is registered
        /// </summary>
        /// <param name="updateInterval">The interval in seconds in which the time should be logged</param>
        public DemoUpdateService(float updateInterval)
        {
            this.updateInterval = updateInterval;
        }

        /// <summary>
        /// Called if the service is unregistered or if the service manager is destroyed
        /// </summary>
        public void Cleanup()
        {
        }

        /// <summary>
        /// Called if the service is registered at the service manager
        /// </summary>
        /// <param name="owner">The service manager where the service is registered</param>
        public void Initialize(IServiceManager owner)
        {
        }

        /// <summary>
        /// Called every frame by the service manager if this service is registered and enabled
        /// </summary>
        public void Update()
        {
            time += Time.deltaTime;
            if (time > updateInterval)
            {
                i5Debug.Log(Time.time.ToString(), this);
                time %= updateInterval;
            }
        }
    }
}