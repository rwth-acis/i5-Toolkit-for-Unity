using i5.Toolkit.Core.DeepLinkAPI;
using NUnit.Framework;
using UnityEngine;

public class DeepLinkTestDefinition
{
    public int TimesWithoutParamsCalled { get; private set; } = 0;

    public int TimesWithParamsCalled { get; private set; } = 0;

    public DeepLinkArgs DeepLinkArgs { get; private set; }

    [DeepLink("withoutParams")]
    public void EmptyDeepLinkTargetPass()
    {
        Debug.Log("Method without parameters called");
        TimesWithoutParamsCalled++;
    }

    [DeepLink("withParams")]
    public void WithString(DeepLinkArgs args)
    {
        Debug.Log("Method with parameters called");
        TimesWithParamsCalled++;
        DeepLinkArgs = args;
    }

    [DeepLink("duplicate")]
    public void Duplicate()
    {
        TimesWithoutParamsCalled++;
    }

    [DeepLink("duplicate")]
    public void Duplicate2()
    {
        TimesWithoutParamsCalled++;
    }

    [DeepLink("duplicate")]
    public void Duplicate(DeepLinkArgs args)
    {
        TimesWithParamsCalled++;
    }

    [DeepLink("multiPaths")]
    [DeepLink("multiPaths2")]
    public void MultipePaths(DeepLinkArgs args)
    {
        DeepLinkArgs = args;
    }

    [DeepLink("faulty")]
    public void Faulty(string invalidParam)
    {
        TimesWithParamsCalled++;
    }
}
