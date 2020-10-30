using UnityEngine;

namespace i5.Toolkit.Core.AppConsole
{
    public class TMPLogColorFormatterLogic : ILogColorFormatterLogic
    {
        public Color LogColor { get; set; } = Color.white;

        public Color WarningColor { get; set; } = Color.yellow;

        public Color ErrorColor { get; set; } = Color.red;

        public Color ExceptionColor { get; set; } = Color.cyan;

        public Color AssertColor { get; set; } = Color.cyan;

        public Color DefaultColor { get; set; } = Color.white;

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
