using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i5.Toolkit.Core.ServiceCore
{
    /// <summary>
    /// Contract which defines which methods a MonoBehaviour runner can access
    /// This can be used to provide MonoBehaviour events to non-MonoBehaviours
    /// </summary>
    public interface IRunnerReceiver
    {
        /// <summary>
        /// Called every frame by the runner
        /// </summary>
        void Update();

        /// <summary>
        /// Called to inform the receiver that the runner object is destroyed
        /// </summary>
        void OnRunnerDestroyed();
    }
}
