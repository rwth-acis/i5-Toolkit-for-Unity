using UnityEngine;

namespace i5.Toolkit.Core.ServiceCore
{
    public class ServiceManagerRunner : MonoBehaviour
    {
        private IRunnerReceiver runnerReceiver;

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