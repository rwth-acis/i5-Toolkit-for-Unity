using UnityEngine;

namespace i5.Toolkit.Core.AppConsole
{
    /// <summary>
    /// A console formatter configuration object which can be set up as a scriptable object
    /// </summary>
    public abstract class LogFormatterConfiguration : ScriptableObject
    {
        /// <summary>
        /// Creates a formatter instance which can be used to format messages
        /// </summary>
        /// <returns>Returns the formatter instance with the set up configuration</returns>
        public abstract ILogFormatter GenerateFormatter();
    }
}