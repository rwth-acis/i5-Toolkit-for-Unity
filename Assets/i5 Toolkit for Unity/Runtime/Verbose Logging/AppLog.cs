using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

namespace i5.Toolkit.Core.VerboseLogging
{
	public static class AppLog
	{
		public static LogLevel LogLevel { get; set; } = LogLevel.TRACE;

		public static bool UseColors { get; set; } = true;

		public static void LogCritical(string message, Object context = null)
		{
			Log(message, LogLevel.CRITICAL, context);
		}

		public static void LogError(string message, Object context = null)
		{
			Log(message, LogLevel.ERROR, context);
		}

		public static void LogException(System.Exception e, Object context = null)
		{
			Log(e.ToString(), LogLevel.ERROR, context);
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
			if (level <= LogLevel)
			{
				string output = $"[{level.ToString()}] {message}";

#if UNITY_EDITOR
				if (UseColors)
				{
					string color = "black";
					switch (level)
					{
						case LogLevel.CRITICAL:
							color = "#ff00ff";
							break;
						case LogLevel.ERROR:
							color = "red";
							break;
						case LogLevel.WARNING:
							color = "#f7bc3b";
							break;
						case LogLevel.INFO:
							color = "white";
							break;
						case LogLevel.DEBUG:
							color = "#99f73b";
							break;
						case LogLevel.TRACE:
							color = "#3b99f7";
							break;
					}
					output = $"<color={color}>{output}</color>";
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