using i5.Toolkit.ServiceCore;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DemoAsyncService : AsyncWorkerService<Operation<float>>
{
    protected override void AsyncOperation(Operation<float> operation)
    {
        base.AsyncOperation(operation);
        Debug.Log("Async Op started");
        Thread.Sleep(5000);
        Debug.Log("5 seconds elapsed");
        operation.status = OperationStatus.SUCCESS;
    }
}
