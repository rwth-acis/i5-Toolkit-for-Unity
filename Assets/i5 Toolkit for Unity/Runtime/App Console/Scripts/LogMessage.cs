using UnityEngine;

namespace i5.Toolkit.Core.AppConsole
{
    public class LogMessage : ILogMessage
    {
        public string Content { get; }

        public string StackTrace { get; }

        public LogType LogType { get; }

        public LogMessage(string content, string stackTrace, LogType logType)
        {
            Content = content;
            StackTrace = stackTrace;
            LogType = logType;
        }
    }
}