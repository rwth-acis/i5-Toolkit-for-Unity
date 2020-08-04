using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FakeItEasy;
using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.TestUtilities;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace i5.Toolkit.Core.Tests.ServiceCore
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

        /// <summary>
        /// Checks that a new runner is created if the existing runner was destroyed
        /// </summary>
        [UnityTest]
        public IEnumerator Runner_RunnerGameObjectDestroyed_CreatesNewRunner()
        {
            ServiceManager manager = new ServiceManager();
            Assert.IsTrue(manager.Runner != null);
            manager.Runner.gameObject.name = "Old Runner";
            GameObject.Destroy(manager.Runner.gameObject);
            yield return null;
            Assert.IsTrue(manager.Runner != null);
            Assert.AreNotEqual("Old Runner", manager.Runner.name);
        }

        [UnityTest]
        public IEnumerator Runner_RunnerComponentDestroyed_DestroysGameObject()
        {
            ServiceManager manager = new ServiceManager();
            GameObject runnerObj = manager.Runner.gameObject;
            GameObject.Destroy(manager.Runner);
            yield return null;
            Assert.IsTrue(runnerObj == null);
        }

        [UnityTest]
        public IEnumerator Runner_RunnerComponentDestroyed_CreatesNewRunner()
        {
            ServiceManager manager = new ServiceManager();
            Assert.IsTrue(manager.Runner != null);
            manager.Runner.gameObject.name = "Old Runner";
            GameObject.Destroy(manager.Runner);
            yield return null;
            Assert.IsTrue(manager.Runner != null);
            Assert.AreNotEqual("Old Runner", manager.Runner.name);
        }

        [UnityTest]
        public IEnumerator Update_NoUpdateableServices_NoError()
        {
            // create a service manager
            ServiceManager serviceManager = new ServiceManager();
            // go over two frames
            yield return null;
            yield return null;
        }

        [UnityTest]
        public IEnumerator Update_UpdateableServiceEnabled_ServiceUpdated()
        {
            ServiceManager serviceManager = new ServiceManager();
            IUpdateableService updateableService = A.Fake<IUpdateableService>();
            A.CallTo(() => updateableService.Enabled).Returns(true);
            serviceManager.InstRegisterService(updateableService);
            yield return null;
            yield return null;
            A.CallTo(() => updateableService.Update()).MustHaveHappenedTwiceExactly();
        }

        [UnityTest]
        public IEnumerator Update_UpdateableServiceDisabled_ServiceNotUpdated()
        {
            ServiceManager serviceManager = new ServiceManager();
            IUpdateableService updateableService = A.Fake<IUpdateableService>();
            A.CallTo(() => updateableService.Enabled).Returns(false);
            serviceManager.InstRegisterService(updateableService);
            yield return null;
            yield return null;
            A.CallTo(() => updateableService.Update()).MustNotHaveHappened();
        }
    }
}
