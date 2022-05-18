using i5.Toolkit.Core.ServiceCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.Examples.ServiceExample
{
    /// <summary>
    /// Demo service which demonstrates how a service can work
    /// </summary>
    public class DemoService : IService
    {
        /// <summary>
        /// A message that should be returned if the service is accessed
        /// </summary>
        private string message;

        /// <summary>
        /// Initializes the service when before it is added to the service manager
        /// </summary>
        /// <param name="message"></param>
        public DemoService(string message)
        {
            this.message = message;
        }

        /// <summary>
        /// Called if the service is unregistered or the service manager is destroyed
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
        /// Returns a demo message to show that the correct service is accessed
        /// </summary>
        /// <returns></returns>
        public string GetDemoMessage()
        {
            return message + " at time " + DateTime.Now;
        }
    }
}