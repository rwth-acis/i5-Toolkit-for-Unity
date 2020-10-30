using UnityEngine;

namespace i5.Toolkit.Core.AppConsole
{
    [CreateAssetMenu(fileName = "TMPLogFormatter", menuName = "i5 Toolkit/Console Formatters/TextMeshPro Log Formatter")]
    public class TextMeshProLogColorFormatter : LogColorFormatter
    {
        private TMPLogColorFormatterLogic tmpLogColorFormatter;

        public override string Format(ILogMessage message)
        {
            if (tmpLogColorFormatter == null)
            {
                tmpLogColorFormatter = new TMPLogColorFormatterLogic()
                {
                    LogColor = logColor,
                    WarningColor = warningColor,
                    ErrorColor = errorColor,
                    ExceptionColor = exceptionColor,
                    AssertColor = assertColor,
                    DefaultColor = Color.white
                };
            }

            return tmpLogColorFormatter.Format(message);
        }
    }
}