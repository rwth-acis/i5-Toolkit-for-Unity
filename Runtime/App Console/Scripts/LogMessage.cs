using UnityEngine;

namespace i5.Toolkit.Core.AppConsole
{
    /// <summary>
    /// Log message as posted by the Unity API
    /// </summary>
    public class LogMessage : ILogMessage
    {
        /// <summary>
        /// Content of the log message
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// Stack trace of the log message
        /// </summary>
        public string StackTrace { get; }

        /// <summary>
        /// Type of the log message
        /// </summary>
        public LogType LogType { get; }

        /// <summary>
        /// Creates a new log message instance
        /// </summary>
        /// <param name="content">The content of the log message</param>
        /// <param name="stackTrace">The stack trace of the log message</param>
        /// <param name="logType">The typ of log message</param>
        public LogMessage(string content, string stackTrace, LogType logType)
        {
            Content = content;
            StackTrace = stackTrace;
            LogType = logType;
        }
    }
}