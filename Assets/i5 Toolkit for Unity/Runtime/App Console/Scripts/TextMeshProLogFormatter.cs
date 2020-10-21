using UnityEngine;

namespace i5.Toolkit.Core.AppConsole
{
    [CreateAssetMenu(fileName = "TMPLogFormatter", menuName = "i5 Toolkit/Console Formatters/TextMeshPro Log Formatter")]
    public class TextMeshProLogFormatter : LogFormatter
    {
        public override string Format(INotificationMessage message)
        {
            if (message is LogMessage)
            {
                LogMessage logMessage = (LogMessage)message;
                Color selectedColor;
                switch (logMessage.LogType)
                {
                    case LogType.Log:
                        selectedColor = logColor;
                        break;
                    case LogType.Warning:
                        selectedColor = warningColor;
                        break;
                    case LogType.Error:
                        selectedColor = errorColor;
                        break;
                    case LogType.Exception:
                        selectedColor = exceptionColor;
                        break;
                    case LogType.Assert:
                        selectedColor = assertColor;
                        break;
                    default:
                        selectedColor = Color.white;
                        break;
                }
                return $"<color=#{ColorUtility.ToHtmlStringRGB(selectedColor)}><b>{logMessage.LogType}</b>: " +
                    $"{logMessage.Content}\n<size=80%>{logMessage.StackTrace}</size></color>";
            }
            else
            {
                return "LOG FORMAT ERROR: This is not a log message";
            }
        }
    }
}