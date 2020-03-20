using i5.Toolkit.ServiceCore;
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
            Debug.Log(ServiceManager.GetService<DemoService>().GetDemoMessage());
        }
    }
}
