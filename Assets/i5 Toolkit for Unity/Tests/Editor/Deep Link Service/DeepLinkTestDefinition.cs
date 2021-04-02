using i5.Toolkit.Core.DeepLinkAPI;
using NUnit.Framework;
using UnityEngine;

public class DeepLinkTestDefinition
{
    public int TimesWithoutParamsCalled { get; private set; } = 0;

    public int TimesWithParamsCalled { get; private set; } = 0;

    public string StringValue { get; private set; }
    public int IntValue { get; private set; }

    [DeepLink("withoutParams")]
    public void EmptyDeepLinkTargetPass()
    {
        Debug.Log("Method without parameters called");
        TimesWithoutParamsCalled++;
    }

    [DeepLink("withString")]
    public void WithString(string value)
    {
        Debug.Log("Method with string parameter called");
        TimesWithParamsCalled++;
        StringValue = value;
    }


    [DeepLink("multiParams")]
    public void MultiParams(string value1, int value2 = 100)
    {
        Debug.Log("Method with multiple parameters called");
        TimesWithParamsCalled++;
        StringValue = value1;
        IntValue = value2;
    }

    [DeepLink("duplicate")]
    public void Duplicate()
    {

    }

    [DeepLink("duplicate")]
    public void Duplicate2()
    {

    }

    [DeepLink("duplicate")]
    public void Duplicate(DeepLinkArgs args)
    {

    }

    [DeepLink("duplicate")]
    [DeepLink("duplicate2")]
    public void MultipePaths()
    {

    }
}
