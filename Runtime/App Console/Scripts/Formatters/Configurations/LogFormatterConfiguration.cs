using UnityEngine;

namespace i5.Toolkit.Core.AppConsole
{
    /// <summary>
    /// A console formatter which can format messages in a specific way
    /// </summary>
    public abstract class LogFormatterConfiguration : ScriptableObject
    {
        public abstract ILogFormatter GenerateFormatter();
    }
}