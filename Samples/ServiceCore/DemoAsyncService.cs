using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace i5.Toolkit.Core.Examples.ServiceExample
{
    /// <summary>
    /// Demo for an asynchronous service
    /// It prints a statement, waits 5 seconds and then prints another statement
    /// </summary>
    public class DemoAsyncService : AsyncThreadedWorkerService<Operation<float>>
    {
        protected override void AsyncOperation(Operation<float> operation)
        {
            base.AsyncOperation(operation);
            i5Debug.Log("Async Op started", this);
            Thread.Sleep(5000);
            i5Debug.Log("5 seconds elapsed", this);
            operation.status = OperationStatus.SUCCESS;
        }
    }
}