using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

namespace i5.Toolkit.Core.VerboseLogging
{
	/// <summary>
	/// Logging class which applies a verbosity filter to all logs which are produced with it
	/// </summary>
	public static class AppLog
	{
		/// <summary>
		/// The minimum importance level of logs which will be ouput.
		/// All logs with a lower importance level are ignored.
		/// </summary>
		public static LogLevel MinimumLogLevel { get; set; } = LogLevel.TRACE;

		/// <summary>
		/// If true, the module will style messages in the editor with colors
		/// </summary>
		public static bool UseColors { get; set; } = true;

		/// <summary>
		/// The color which should be applied to critical log messags if UseColors is true
		/// </summary>
		public static Color CriticalColor { get; set; } = Color.magenta;
        /// <summary>
        /// The color which should be applied to error log messags if UseColors is true
        /// </summary>
        public static Color ErrorColor { get; set; } = Color.red;
        /// <summary>
        /// The color which should be applied to warning log messags if UseColors is true
        /// </summary>
        public static Color WarningColor { get; set; } = new Color(0.97f, 0.74f, 0.23f);
        /// <summary>
        /// The color which should be applied to info log messags if UseColors is true
        /// </summary>
        public static Color InfoColor { get; set; } = Color.white;
        /// <summary>
        /// The color which should be applied to debug log messags if UseColors is true
        /// </summary>
        public static Color DebugColor { get; set; } = new Color(0.6f, 0.97f, 0.23f);
        /// <summary>
        /// The color which should be applied to trace log messags if UseColors is true
        /// </summary>
        public static Color TraceColor { get; set; } = new Color(0.23f, 0.6f, 0.97f);

		/// <summary>
		/// Logs a critical error message
		/// </summary>
		/// <param name="message">The message to log</param>
		/// <param name="context">The context of the message</param>
		public static void LogCritical(string message, Object context = null)
		{
			Log(message, LogLevel.CRITICAL, context);
		}

        /// <summary>
        /// Logs an error message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="context">The context of the message</param>
        public static void LogError(string message, Object context = null)
		{
			Log(message, LogLevel.ERROR, context);
		}

		/// <summary>
		/// Logs an exception either as an error message or a critical error message
		/// </summary>
		/// <param name="e">The exception which should be logged</param>
		/// <param name="isCritical">If set to true, the message will be logged as a critical error</param>
		/// <param name="context">The context of the message</param>
		public static void LogException(System.Exception e, bool isCritical= false, Object context = null)
		{
			LogLevel level = LogLevel.ERROR;
			if (isCritical)
			{
				level = LogLevel.CRITICAL;
			}
			Log(e.ToString(), level, context);
		}

        /// <summary>
        /// Logs a warning message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="context">The context of the message</param>
        public static void LogWarning(string message, Object context = null)
		{
			Log(message, LogLevel.WARNING, context);
		}

        /// <summary>
        /// Logs an info message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="context">The context of the message</param>
        public static void LogInfo(string message, Object context = null)
		{
			Log(message, LogLevel.INFO, context);
		}

        /// <summary>
        /// Logs a message for debugging the code
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="context">The context of the message</param>
        public static void LogDebug(string message, Object context = null)
		{
			Log(message, LogLevel.DEBUG, context);
		}

        /// <summary>
        /// Logs a message which can be used to trace the code execution path
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="context">The context of the message</param>
        public static void LogTrace(string message, Object context = null)
		{
			Log(message, LogLevel.TRACE, context);
		}

		/// <summary>
		/// Logs a message with a given lelve
		/// </summary>
		/// <param name="message">The message to log</param>
		/// <param name="level">The importance level of the message</param>
		/// <param name="context">The context of the message</param>
		public static void Log(string message, LogLevel level, Object context = null)
		{
			if (level <= MinimumLogLevel)
			{
				string output = $"[{level}] {message}";

#if UNITY_EDITOR
				if (UseColors)
				{
					string color = "black";
					switch (level)
					{
						case LogLevel.CRITICAL:
							color = $"#{ColorUtility.ToHtmlStringRGB(CriticalColor)}";
							break;
						case LogLevel.ERROR:
							color = $"#{ColorUtility.ToHtmlStringRGB(ErrorColor)}";
							break;
						case LogLevel.WARNING:
							color = $"#{ColorUtility.ToHtmlStringRGB(WarningColor)}";
                            break;
						case LogLevel.INFO:
							color = $"#{ColorUtility.ToHtmlStringRGB(InfoColor)}";
                            break;
						case LogLevel.DEBUG:
							color = $"#{ColorUtility.ToHtmlStringRGB(DebugColor)}";
                            break;
						case LogLevel.TRACE:
							color = $"#{ColorUtility.ToHtmlStringRGB(TraceColor)}";
							break;
					}
                    output = $"<color={color}>[{level}]</color> {message}";
				}
#endif

				if (level == LogLevel.CRITICAL || level == LogLevel.ERROR)
				{
					Debug.LogError(output, context);
				}
				else if (level == LogLevel.WARNING)
				{
					Debug.LogWarning(output, context);
				}
				else
				{
					Debug.Log(output, context);
				}
			}
		}
	}
}