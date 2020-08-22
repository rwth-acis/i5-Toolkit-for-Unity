using System;
using UnityEngine;

public interface ILogFormatter
{
    string FormatString(Exception exception, UnityEngine.Object context);
    string FormatString(LogType logType, UnityEngine.Object context, string format, params object[] args);
}
