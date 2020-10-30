using UnityEngine;

namespace i5.Toolkit.Core.AppConsole
{
    public interface ILogMessage : INotificationMessage
    {
        string StackTrace { get; }

        LogType LogType { get; }
    }
}
