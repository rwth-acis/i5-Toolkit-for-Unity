using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FakeItEasy;
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
        /// Checks that a runner object was created if an instance is requested in an empty scene
        /// </summary>
        [Test]
        public void Constructor_Called_RunnerCreated()
        {
            ServiceManager manager = new ServiceManager();
            Assert.IsTrue(manager.Runner != null);

            GameObject find = GameObject.Find(manager.Runner.name);
            Assert.IsTrue(find != null);
        }

        [Test]
        public void Constructor_Called_ComponentAddedToRunner()
        {
            ServiceManager manager = new ServiceManager();
            ServiceManagerRunner serviceManagerRunner = manager.Runner.GetComponent<ServiceManagerRunner>();
            Assert.IsTrue(serviceManagerRunner != null);
        }

        /// <summary>
        /// Checks that a registered service is available
        /// </summary>
        [Test]
        public void RegisterService_ServiceRegistered_ServiceFound()
        {
            ServiceManager serviceManager = new ServiceManager();
            IService service = A.Fake<IService>();
            serviceManager.InstRegisterService(service);

            Assert.IsTrue(serviceManager.InstServiceExists<IService>());
        }

        /// <summary>
        /// Checks that a service which is registered on top of an existing service returns logs an error
        /// </summary>
        [Test]
        public void RegisterService_ServiceRegisteredDouble_ErrorLogged()
        {
            ServiceManager serviceManager = new ServiceManager();
            IService service1 = A.Fake<IService>();
            IService service2 = A.Fake<IService>();
            serviceManager.InstRegisterService(service1);

            LogAssert.Expect(LogType.Error, new Regex(@"\w*An instance of this service is already registered\w*"));
            serviceManager.InstRegisterService(service2);
        }

        /// <summary>
        /// Checks that the first registered service keeps being used if we try to register another service on top of it
        /// </summary>
        [Test]
        public void RegisterService_ServiceRegisteredDouble_FirstServiceStored()
        {
            ServiceManager serviceManager = new ServiceManager();
            IService service1 = A.Fake<IService>();
            IService service2 = A.Fake<IService>();
            serviceManager.InstRegisterService(service1);

            LogAssert.Expect(LogType.Error, new Regex(@"\w*An instance of this service is already registered\w*"));
            serviceManager.InstRegisterService(service2);

            IService retrieved = serviceManager.InstGetService<IService>();
            Assert.AreEqual(service1, retrieved);
        }

        /// <summary>
        /// Checks that a registered service is initialized
        /// </summary>
        [Test]
        public void RegisterService_ServiceRegistered_ServiceInitialized()
        {
            ServiceManager serviceManager = new ServiceManager();
            IService service = A.Fake<IService>();
            serviceManager.InstRegisterService(service);

            A.CallTo(() => service.Initialize(A<IServiceManager>.Ignored)).MustHaveHappenedOnceExactly();
        }

        /// <summary>
        /// Checks that ServiceExists returns false for services that do not exist
        /// </summary>
        [Test]
        public void ServiceExists_ServiceNotRegistered_ReturnsFalse()
        {
            ServiceManager serviceManager = new ServiceManager();
            Assert.IsFalse(serviceManager.InstServiceExists<IService>());
        }

        /// <summary>
        /// Checks that ServiceExists returns true for services that exist
        /// </summary>
        [Test]
        public void ServiceExists_ServiceRegistered_ReturnsTrue()
        {
            ServiceManager serviceManager = new ServiceManager();
            IService service = A.Fake<IService>();
            serviceManager.InstRegisterService(service);
            Assert.IsTrue(serviceManager.InstServiceExists<IService>());
        }

        /// <summary>
        /// Checks that GetService throws an exception if a service is called which is not registered
        /// </summary>
        [Test]
        public void GetService_ServiceNotRegistered_ThrowsException()
        {
            ServiceManager serviceManager = new ServiceManager();
            Assert.Throws<InvalidOperationException>(
(TestDelegate)delegate
                {
                    serviceManager.InstGetService<IService>();
                });
        }

        /// <summary>
        /// Checks that GetService returns registered services
        /// </summary>
        [Test]
        public void GetService_ServiceRegistered_ReturnsService()
        {
            ServiceManager serviceManager = new ServiceManager();
            IService service = A.Fake<IService>();
            serviceManager.InstRegisterService(service);
            IService retrieved = serviceManager.InstGetService<IService>();
            Assert.AreEqual(service, retrieved);
        }

        /// <summary>
        /// Checks that existing services are removed by RemoveService
        /// </summary>
        [Test]
        public void RemoveService_ServiceExists_ServiceRemoved()
        {
            ServiceManager serviceManager = new ServiceManager();
            IService service = A.Fake<IService>();
            serviceManager.InstRegisterService(service);
            Assert.IsTrue(serviceManager.InstServiceExists<IService>());
            serviceManager.InstRemoveService<IService>();
            Assert.IsFalse(serviceManager.InstServiceExists<IService>());
        }

        /// <summary>
        /// Checks that a service which is being removed is cleaned up properly
        /// </summary>
        [Test]
        public void RemoveServcie_ServiceExists_ServiceCleanedUp()
        {
            ServiceManager serviceManager = new ServiceManager();
            IService service = A.Fake<IService>();
            serviceManager.InstRegisterService(service);
            serviceManager.InstRemoveService<IService>();
            A.CallTo(() => service.Cleanup()).MustHaveHappenedOnceExactly();
        }

        /// <summary>
        /// Checks that RemoveService throws an exception if we try to remove a service which does not exist
        /// </summary>
        [Test]
        public void RemoveService_ServiceDoesNotExist_ThrowsException()
        {
            ServiceManager serviceManager = new ServiceManager();
            Assert.Throws<InvalidOperationException>((TestDelegate)delegate
           {
               serviceManager.InstRemoveService<IService>();
           });
        }
    }
}
