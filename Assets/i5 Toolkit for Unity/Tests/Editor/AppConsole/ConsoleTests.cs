using i5.Toolkit.Core.AppConsole;
using i5.Toolkit.Core.Editor.TestHelpers;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Core.Tests.AppConsole
{
    /// <summary>
    /// Tests for the console logic
    /// </summary>
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

        /// <summary>
        /// Checks that no logs are captured if IsCapturing is set to false
        /// </summary>
        [Test]
        public void IsCapturing_False_NoLogsCaptured()
        {
            Console console = new Console();
            console.IsCapturing = false;

            Debug.Log("Test");

            Assert.AreEqual(0, console.Messages.Count);
        }

        /// <summary>
        /// Checks that the console captures logs when IsCapturing is true
        /// </summary>
        [Test]
        public void IsCapturing_True_LogsCaptured()
        {
            Console console = new Console();
            console.IsCapturing = true;

            Debug.Log("Test");

            Assert.AreEqual(1, console.Messages.Count);
        }

        /// <summary>
        /// Checks that the correct log type is captured if a log message is received
        /// </summary>
        [Test]
        public void MessageReceived_Log_CorrectTypeCaptured()
        {
            Console console = new Console();
            console.IsCapturing = true;

            Debug.Log("Test");

            Assert.AreEqual(LogType.Log, console.Messages[0].LogType);
        }

        /// <summary>
        /// Checks that the correct log type is captured if a warning message is received
        /// </summary>
        [Test]
        public void MessageReceived_Warning_CorrectTypeCaptured()
        {
            Console console = new Console();
            console.IsCapturing = true;

            Debug.LogWarning("Test");

            Assert.AreEqual(LogType.Warning, console.Messages[0].LogType);
        }

        /// <summary>
        /// Checks that the correct log type is captured if an error message is received
        /// </summary>
        [Test]
        public void MessageReceived_Error_CorrectTypeCaptured()
        {
            Console console = new Console();
            console.IsCapturing = true;

            LogAssert.Expect(LogType.Error, "Test");

            Debug.LogError("Test");

            Assert.AreEqual(LogType.Error, console.Messages[0].LogType);
        }

        /// <summary>
        /// Checks that the text content is captured if a log message is received
        /// </summary>
        [Test]
        public void MessageReceived_Log_TextCaptured()
        {
            const string text = "Test";

            Console console = new Console();
            console.IsCapturing = true;

            Debug.Log(text);

            Assert.AreEqual(text, console.Messages[0].Content);
        }

        /// <summary>
        /// Checks that the text content is captured if a warning message is received
        /// </summary>
        [Test]
        public void MessageReceived_LogWarning_TextCaptured()
        {
            const string text = "Test";

            Console console = new Console();
            console.IsCapturing = true;

            Debug.LogWarning(text);

            Assert.AreEqual(text, console.Messages[0].Content);
        }

        /// <summary>
        /// Checks that the text content is captured if an error message is received
        /// </summary>
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

        /// <summary>
        /// Checks that new messages invoke the OnMessageAdded event
        /// </summary>
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

        /// <summary>
        /// Checks that no event is invoked if the console is not capturing
        /// </summary>
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
