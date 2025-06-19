using i5.Toolkit.Core.DeepLinkAPI;
using UnityEngine;
using UnityEngine.Scripting;

namespace i5.Toolkit.Core.Examples.DeepLinkAPI
{
    public class DeepLinkReceiver
    {
        [Preserve]
        [DeepLink("helloWorld")]
        public void HelloWorld()
        {
            Debug.Log($"Hello World");
        }
    }
}