using System.Collections;
using System.Collections.Generic;
using i5.Toolkit.Core.TestUtilities;
using i5.Toolkit.Core.Utilities;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Core.Tests.Utilities
{
    public class i5DebugTests
    {
        [SetUp]
        public void ResetScene()
        {
            EditModeTestUtilities.ResetScene();
        }

        [Test]
        public void Log_ObjectSender_MessageLogged()
        {
            i5Debug.Log("Test message", this);
            LogAssert.Expect(LogType.Log, "[i5.Toolkit.Core.Tests.Utilities.i5DebugTests] Test message");
        }

        [Test]
        public void Log_MonoBehaviourSender_MessageLogged()
        {
            Rigidbody rigidbody = SampleMonoBehaviourSetup();
            i5Debug.Log("Test message", rigidbody);
            LogAssert.Expect(LogType.Log, "[UnityEngine.Rigidbody] Test message");
        }

        [Test]
        public void LogError_ObjectSender_MessageLogged()
        {
            i5Debug.LogError("Test message", this);
            LogAssert.Expect(LogType.Error, "[i5.Toolkit.Core.Tests.Utilities.i5DebugTests] Test message");
        }

        [Test]
        public void LogError_MonoBehaviourSender_MessageLogged()
        {
            Rigidbody rigidbody = SampleMonoBehaviourSetup();
            i5Debug.LogError("Test message", rigidbody);
            LogAssert.Expect(LogType.Error, "[UnityEngine.Rigidbody] Test message");
        }

        [Test]
        public void LogWarning_ObjectSender_MessageLogged()
        {
            i5Debug.LogWarning("Test message", this);
            LogAssert.Expect(LogType.Warning, "[i5.Toolkit.Core.Tests.Utilities.i5DebugTests] Test message");
        }

        [Test]
        public void LogWarning_MonoBehaviourSender_MessageLogged()
        {
            Rigidbody rigidbody = SampleMonoBehaviourSetup();
            i5Debug.LogWarning("Test message", rigidbody);
            LogAssert.Expect(LogType.Warning, "[UnityEngine.Rigidbody] Test message");
        }

        private Rigidbody SampleMonoBehaviourSetup()
        {
            GameObject go = new GameObject();
            Rigidbody rigidbody = go.AddComponent<Rigidbody>();
            return rigidbody;
        }
    }
}
