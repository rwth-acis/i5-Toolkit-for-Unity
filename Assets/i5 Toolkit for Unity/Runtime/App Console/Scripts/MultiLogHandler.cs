using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.AppConsole
{
    public class MultiLogHandler : List<ILogHandler>, ILogHandler
    {
        public void LogException(Exception exception, UnityEngine.Object context)
        {
            for (int i = 0; i < Count; i++)
            {
                this[i].LogException(exception, context);
            }
        }

        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            for (int i = 0; i < Count; i++)
            {
                this[i].LogFormat(logType, context, format, args);
            }
        }
    }
}