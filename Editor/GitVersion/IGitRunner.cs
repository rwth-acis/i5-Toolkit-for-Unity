using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i5.Toolkit.Core.GitVersion
{
    public interface IGitRunner
    {
        int RunCommand(string arguments, out string output, out string errors);
    }
}
