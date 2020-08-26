using i5.Toolkit.Core.ServiceCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LogNotificationAdapter
{
    public INotificationMessage FromLog(string condition, string stackTrace, LogType type)
    {
        INotificationMessage logMessage = new LogMessage(condition, stackTrace, type);
        return logMessage;
    }
}

