using i5.Toolkit.Core.Experimental.NotificationSystem;
using UnityEngine;

namespace i5.Toolkit.Core.AppConsole
{
    /// <summary>
    /// Log message
    /// </summary>
    public interface ILogMessage : INotificationMessage
    {
        /// <summary>
        /// The stack trace of the log message
        /// </summary>
        string StackTrace { get; }

        /// <summary>
        /// The type of log message
        /// </summary>
        LogType LogType { get; }
    }
}
