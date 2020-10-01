using UnityEngine;

public class LogMessage : INotificationMessage
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
