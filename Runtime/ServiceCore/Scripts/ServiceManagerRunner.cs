using UnityEngine;

namespace i5.Toolkit.Core.ServiceCore
{
    /// <summary>
    /// The runner which provides MonoBehaviour events to an IRunnerReceiver
    /// </summary>
    public class ServiceManagerRunner : MonoBehaviour
    {
        /// <summary>
        /// The IRunnerReceiver which should receive the MonoBehaviour
        /// </summary>
        private IRunnerReceiver runnerReceiver;

        /// <summary>
        /// Initializes the runner by assigning the runner receiver
        /// </summary>
        /// <param name="runnerReceiver">The runner receiver that should receive the MonoBehaviour events</param>
        public void Initialize(IRunnerReceiver runnerReceiver)
        {
            this.runnerReceiver = runnerReceiver;
        }

        private void Update()
        {
            runnerReceiver.Update();
        }

        private void OnDestroy()
        {
            runnerReceiver.OnRunnerDestroyed();
        }
    }
}