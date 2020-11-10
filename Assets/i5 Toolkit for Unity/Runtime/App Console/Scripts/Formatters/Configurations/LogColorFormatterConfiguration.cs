using UnityEngine;

namespace i5.Toolkit.Core.AppConsole
{
    /// <summary>
    /// A configuration object for formatting log messages using colors
    /// </summary>
    public abstract class LogColorFormatterConfiguration : LogFormatterConfiguration
    {
        [Tooltip("The color which is used for log messages")]
        [SerializeField] protected Color logColor = Color.white;
        [Tooltip("The color which is used for warning messages")]
        [SerializeField] protected Color warningColor = Color.yellow;
        [Tooltip("The color which is used for error messages")]
        [SerializeField] protected Color errorColor = Color.red;
        [Tooltip("The color which is used for exception messages")]
        [SerializeField] protected Color exceptionColor = Color.red;
        [Tooltip("The color which is used for assert messages")]
        [SerializeField] protected Color assertColor = Color.red;
    }
}