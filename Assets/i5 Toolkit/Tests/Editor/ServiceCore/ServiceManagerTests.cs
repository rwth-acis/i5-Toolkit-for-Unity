using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.TestUtilities;
using i5.Toolkit.Core.Utilities;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Core.Tests.ServiceCore
{
    /// <summary>
    /// Tests for the ServiceManager class
    /// </summary>
    public class ServiceManagerTests
    {
        /// <summary>
        /// Resets the scene to the standard test scene before executed each test
        /// </summary>
        [SetUp]
        public void ResetScene()
        {
            EditModeTestUtilities.ResetScene();
        }

        /// <summary>
        /// Checks that an instance is returned in an empty scene
        /// </summary>
        [Test]
        public void Instance_NoInstanceInScene_ReturnsNotNull()
        {
            ServiceManager manager = ServiceManager.Instance;
            Assert.IsTrue(manager != null);
        }

        /// <summary>
        /// Checks that an instance object was created if an instance is requested in an empty scene
        /// </summary>
        [Test]
        public void Instance_NoInstanceInScene_InstanceCreated()
        {
            ServiceManager manager = ServiceManager.Instance;
            GameObject obj = GameObject.Find("Service Manager");
            Assert.IsTrue(obj != null);
        }

        /// <summary>
        /// Checks that a new instance is created if the existing service manager instance was destroyed
        /// </summary>
        [Test]
        public void Instance_InstanceDestroyed_CreatesNewInstance()
        {
            ServiceManager manager = ServiceManager.Instance;
            Assert.IsTrue(manager != null);
            GameObject.DestroyImmediate(manager.gameObject);
            Assert.IsTrue(manager == null);
            manager = ServiceManager.Instance;
            Assert.IsTrue(manager != null);
        }

        /// <summary>
        /// Checks that a registered service is available
        /// </summary>
        [Test]
        public void RegisterService_ServiceRegistered_ServiceFound()
        {
            TestService testService = new TestService();
            ServiceManager.RegisterService(testService);

            Assert.IsTrue(ServiceManager.ServiceExists<TestService>());
        }

        /// <summary>
        /// Checks that a service which is registered on top of an existing service returns logs an error
        /// </summary>
        [Test]
        public void RegisterService_ServiceRegisteredDouble_ErrorLogged()
        {
            TestService testService = new TestService();
            ServiceManager.RegisterService(testService);
            TestService testService2 = new TestService();
            ServiceManager.RegisterService(testService2);
            LogAssert.Expect(LogType.Error, new Regex(@"\w*An instance of this service is already registered\w*"));
        }

        /// <summary>
        /// Checks that the first registered service keeps being used if we try to register another service on top of it
        /// </summary>
        [Test]
        public void RegisterService_ServiceRegisteredDouble_FirstServiceStored()
        {
            TestService testService = new TestService();
            ServiceManager.RegisterService(testService);
            TestService testService2 = new TestService();
            ServiceManager.RegisterService(testService2);
            LogAssert.Expect(LogType.Error, new Regex(@"\w*An instance of this service is already registered\w*"));
            TestService retrieved = ServiceManager.GetService<TestService>();
            Assert.AreEqual(testService, retrieved);
        }

        /// <summary>
        /// Checks that a registered service is initialized
        /// </summary>
        [Test]
        public void RegisterService_ServiceRegistered_ServiceInitialized()
        {
            TestService testService = new TestService();
            ServiceManager.RegisterService(testService);

            Assert.AreEqual(100, testService.TestCounter);
        }

        /// <summary>
        /// Checks that ServiceExists returns false for services that do not exist
        /// </summary>
        [Test]
        public void ServiceExists_ServiceNotRegistered_ReturnsFalse()
        {
            Assert.IsFalse(ServiceManager.ServiceExists<TestService>());
        }

        /// <summary>
        /// Checks that ServiceExists returns true for services that exist
        /// </summary>
        [Test]
        public void ServiceExists_ServiceRegistered_ReturnsTrue()
        {
            ServiceManager.RegisterService(new TestService());
            Assert.IsTrue(ServiceManager.ServiceExists<TestService>());
        }

        /// <summary>
        /// Checks that GetService throws an exception if a service is called which is not registered
        /// </summary>
        [Test]
        public void GetService_ServiceNotRegistered_ThrowsException()
        {
            Assert.Throws<InvalidOperationException>(
                delegate
                {
                    ServiceManager.GetService<TestService>();
                });
        }

        /// <summary>
        /// Checks that GetService returns registered services
        /// </summary>
        [Test]
        public void GetService_ServiceRegistered_ReturnsService()
        {
            TestService testService = new TestService();
            ServiceManager.RegisterService(testService);
            TestService retrieved = ServiceManager.GetService<TestService>();
            Assert.AreEqual(testService, retrieved);
        }

        /// <summary>
        /// Checks that existing services are removed by RemoveService
        /// </summary>
        [Test]
        public void RemoveService_ServiceExists_ServiceRemoved()
        {
            ServiceManager.RegisterService(new TestService());
            Assert.IsTrue(ServiceManager.ServiceExists<TestService>());
            ServiceManager.RemoveService<TestService>();
            Assert.IsFalse(ServiceManager.ServiceExists<TestService>());
        }

        /// <summary>
        /// Checks that a service which is being removed is cleaned up properly
        /// </summary>
        [Test]
        public void RemoveServcie_ServiceExists_ServiceCleanedUp()
        {
            ServiceManager.RegisterService(new TestService());
            ServiceManager.RemoveService<TestService>();
            LogAssert.Expect(LogType.Log, new Regex(@"\w*Cleaned up test service\w*"));
        }

        /// <summary>
        /// Checks that RemoveService throws an exception if we try to remove a service which does not exist
        /// </summary>
        [Test]
        public void RemoveService_ServiceDoesNotExist_ThrowsException()
        {
            Assert.Throws<InvalidOperationException>(delegate
           {
               ServiceManager.RemoveService<TestService>();
           });
        }
    }
}
