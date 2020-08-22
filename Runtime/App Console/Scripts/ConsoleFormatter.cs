using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleFormatter : MonoBehaviour, ILogFormatter
{
    [SerializeField] private Color logColor;
    [SerializeField] private Color warningColor;
    [SerializeField] private Color errorColor;
    [SerializeField] private Color exceptionColor;

    public string FormatString(Exception exception, UnityEngine.Object context)
    {
        return exception.ToString();
    }

    public string FormatString(LogType logType, UnityEngine.Object context, string format, params object[] args)
    {
        return logType.ToString() + format;
    }
}
