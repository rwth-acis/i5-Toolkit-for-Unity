using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i5.Toolkit.Core.AppConsole
{
    public interface IConsole
    {
        List<ILogMessage> Messages { get; }

        bool IsCapturing { get; set; }

        event Action OnMessageAdded;
    }
}
