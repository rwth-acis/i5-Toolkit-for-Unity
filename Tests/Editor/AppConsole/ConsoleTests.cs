using System.Collections;
using System.Collections.Generic;
using i5.Toolkit.Core.AppConsole;
using i5.Toolkit.Core.Editor.TestHelpers;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Core.Tests.AppConsole
{
    public class ConsoleTests
    {
        /// <summary>
        /// Resets the scene for each test
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            EditModeTestUtilities.ResetScene();
        }

        [Test]
        public void IsCapturing_False_NoLogsCaptured()
        {
            Console console = new Console();
            console.IsCapturing = false;

            Debug.Log("Test");

            Assert.AreEqual(0, console.Messages.Count);
        }

        [Test]
        public void IsCapturing_True_LogsCaptured()
        {
            Console console = new Console();
            console.IsCapturing = true;

            Debug.Log("Test");

            Assert.AreEqual(1, console.Messages.Count);
        }

        [Test]
        public void MessageReceived_Log_CorrectTypeCaptured()
        {
            Console console = new Console();
            console.IsCapturing = true;

            Debug.Log("Test");

            Assert.AreEqual(LogType.Log, console.Messages[0].LogType);
        }

        [Test]
        public void MessageReceived_Warning_CorrectTypeCaptured()
        {
            Console console = new Console();
            console.IsCapturing = true;

            Debug.LogWarning("Test");

            Assert.AreEqual(LogType.Warning, console.Messages[0].LogType);
        }

        [Test]
        public void MessageReceived_Error_CorrectTypeCaptured()
        {
            Console console = new Console();
            console.IsCapturing = true;

            LogAssert.Expect(LogType.Error, "Test");

            Debug.LogError("Test");

            Assert.AreEqual(LogType.Error, console.Messages[0].LogType);
        }

        [Test]
        public void MessageReceived_Log_TextCaptured()
        {
            const string text = "Test";

            Console console = new Console();
            console.IsCapturing = true;

            Debug.Log(text);

            Assert.AreEqual(text, console.Messages[0].Content);
        }

        [Test]
        public void MessageReceived_LogWarning_TextCaptured()
        {
            const string text = "Test";

            Console console = new Console();
            console.IsCapturing = true;

            Debug.LogWarning(text);

            Assert.AreEqual(text, console.Messages[0].Content);
        }

        [Test]
        public void MessageReceived_LogError_TextCaptured()
        {
            const string text = "Test";

            Console console = new Console();
            console.IsCapturing = true;

            LogAssert.Expect(LogType.Error, "Test");

            Debug.LogError(text);

            Assert.AreEqual(text, console.Messages[0].Content);
        }

        [Test]
        public void OnMessageAdded_IsCapturing_EventInvoked()
        {
            bool eventInvoked = false;

            Console console = new Console();
            console.IsCapturing = true;

            console.OnMessageAdded += () =>
            {
                eventInvoked = true;
            };

            Debug.Log("Test");

            Assert.True(eventInvoked);
        }

        [Test]
        public void OnMessageAdded_IsNotCapturing_EventNotInvoked()
        {
            bool eventInvoked = false;

            Console console = new Console();
            console.IsCapturing = false;

            console.OnMessageAdded += () =>
            {
                eventInvoked = true;
            };

            Debug.Log("Test");

            Assert.False(eventInvoked);
        }
    }
}
