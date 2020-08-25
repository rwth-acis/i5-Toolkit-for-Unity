using i5.Toolkit.Core.ServiceCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LogNotificationAdapter
{
    private bool isSubscribed;
    private bool currentlySubscribed;

    private INotificationMessageReceiver receiver;

    public LogNotificationAdapter(INotificationMessageReceiver receiver)
    {
        this.receiver = receiver;
    }

    public bool IsSubscribed
    {
        get { return isSubscribed; }
        set
        {
            isSubscribed = value;
            if (isSubscribed && !currentlySubscribed)
            {
                SubscribeToUnityLog();
            }
            else if (!isSubscribed && currentlySubscribed)
            {
                UnsubscribeFromUnityLog();
            }
        }
    }

    private void SubscribeToUnityLog()
    {
        Application.logMessageReceived += Application_logMessageReceived;
        currentlySubscribed = true;
    }

    private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
    {
        INotificationMessage logMessage = new LogMessage(condition, stackTrace, type);
        receiver.ReceiveNotification(logMessage);
    }

    private void UnsubscribeFromUnityLog()
    {
        Application.logMessageReceived -= Application_logMessageReceived;
        currentlySubscribed = false;
    }
}

