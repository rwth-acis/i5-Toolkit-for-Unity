using System.Collections;
using System.Collections.Generic;
using FakeItEasy;
using i5.Toolkit.Core.AppConsole;
using i5.Toolkit.Core.Editor.TestHelpers;
using i5.Toolkit.Core.Utilities.UnityAdapters;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Core.Tests.AppConsole
{
    public class TextConsoleUITests
    {
        [SetUp]
        public void SetUp()
        {
            EditModeTestUtilities.ResetScene();
        }

        [Test]
        public void OnEnable_ConsoleCaptureTrue()
        {
            TextConsoleUI textConsoleUI = SetupTextConsoleUI(out ITextDisplay textAdapter, out IConsole console);

            textConsoleUI.OnEnable();

            Assert.True(console.IsCapturing);
        }

        [Test]
        public void OnDisable_CaptureInBackground_ConsoleCaptureTrue()
        {
            TextConsoleUI textConsoleUI = SetupTextConsoleUI(out ITextDisplay textAdapter, out IConsole console);
            textConsoleUI.CaptureInBackground = true;

            textConsoleUI.OnEnable();
            textConsoleUI.OnDisable();
            Assert.True(console.IsCapturing);
        }

        [Test]
        public void OnDisable_NoCaptureInBackground_ConsoleCaptureFalse()
        {
            TextConsoleUI textConsoleUI = SetupTextConsoleUI(out ITextDisplay textAdapter, out IConsole console);
            textConsoleUI.CaptureInBackground = false;

            textConsoleUI.OnEnable();
            textConsoleUI.OnDisable();
            Assert.False(console.IsCapturing);
        }

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

        private TextConsoleUI SetupTextConsoleUI(out ITextDisplay textAdapter, out IConsole console)
        {
            textAdapter = A.Fake<ITextDisplay>();
            TextConsoleUI textConsoleUI = new TextConsoleUI(textAdapter);
            console = A.Fake<Console>();

            textConsoleUI.Console = console;
            return textConsoleUI;
        }
    }
}
