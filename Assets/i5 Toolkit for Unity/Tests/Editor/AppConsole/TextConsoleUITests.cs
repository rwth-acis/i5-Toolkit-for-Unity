using FakeItEasy;
using i5.Toolkit.Core.AppConsole;
using i5.Toolkit.Core.Editor.TestHelpers;
using i5.Toolkit.Core.Experimental.UnityAdapters;
using NUnit.Framework;

namespace i5.Toolkit.Core.Tests.AppConsole
{
    /// <summary>
    /// Tests for the text console UI
    /// </summary>
    public class TextConsoleUITests
    {
        /// <summary>
        /// Resets the scene
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            EditModeTestUtilities.ResetScene();
        }

        /// <summary>
        /// Checks that the console starts capturing when the object is enabled
        /// </summary>
        [Test]
        public void OnEnable_ConsoleCaptureTrue()
        {
            TextConsoleUI textConsoleUI = SetupTextConsoleUI(out ITextDisplay textAdapter, out IConsole console);

            textConsoleUI.OnEnable();

            Assert.True(console.IsCapturing);
        }

        /// <summary>
        /// Checks that the console keeps capturing in background if the console is configured for background capture and the object is deactivated
        /// </summary>
        [Test]
        public void OnDisable_CaptureInBackground_ConsoleCaptureTrue()
        {
            TextConsoleUI textConsoleUI = SetupTextConsoleUI(out ITextDisplay textAdapter, out IConsole console);
            textConsoleUI.CaptureInBackground = true;

            textConsoleUI.OnEnable();
            textConsoleUI.OnDisable();
            Assert.True(console.IsCapturing);
        }

        /// <summary>
        /// Checks that the console stops capturing if the console should not capture in background and is deactivated
        /// </summary>
        [Test]
        public void OnDisable_NoCaptureInBackground_ConsoleCaptureFalse()
        {
            TextConsoleUI textConsoleUI = SetupTextConsoleUI(out ITextDisplay textAdapter, out IConsole console);
            textConsoleUI.CaptureInBackground = false;

            textConsoleUI.OnEnable();
            textConsoleUI.OnDisable();
            Assert.False(console.IsCapturing);
        }

        /// <summary>
        /// Checks that the console writes an empty text if no messages have been captured yet
        /// </summary>
        [Test]
        public void UpdateUI_NoMessages_WritesEmptyString()
        {
            TextConsoleUI textConsoleUI = SetupTextConsoleUI(out ITextDisplay textAdapter, out IConsole console);

            textConsoleUI.OnEnable();
            textConsoleUI.GetType()
                .GetMethod("UpdateUI", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(textConsoleUI, null);

            Assert.AreEqual("", textAdapter.Text);
        }

        /// <summary>
        /// Checks that a captured log message is written to the text display
        /// </summary>
        [Test]
        public void UpdateUI_MessageGiven_WritesMessage()
        {
            const string message = "Hello World";

            TextConsoleUI textConsoleUI = SetupTextConsoleUI(out ITextDisplay textAdapter, out IConsole console);
            ILogMessage logMessage = A.Fake<ILogMessage>();
            A.CallTo(() => logMessage.Content).Returns(message);
            console.Messages.Add(logMessage);

            textConsoleUI.OnEnable();
            textConsoleUI.GetType()
                .GetMethod("UpdateUI", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(textConsoleUI, null);

            Assert.AreEqual(message, textAdapter.Text);
        }

        /// <summary>
        /// Sets up a text console UI instance for the tests
        /// </summary>
        /// <param name="textDisplay">The text display which is used to show console's captured messages</param>
        /// <param name="console">The console which handles the capturing</param>
        /// <returns></returns>
        private TextConsoleUI SetupTextConsoleUI(out ITextDisplay textDisplay, out IConsole console)
        {
            textDisplay = A.Fake<ITextDisplay>();
            TextConsoleUI textConsoleUI = new TextConsoleUI(textDisplay);
            console = A.Fake<Console>();

            textConsoleUI.Console = console;
            return textConsoleUI;
        }
    }
}
