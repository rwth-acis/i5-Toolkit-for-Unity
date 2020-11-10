using UnityEngine;

namespace i5.Toolkit.Core.AppConsole
{
    /// <summary>
    /// The color log format configuration for TextMeshPro displays
    /// </summary>
    [CreateAssetMenu(fileName = "TMPLogFormatterConfiguration", menuName = "i5 Toolkit/Console Formatters/TextMeshPro Log Formatter")]
    public class TMPLogColorFormatterConfiguration : LogColorFormatterConfiguration
    {
        /// <summary>
        /// Generates a formatter for TextMeshPro configurers
        /// </summary>
        /// <returns>Returns the set up formatter</returns>
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