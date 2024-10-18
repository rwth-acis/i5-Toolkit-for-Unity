using i5.Toolkit.Core.DeepLinkAPI;
using UnityEngine;

namespace i5.Toolkit.Core.Examples.DeepLinkAPI
{
    public class DeepLinkReceiver
    {
        [DeepLink("helloWorld")]
        public void HelloWorld()
        {
            Debug.Log($"Hello World");
        }
    }
}