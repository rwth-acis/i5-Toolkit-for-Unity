using UnityEngine;

namespace i5.Toolkit.Core.AppConsole
{
    /// <summary>
    /// Color-coded log formatter for TextMeshPro
    /// </summary>
    public class TMPLogColorFormatter : ILogColorFormatter
    {
        /// <summary>
        /// The color for log messages
        /// </summary>
        public Color LogColor { get; set; } = Color.white;

        /// <summary>
        /// The color for warning messages
        /// </summary>
        public Color WarningColor { get; set; } = Color.yellow;

        /// <summary>
        /// The color for error messages
        /// </summary>
        public Color ErrorColor { get; set; } = Color.red;

        /// <summary>
        /// The color for exception messages
        /// </summary>
        public Color ExceptionColor { get; set; } = Color.cyan;

        /// <summary>
        /// The color for assert messages
        /// </summary>
        public Color AssertColor { get; set; } = Color.cyan;

        /// <summary>
        /// The color for unexpected message types
        /// </summary>
        public Color DefaultColor { get; set; } = Color.white;

        /// <summary>
        /// Formats the given log message to text output
        /// </summary>
        /// <param name="message">The message to format</param>
        /// <returns>Returns formatted text output based on the log message</returns>
        public string Format(ILogMessage message)
        {
            Color selectedColor;
            switch (message.LogType)
            {
                case LogType.Log:
                    selectedColor = LogColor;
                    break;
                case LogType.Warning:
                    selectedColor = WarningColor;
                    break;
                case LogType.Error:
                    selectedColor = ErrorColor;
                    break;
                case LogType.Exception:
                    selectedColor = ExceptionColor;
                    break;
                case LogType.Assert:
                    selectedColor = AssertColor;
                    break;
                default:
                    selectedColor = Color.white;
                    break;
            }
            return $"<color=#{ColorUtility.ToHtmlStringRGB(selectedColor)}><b>{message.LogType}</b>: " +
                $"{message.Content}\n<size=80%>{message.StackTrace}</size></color>";
        }
    }
}
