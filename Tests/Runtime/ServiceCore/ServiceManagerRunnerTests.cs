using System.Collections;
using System.Collections.Generic;
using FakeItEasy;
using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.TestUtilities;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Core.Tests.ServiceCore
{
    public class ServiceManagerRunnerTests
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

        [UnityTest]
        public IEnumerator Update_CallsReceiverUpdate()
        {
            IRunnerReceiver runnerReceiver = A.Fake<IRunnerReceiver>();
            GameObject go = new GameObject();
            ServiceManagerRunner runner = go.AddComponent<ServiceManagerRunner>();
            runner.Initialize(runnerReceiver);

            yield return null;

            A.CallTo(() => runnerReceiver.Update()).MustHaveHappenedOnceExactly();
        }

        [UnityTest]
        public IEnumerator OnDestroy_ComponentDestroyed_CallsReceiverOnDestroy()
        {
            IRunnerReceiver runnerReceiver = A.Fake<IRunnerReceiver>();
            GameObject go = new GameObject();
            ServiceManagerRunner runner = go.AddComponent<ServiceManagerRunner>();
            runner.Initialize(runnerReceiver);

            GameObject.Destroy(runner);
            yield return null;
            A.CallTo(() => runnerReceiver.OnRunnerDestroyed()).MustHaveHappenedOnceExactly();
        }

        [UnityTest]
        public IEnumerator OnDestroy_GameObjectDestroyed_CallsReceiverOnDestroy()
        {
            IRunnerReceiver runnerReceiver = A.Fake<IRunnerReceiver>();
            GameObject go = new GameObject();
            ServiceManagerRunner runner = go.AddComponent<ServiceManagerRunner>();
            runner.Initialize(runnerReceiver);

            GameObject.Destroy(go);
            yield return null;
            A.CallTo(() => runnerReceiver.OnRunnerDestroyed()).MustHaveHappenedOnceExactly();
        }
    }
}
