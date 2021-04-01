using i5.Toolkit.Core.DeepLinkAPI;
using i5.Toolkit.Core.ServiceCore;
using UnityEngine;

public class DeepLinkTester : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            //ServiceManager.GetService<DeepLinkingService>().OnDeepLinkActivated("i5://helloWorld?x=42&check=true");
        }
    }
}
