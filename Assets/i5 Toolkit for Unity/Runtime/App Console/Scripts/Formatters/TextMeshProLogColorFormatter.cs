using UnityEngine;

namespace i5.Toolkit.Core.AppConsole
{
    [CreateAssetMenu(fileName = "TMPLogFormatter", menuName = "i5 Toolkit/Console Formatters/TextMeshPro Log Formatter")]
    public class TextMeshProLogColorFormatter : LogColorFormatter
    {
        public override string Format(ILogMessage message)
        {
            if (logFormatterLogic == null)
            {
                logFormatterLogic = new TMPLogColorFormatterLogic()
                {
                    LogColor = logColor,
                    WarningColor = warningColor,
                    ErrorColor = errorColor,
                    ExceptionColor = exceptionColor,
                    AssertColor = assertColor,
                    DefaultColor = Color.white
                };
            }

            return logFormatterLogic.Format(message);
        }
    }
}