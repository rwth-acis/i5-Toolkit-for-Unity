using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

namespace i5.Toolkit.Core.VerboseLogging
{
	public static class AppLog
	{
		public static LogLevel MinimumLogLevel { get; set; } = LogLevel.TRACE;

		public static bool UseColors { get; set; } = true;

		public static Color CriticalColor { get; set; } = Color.magenta;
		public static Color ErrorColor { get; set; } = Color.red;
		public static Color WarningColor { get; set; } = new Color(0.97f, 0.74f, 0.23f);
		public static Color InfoColor { get; set; } = Color.white;
		public static Color DebugColor { get; set; } = new Color(0.6f, 0.97f, 0.23f);
		public static Color TraceColor { get; set; } = new Color(0.23f, 0.6f, 0.97f);

		public static void LogCritical(string message, Object context = null)
		{
			Log(message, LogLevel.CRITICAL, context);
		}

		public static void LogError(string message, Object context = null)
		{
			Log(message, LogLevel.ERROR, context);
		}

		public static void LogException(System.Exception e, bool isCritical= false, Object context = null)
		{
			LogLevel level = LogLevel.ERROR;
			if (isCritical)
			{
				level = LogLevel.CRITICAL;
			}
			Log(e.ToString(), level, context);
		}

		public static void LogWarning(string message, Object context = null)
		{
			Log(message, LogLevel.WARNING, context);
		}

		public static void LogInfo(string message, Object context = null)
		{
			Log(message, LogLevel.INFO, context);
		}

		public static void LogDebug(string message, Object context = null)
		{
			Log(message, LogLevel.DEBUG, context);
		}

		public static void LogTrace(string message, Object context = null)
		{
			Log(message, LogLevel.TRACE, context);
		}

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