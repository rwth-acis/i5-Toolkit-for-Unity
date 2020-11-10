using System;
using System.Collections.Generic;

namespace i5.Toolkit.Core.AppConsole
{
    /// <summary>
    /// Console for capturing messages
    /// </summary>
    public interface IConsole
    {
        /// <summary>
        /// List of captured messages
        /// </summary>
        List<ILogMessage> Messages { get; }

        /// <summary>
        /// If true, the console is capturing messages
        /// </summary>
        bool IsCapturing { get; set; }

        /// <summary>
        /// Event which is invoked if a message was captured
        /// </summary>
        event Action OnMessageAdded;
    }
}
