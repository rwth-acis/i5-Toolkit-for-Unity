using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i5.Toolkit.Core.AppConsole
{
    public class DefaultConsoleFormatter : ConsoleFormatterBase
    {
        public override string Format(ILogMessage message)
        {
            if (logFormatterLogic == null)
            {
                logFormatterLogic = new DefaultConsoleFormatterLogic();
            }
            return base.Format(message);
        }
    }
}