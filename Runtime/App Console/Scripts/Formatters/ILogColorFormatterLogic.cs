using UnityEngine;

namespace i5.Toolkit.Core.AppConsole
{
    public interface ILogColorFormatterLogic : ILogFormatterLogic
    {
        Color LogColor { get; set; }
        Color WarningColor { get; set; }
        Color ErrorColor { get; set; }
        Color ExceptionColor { get; set; }
        Color AssertColor { get; set; }
        Color DefaultColor { get; set; }
    }
}
