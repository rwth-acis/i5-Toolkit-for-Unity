using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i5.Toolkit.Core.ServiceCore
{
    public interface IRunnerReceiver
    {
        void Update();

        void OnRunnerDestroyed();
    }
}
