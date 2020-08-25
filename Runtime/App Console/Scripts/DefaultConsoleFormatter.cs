using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DefaultConsoleFormatter : ConsoleFormatterBase
{
    public override string Format(INotificationMessage message)
    {
        return message.Content;
    }
}
