using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class DefaultLogFormatter : ILogFormatter
{
    public string FormatString(Exception exception, UnityEngine.Object context)
    {
        return exception.ToString();
    }

    public string FormatString(LogType logType, UnityEngine.Object context, string format, params object[] args)
    {
        return logType.ToString() + format.ToString();
    }
}
