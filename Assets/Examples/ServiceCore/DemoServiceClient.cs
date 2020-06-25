using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoServiceClient : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            i5Debug.Log(ServiceManager.GetService<DemoService>().GetDemoMessage(), this);
            Operation<float> op = new Operation<float>(CallbackResult);
            ServiceManager.GetService<DemoAsyncService>().AddOperation(op);
        }
    }

    private void CallbackResult(Operation<float> finishedOperation)
    {
        if (finishedOperation.status == OperationStatus.SUCCESS)
        {
            i5Debug.Log("Result of operation: " + finishedOperation.result, this);
        }
    }
}
