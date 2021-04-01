using i5.Toolkit.Core.DeepLinkAPI;
using NUnit.Framework;
using UnityEngine;

public class DeepLinkTestDefinition
{
    [DeepLink("passWithoutParams")]
    public void EmptyDeepLinkTargetPass()
    {
        Debug.Log("Pass without parameters");
    }
}
