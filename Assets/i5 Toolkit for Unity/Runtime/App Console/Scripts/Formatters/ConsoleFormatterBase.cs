using UnityEngine;

namespace i5.Toolkit.Core.AppConsole
{
    /// <summary>
    /// A console formatter which can format messages in a specific way
    /// </summary>
    public abstract class ConsoleFormatterBase : ScriptableObject
    {
        /// <summary>
        /// Formats a given notification message to a string that can be displayed
        /// </summary>
        /// <param name="message">The message to format</param>
        /// <returns>The formatted message string</returns>
        public abstract string Format(ILogMessage message);
    }
}