using UnityEngine;

public class DeepLinkReceiver
{
    [DeepLink("helloWorld")]
    public void HelloWorld()
    {
        Debug.Log("Hello World");
    }
}
