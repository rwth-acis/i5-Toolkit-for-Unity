using i5.Toolkit.Core.ServiceCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepLinkTester : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            ServiceManager.GetService<DeepLinkingService>().OnDeepLinkActivated("i5://helloWorld");
        }
    }
}
