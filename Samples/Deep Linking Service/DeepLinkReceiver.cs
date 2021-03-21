using UnityEngine;

public class DeepLinkReceiver
{
    [DeepLink("helloWorld")]
    public void HelloWorld(int x, bool check)
    {
        Debug.Log($"Hello World; x={x}; check={check}");
    }
}
