using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.Examples.ServiceExample
{
    /// <summary>
    /// A demo client which shows how to access services
    /// If the tester presses F5, a demo message is logged using hte DemoService
    /// and an operation on the DemoAsyncService is triggered
    /// </summary>
    public class DemoServiceClient : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                i5Debug.Log(ServiceManager.GetService<DemoService>().GetDemoMessage(), this);
                Operation<float> op = new Operation<float>(CallbackResult);
                ServiceManager.GetService<DemoAsyncService>().AddOperation(op);
            }
        }

        /// <summary>
        /// Called by the DemoAsyncService once the operation is finished
        /// Shows the result of the async operation
        /// </summary>
        /// <param name="finishedOperation">The operation that was finished</param>
        private void CallbackResult(Operation<float> finishedOperation)
        {
            if (finishedOperation.status == OperationStatus.SUCCESS)
            {
                i5Debug.Log("Result of operation: " + finishedOperation.result, this);
            }
        }
    }
}