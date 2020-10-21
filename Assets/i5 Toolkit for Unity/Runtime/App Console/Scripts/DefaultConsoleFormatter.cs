using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i5.Toolkit.Core.SceneConsole
{
    public class DefaultConsoleFormatter : ConsoleFormatterBase
    {
        public override string Format(INotificationMessage message)
        {
            return message.Content;
        }
    }
}