using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.AppConsole
{
    public static class LoggerExtensions
    {
        public static void AddLogHandler(this ILogger logger, ILogHandler logHandler)
        {
            if (logger.logHandler is MultiLogHandler)
            {
                ((MultiLogHandler)logger.logHandler).Add(logHandler);
            }
            else
            {
                MultiLogHandler newLogHandler = new MultiLogHandler();
                newLogHandler.Add(logger.logHandler);
                newLogHandler.Add(logHandler);
                logger.logHandler = newLogHandler;
            }
        }
    }
}