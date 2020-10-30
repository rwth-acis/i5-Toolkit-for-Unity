using System.Collections;
using System.Collections.Generic;
using FakeItEasy;
using i5.Toolkit.Core.AppConsole;
using i5.Toolkit.Core.Editor.TestHelpers;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Core.Tests.AppConsole
{
    public class TMPLogColorFormatterLogicTests
    {
        [SetUp]
        public void SetUp()
        {
            EditModeTestUtilities.ResetScene();
        }

        [Test]
        public void Format_Log_OutputUsesLogColor()
        {
            Color expectedColor = Color.green;
            TMPLogColorFormatterLogic formatter = new TMPLogColorFormatterLogic()
            {
                LogColor = expectedColor,
                ErrorColor = Color.red,
                AssertColor = Color.red,
                ExceptionColor = Color.red,
                WarningColor = Color.red,
                DefaultColor = Color.red
            };
            ILogMessage logMessage = CreateFakeLogMessage(LogType.Log, "content", "stackTrace");

            string result = formatter.Format(logMessage);

            Assert.True(result.Contains(ColorUtility.ToHtmlStringRGB(expectedColor)));
        }

        [Test]
        public void Format_ErrorLog_OutputUsesErrorColor()
        {
            Color expectedColor = Color.green;
            TMPLogColorFormatterLogic formatter = new TMPLogColorFormatterLogic()
            {
                LogColor = Color.red,
                ErrorColor = expectedColor,
                AssertColor = Color.red,
                ExceptionColor = Color.red,
                WarningColor = Color.red,
                DefaultColor = Color.red
            };
            ILogMessage logMessage = CreateFakeLogMessage(LogType.Error, "content", "stackTrace");

            string result = formatter.Format(logMessage);

            Assert.True(result.Contains(ColorUtility.ToHtmlStringRGB(expectedColor)));
        }

        [Test]
        public void Format_AssertLog_OutputUsesAssertColor()
        {
            Color expectedColor = Color.green;
            TMPLogColorFormatterLogic formatter = new TMPLogColorFormatterLogic()
            {
                LogColor = Color.red,
                ErrorColor = Color.red,
                AssertColor = expectedColor,
                ExceptionColor = Color.red,
                WarningColor = Color.red,
                DefaultColor = Color.red
            };
            ILogMessage logMessage = CreateFakeLogMessage(LogType.Assert, "content", "stackTrace");

            string result = formatter.Format(logMessage);

            Assert.True(result.Contains(ColorUtility.ToHtmlStringRGB(expectedColor)));
        }

        [Test]
        public void Format_ExceptionLog_OutputUsesExceptionColor()
        {
            Color expectedColor = Color.green;
            TMPLogColorFormatterLogic formatter = new TMPLogColorFormatterLogic()
            {
                LogColor = Color.red,
                ErrorColor = Color.red,
                AssertColor = Color.red,
                ExceptionColor = expectedColor,
                WarningColor = Color.red,
                DefaultColor = Color.red
            };
            ILogMessage logMessage = CreateFakeLogMessage(LogType.Exception, "content", "stackTrace");

            string result = formatter.Format(logMessage);

            Assert.True(result.Contains(ColorUtility.ToHtmlStringRGB(expectedColor)));
        }

        [Test]
        public void Format_WarningLog_OutputUsesWarningColor()
        {
            Color expectedColor = Color.green;
            TMPLogColorFormatterLogic formatter = new TMPLogColorFormatterLogic()
            {
                LogColor = Color.red,
                ErrorColor = Color.red,
                AssertColor = Color.red,
                ExceptionColor = Color.red,
                WarningColor = expectedColor,
                DefaultColor = Color.red
            };
            ILogMessage logMessage = CreateFakeLogMessage(LogType.Warning, "content", "stackTrace");

            string result = formatter.Format(logMessage);

            Assert.True(result.Contains(ColorUtility.ToHtmlStringRGB(expectedColor)));
        }

        [Test]
        public void Format_OutputContainsContent()
        {
            Color expectedColor = Color.green;
            TMPLogColorFormatterLogic formatter = new TMPLogColorFormatterLogic()
            {
                LogColor = Color.red,
                ErrorColor = Color.red,
                AssertColor = expectedColor,
                ExceptionColor = Color.red,
                WarningColor = Color.red,
                DefaultColor = Color.red
            };
            const string expectedContent = "my content";
            ILogMessage logMessage = CreateFakeLogMessage(LogType.Assert, expectedContent, "stackTrace");

            string result = formatter.Format(logMessage);

            Assert.True(result.Contains(expectedContent));
        }

        [Test]
        public void Format_OutputContainsStackTrace()
        {
            Color expectedColor = Color.green;
            TMPLogColorFormatterLogic formatter = new TMPLogColorFormatterLogic()
            {
                LogColor = Color.red,
                ErrorColor = Color.red,
                AssertColor = expectedColor,
                ExceptionColor = Color.red,
                WarningColor = Color.red,
                DefaultColor = Color.red
            };
            const string expectedStackTrace = "my stack trace";
            ILogMessage logMessage = CreateFakeLogMessage(LogType.Assert, "content", expectedStackTrace);

            string result = formatter.Format(logMessage);

            Assert.True(result.Contains(expectedStackTrace));
        }


        private ILogMessage CreateFakeLogMessage(LogType type, string content, string stackTrace)
        {
            ILogMessage logMessage = A.Fake<ILogMessage>();
            A.CallTo(() => logMessage.LogType).Returns(type);
            A.CallTo(() => logMessage.Content).Returns(content);
            A.CallTo(() => logMessage.StackTrace).Returns(stackTrace);
            return logMessage;
        }
    }
}
