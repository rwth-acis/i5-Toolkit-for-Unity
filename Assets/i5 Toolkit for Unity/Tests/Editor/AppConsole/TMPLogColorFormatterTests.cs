using FakeItEasy;
using i5.Toolkit.Core.AppConsole;
using i5.Toolkit.Core.Editor.TestHelpers;
using NUnit.Framework;
using UnityEngine;

namespace i5.Toolkit.Core.Tests.AppConsole
{
    /// <summary>
    /// Tests for the TextMeshPro color formatter
    /// </summary>
    public class TMPLogColorFormatterTests
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
        /// Checks that log messages are formatted using the log color
        /// </summary>
        [Test]
        public void Format_Log_OutputUsesLogColor()
        {
            Color expectedColor = Color.green;
            TMPLogColorFormatter formatter = new TMPLogColorFormatter()
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

        /// <summary>
        /// Checks that error messages are formatted using the error color
        /// </summary>
        [Test]
        public void Format_ErrorLog_OutputUsesErrorColor()
        {
            Color expectedColor = Color.green;
            TMPLogColorFormatter formatter = new TMPLogColorFormatter()
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

        /// <summary>
        /// Checks that assert messages are formatted using the assert color
        /// </summary>
        [Test]
        public void Format_AssertLog_OutputUsesAssertColor()
        {
            Color expectedColor = Color.green;
            TMPLogColorFormatter formatter = new TMPLogColorFormatter()
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

        /// <summary>
        /// Checks that exception messages are formatted using the exception color
        /// </summary>
        [Test]
        public void Format_ExceptionLog_OutputUsesExceptionColor()
        {
            Color expectedColor = Color.green;
            TMPLogColorFormatter formatter = new TMPLogColorFormatter()
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

        /// <summary>
        /// Checks that warning messages are formatted using the warning color
        /// </summary>
        [Test]
        public void Format_WarningLog_OutputUsesWarningColor()
        {
            Color expectedColor = Color.green;
            TMPLogColorFormatter formatter = new TMPLogColorFormatter()
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

        /// <summary>
        /// Checks that the formatted output containts the message's output
        /// </summary>
        [Test]
        public void Format_OutputContainsContent()
        {
            Color expectedColor = Color.green;
            TMPLogColorFormatter formatter = new TMPLogColorFormatter()
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

        /// <summary>
        /// Checks that the formatted output contains the stack trace
        /// </summary>
        [Test]
        public void Format_OutputContainsStackTrace()
        {
            Color expectedColor = Color.green;
            TMPLogColorFormatter formatter = new TMPLogColorFormatter()
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

        /// <summary>
        /// Creates a fake log message for testing
        /// </summary>
        /// <param name="type">The type of the log message</param>
        /// <param name="content">The content of the log message</param>
        /// <param name="stackTrace">The stack trace of the log message</param>
        /// <returns></returns>
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
