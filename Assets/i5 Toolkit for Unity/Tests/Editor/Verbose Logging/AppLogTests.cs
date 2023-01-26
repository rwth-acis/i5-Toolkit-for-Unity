using i5.Toolkit.Core.Editor.TestHelpers;
using i5.Toolkit.Core.VerboseLogging;
using NUnit.Framework;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Core.Tests.VerboseLogging
{
    public class AppLogTests
    {
        [SetUp]
        public void ResetScene()
        {
            EditModeTestUtilities.ResetScene();
            AppLog.MinimumLogLevel = LogLevel.TRACE;
            AppLog.UseColors = true;
        }

        private void CheckMessageExists(LogLevel logLevel, LogType expectedlogType, string message)
        {
            LogAssert.Expect(expectedlogType, new Regex(@".*" + message + @".*"));
            AppLog.Log(message, logLevel, null);
        }

        private void CheckLevelPrinted(LogLevel logLevel, LogType expectedLogType, string message)
        {
            LogAssert.Expect(expectedLogType, new Regex(@".*" + logLevel.ToString() + @".*"));
            AppLog.Log(message, logLevel, null);
        }

        [Test]
        public void Log_Critical_LogsError()
        {
            CheckMessageExists(LogLevel.CRITICAL, LogType.Error, "This is a message");
        }

        [Test]
        public void Log_Error_LogsError()
        {
            CheckMessageExists(LogLevel.ERROR, LogType.Error, "This is a message");
        }

        [Test]
        public void Log_Warning_LogsWarning()
        {
            CheckMessageExists(LogLevel.WARNING, LogType.Warning, "This is a message");
        }

        [Test]
        public void Log_Info_LogsMessage()
        {
            CheckMessageExists(LogLevel.INFO, LogType.Log, "This is a message");
        }

        [Test]
        public void Log_Debug_LogsMessage()
        {
            CheckMessageExists(LogLevel.DEBUG, LogType.Log, "This is a message");
        }

        [Test]
        public void Log_Trace_LogsMessage()
        {
            CheckMessageExists(LogLevel.TRACE, LogType.Log, "This is a message");
        }

        [Test]
        public void Log_UnderMinimumLogLevel_DoesNotLog()
        {
            AppLog.MinimumLogLevel = LogLevel.CRITICAL;
            AppLog.Log("This is a message", LogLevel.ERROR, null);
            // passes if no error message is logged; automatically fails if an unexpected error is logged
        }

        [Test]
        public void LogCritical_LogsError()
        {
            LogAssert.Expect(LogType.Error, new Regex(@".*\[CRITICAL\].*This is a message.*"));
            AppLog.LogCritical("This is a message");
        }

        [Test]
        public void LogError_LogsError()
        {
            LogAssert.Expect(LogType.Error, new Regex(@".*\[ERROR\].*This is a message.*"));
            AppLog.LogError("This is a message");
        }

        [Test]
        public void LogWarning_LogsWarning()
        {
            LogAssert.Expect(LogType.Warning, new Regex(@".*\[WARNING\].*This is a message.*"));
            AppLog.LogWarning("This is a message");
        }

        [Test]
        public void LogInfo_LogsMessage()
        {
            LogAssert.Expect(LogType.Log, new Regex(@".*\[INFO\].*This is a message.*"));
            AppLog.LogInfo("This is a message");
        }

        [Test]
        public void LogDebug_LogsMessage()
        {
            LogAssert.Expect(LogType.Log, new Regex(@".*\[DEBUG\].*This is a message.*"));
            AppLog.LogDebug("This is a message");
        }

        [Test]
        public void LogTrace_LogsMessage()
        {
            LogAssert.Expect(LogType.Log, new Regex(@".*\[TRACE\].*This is a message.*"));
            AppLog.LogTrace("This is a message");
        }

        [Test]
        public void LogException_NotCritical_LogsError()
        {
            Exception e = new Exception("This is an exception");
            LogAssert.Expect(LogType.Error, new Regex(@".*\[ERROR\].*This is an exception.*"));
            AppLog.LogException(e, false);
        }

        [Test]
        public void LogException_Critical_LogsError()
        {
            Exception e = new Exception("This is an exception");
            LogAssert.Expect(LogType.Error, new Regex(@".*\[CRITICAL\].*This is an exception.*"));
            AppLog.LogException(e, true);
        }

        [Test]
        public void Log_Critical_LogsLevel()
        {
            CheckLevelPrinted(LogLevel.CRITICAL, LogType.Error, "This is a message");
        }

        [Test]
        public void Log_Error_LogsLevel()
        {
            CheckLevelPrinted(LogLevel.ERROR, LogType.Error, "This is a message");
        }

        [Test]
        public void Log_Warning_LogsLevel()
        {
            CheckLevelPrinted(LogLevel.WARNING, LogType.Warning, "This is a message");
        }

        [Test]
        public void Log_Info_LogsLevel()
        {
            CheckLevelPrinted(LogLevel.INFO, LogType.Log, "This is a message");
        }

        [Test]
        public void Log_Debug_LogsLevel()
        {
            CheckLevelPrinted(LogLevel.DEBUG, LogType.Log, "This is a message");
        }

        [Test]
        public void Log_Trace_LogsLevel()
        {
            CheckLevelPrinted(LogLevel.TRACE, LogType.Log, "This is a message");
        }

        [Test]
        public void UseColors_True_DefinesColor()
        {
            LogAssert.Expect(LogType.Log, new Regex(@".*color.*"));
            AppLog.LogTrace("This is a message");
        }

        [Test]
        public void UsesColors_False_NoColorInfo()
        {
            AppLog.UseColors = false;
            LogAssert.Expect(LogType.Log, new Regex(@"^(?!.*color).*$"));
            AppLog.LogTrace("This is a message");
        }

        [Test]
        public void Log_Critical_ColorApplied()
        {
            LogAssert.Expect(LogType.Error, new Regex(@".*#" + ColorUtility.ToHtmlStringRGB(AppLog.CriticalColor) + @".*"));
            AppLog.Log("This is a message", LogLevel.CRITICAL);
        }

        [Test]
        public void Log_Error_ColorApplied()
        {
            LogAssert.Expect(LogType.Error, new Regex(@".*#" + ColorUtility.ToHtmlStringRGB(AppLog.ErrorColor) + @".*"));
            AppLog.Log("This is a message", LogLevel.ERROR);
        }

        [Test]
        public void Log_Warning_ColorApplied()
        {
            LogAssert.Expect(LogType.Warning, new Regex(@".*#" + ColorUtility.ToHtmlStringRGB(AppLog.WarningColor) + @".*"));
            AppLog.Log("This is a message", LogLevel.WARNING);
        }

        [Test]
        public void Log_Info_ColorApplied()
        {
            LogAssert.Expect(LogType.Log, new Regex(@".*#" + ColorUtility.ToHtmlStringRGB(AppLog.InfoColor) + @".*"));
            AppLog.Log("This is a message", LogLevel.INFO);
        }

        [Test]
        public void Log_Debug_ColorApplied()
        {
            LogAssert.Expect(LogType.Log, new Regex(@".*#" + ColorUtility.ToHtmlStringRGB(AppLog.DebugColor) + @".*"));
            AppLog.Log("This is a message", LogLevel.DEBUG);
        }

        [Test]
        public void Log_Trace_ColorApplied()
        {
            LogAssert.Expect(LogType.Log, new Regex(@".*#" + ColorUtility.ToHtmlStringRGB(AppLog.TraceColor) + @".*"));
            AppLog.Log("This is a message", LogLevel.TRACE);
        }
    }
}
