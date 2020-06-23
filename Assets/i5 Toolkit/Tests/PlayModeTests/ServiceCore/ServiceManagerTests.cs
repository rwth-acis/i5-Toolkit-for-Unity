using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using i5.Toolkit.ServiceCore;
using i5.Toolkit.TestUtilities;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace i5.Toolkit.Tests.ServiceCore
{
    public class ServiceManagerTests
    {
        [SetUp]
        public void LoadScene()
        {
            PlayModeTestUtilities.LoadEmptyTestScene();
        }

        [TearDown]
        public void TearDownScene()
        {
            PlayModeTestUtilities.UnloadTestScene();
        }

        [Test]
        public void Awake_MultipleInstances_LogsError()
        {
            GameObject go1 = new GameObject("Service Manager 1");
            go1.AddComponent<ServiceManager>();
            GameObject go2 = new GameObject("Service Manager 2");
            go2.AddComponent<ServiceManager>();
            LogAssert.Expect(LogType.Error, new Regex(@"\w*There are multiple Service Managers\w*"));
        }

        [Test]
        public void Awake_ManualComponentAdded_ComponentBecomesInstance()
        {
            GameObject serviceManagerObj = new GameObject("Service Manager");
            ServiceManager addedComponent = serviceManagerObj.AddComponent<ServiceManager>();
            Assert.AreEqual(addedComponent, ServiceManager.Instance);
        }

        [UnityTest]
        public IEnumerator Start_BootstrapperProvided_BootstrapperLoaded()
        {
            GameObject serviceManagerObj = new GameObject("Service Manager");
            serviceManagerObj.AddComponent<ServiceManager>();
            serviceManagerObj.AddComponent<TestBootstrapper>();
            yield return null;
            // now the Start() method should have been called

            Assert.IsTrue(ServiceManager.ServiceExists<TestService>());
        }

        [UnityTest]
        public IEnumerator OnDestroy_Destroy_ServiceCleanedUp()
        {
            ServiceManager.RegisterService(new TestService());
            GameObject.Destroy(ServiceManager.Instance);
            // wait a frame so that the Destroy takes effect
            yield return null;
            LogAssert.Expect(LogType.Log, new Regex(@"\w*Cleaned up test service\w*"));
        }

        [UnityTest]
        public IEnumerator Update_NoUpdateableServices_NoError()
        {
            // create a service manager instance
            ServiceManager serviceManager = ServiceManager.Instance;
            // go over two frames
            yield return null;
            yield return null;
        }

        [UnityTest]
        public IEnumerator Update_UpdateableServiceEnabled_ServiceUpdated()
        {
            TestUpdateService testUpdateService = new TestUpdateService();
            testUpdateService.Enabled = true;
            ServiceManager.RegisterService(testUpdateService);
            Assert.AreEqual(100, testUpdateService.TestCounter);
            yield return null;
            Assert.AreEqual(101, testUpdateService.TestCounter);
            yield return null;
            Assert.AreEqual(102, testUpdateService.TestCounter);
        }

        [UnityTest]
        public IEnumerator Update_UpdateableServiceDisabled_ServiceNotUpdated()
        {
            TestUpdateService testUpdateService = new TestUpdateService();
            testUpdateService.Enabled = false;
            ServiceManager.RegisterService(testUpdateService);
            Assert.AreEqual(100, testUpdateService.TestCounter);
            yield return null;
            Assert.AreEqual(100, testUpdateService.TestCounter);
        }
    }
}
