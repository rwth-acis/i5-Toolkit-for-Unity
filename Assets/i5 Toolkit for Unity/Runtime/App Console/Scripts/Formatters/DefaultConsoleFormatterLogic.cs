using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i5.Toolkit.Core.AppConsole
{
    public class DefaultConsoleFormatterLogic : ILogFormatterLogic
    {
        public string Format(ILogMessage logMessage)
        {
            return logMessage.Content;
        }
    }
}
