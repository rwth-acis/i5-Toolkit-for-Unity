using UnityEngine;

namespace i5.Toolkit.Core.AppConsole
{
    /// <summary>
    /// Log formatter that uses color coding
    /// </summary>
    public interface ILogColorFormatter : ILogFormatter
    {
        /// <summary>
        /// The color for log messages
        /// </summary>
        Color LogColor { get; set; }
        /// <summary>
        /// The color for warning messages
        /// </summary>
        Color WarningColor { get; set; }
        /// <summary>
        /// The color for error messages
        /// </summary>
        Color ErrorColor { get; set; }
        /// <summary>
        /// The color for exception messages
        /// </summary>
        Color ExceptionColor { get; set; }
        /// <summary>
        /// The color for assert messages
        /// </summary>
        Color AssertColor { get; set; }
        /// <summary>
        /// The color for unexpected message types
        /// </summary>
        Color DefaultColor { get; set; }
    }
}
