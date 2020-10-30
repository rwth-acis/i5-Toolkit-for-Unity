using UnityEngine;

namespace i5.Toolkit.Core.AppConsole
{
    [CreateAssetMenu(fileName = "TMPLogFormatterConfiguration", menuName = "i5 Toolkit/Console Formatters/TextMeshPro Log Formatter")]
    public class TMPLogColorFormatterConfiguration : LogColorFormatterConfiguration
    {
        public override ILogFormatter GenerateFormatter()
        {
            return new TMPLogColorFormatter()
            {
                LogColor = logColor,
                WarningColor = warningColor,
                ErrorColor = errorColor,
                ExceptionColor = exceptionColor,
                AssertColor = assertColor,
                DefaultColor = Color.white
            };
        }
    }
}