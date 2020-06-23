using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using i5.Toolkit.ServiceCore;
using i5.Toolkit.TestUtilities;
using i5.Toolkit.Utilities;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Tests.ServiceCore
{
    public class ServiceManagerTests
    {
        /// <summary>
        /// Resets the scene to the standard test scene before executed each test
        /// </summary>
        [SetUp]
        public void ResetScene()
        {
            EditorSceneManager.OpenScene(PathUtils.GetPackagePath() + "Tests/TestResources/SetupTestScene.unity");
        }

        [Test]
        public void Instance_NoInstanceInScene_ReturnsNotNull()
        {
            ServiceManager manager = ServiceManager.Instance;
            Assert.IsTrue(manager != null);
        }

        [Test]
        public void Instance_NoInstanceInScene_InstanceCreated()
        {
            ServiceManager manager = ServiceManager.Instance;
            GameObject obj = GameObject.Find("Service Manager");
            Assert.IsTrue(obj != null);
        }

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

        [Test]
        public void RegisterService_ServiceRegistered_ServiceFound()
        {
            TestService testService = new TestService();
            ServiceManager.RegisterService(testService);

            Assert.IsTrue(ServiceManager.ServiceExists<TestService>());
        }

        [Test]
        public void RegisterService_ServiceRegisteredDouble_ErrorLogged()
        {
            TestService testService = new TestService();
            ServiceManager.RegisterService(testService);
            TestService testService2 = new TestService();
            ServiceManager.RegisterService(testService2);
            LogAssert.Expect(LogType.Error, new Regex(@"\w*An instance of this service is already registered\w*"));
        }

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

        [Test]
        public void RegisterService_ServiceRegistered_ServiceInitialized()
        {
            TestService testService = new TestService();
            ServiceManager.RegisterService(testService);

            Assert.AreEqual(100, testService.TestCounter);
        }

        [Test]
        public void ServiceExists_ServiceNotRegistered_ReturnsFalse()
        {
            Assert.IsFalse(ServiceManager.ServiceExists<TestService>());
        }

        [Test]
        public void ServiceExists_ServiceRegistered_ReturnsTrue()
        {
            ServiceManager.RegisterService(new TestService());
            Assert.IsTrue(ServiceManager.ServiceExists<TestService>());
        }

        [Test]
        public void GetService_ServiceNotRegistered_ThrowsException()
        {
            Assert.Throws<InvalidOperationException>(
                delegate
                {
                    ServiceManager.GetService<TestService>();
                });
        }

        [Test]
        public void GetService_ServiceRegistered_ReturnsService()
        {
            TestService testService = new TestService();
            ServiceManager.RegisterService(testService);
            TestService retrieved = ServiceManager.GetService<TestService>();
            Assert.AreEqual(testService, retrieved);
        }

        [Test]
        public void RemoveService_ServiceExists_ServiceRemoved()
        {
            ServiceManager.RegisterService(new TestService());
            Assert.IsTrue(ServiceManager.ServiceExists<TestService>());
            ServiceManager.RemoveService<TestService>();
            Assert.IsFalse(ServiceManager.ServiceExists<TestService>());
        }

        [Test]
        public void RemoveServcie_ServiceExists_ServiceCleanedUp()
        {
            ServiceManager.RegisterService(new TestService());
            ServiceManager.RemoveService<TestService>();
            LogAssert.Expect(LogType.Log, new Regex(@"\w*Cleaned up test service\w*"));
        }

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
