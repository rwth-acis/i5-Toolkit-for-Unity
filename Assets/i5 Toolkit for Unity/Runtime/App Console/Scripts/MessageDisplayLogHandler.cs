using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageDisplayLogHandler : ILogHandler
{
    IMessageDisplay messageDisplay;
    ILogFormatter messageFormatter;

    public MessageDisplayLogHandler(IMessageDisplay messageDisplay, ILogFormatter messageFormatter = null)
    {
        this.messageDisplay = messageDisplay;
        if (messageFormatter == null)
        {
            this.messageFormatter = new DefaultLogFormatter();
        }
        else
        {
            this.messageFormatter = messageFormatter;
        }
    }

    public void LogException(Exception exception, UnityEngine.Object context)
    {
        string message = messageFormatter.FormatString(exception, context);
        messageDisplay.AddMessage(message);
    }

    public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
    {
        string message = messageFormatter.FormatString(logType, context, format, args);
        messageDisplay.AddMessage(message);
    }
}
