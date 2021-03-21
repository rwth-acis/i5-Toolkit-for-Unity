using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class InstancedMethod
{
    public object ClassInstance { get; private set; }
    public MethodInfo Method { get; private set; }

    public InstancedMethod(object classInstance, MethodInfo method)
    {
        ClassInstance = classInstance;
        Method = method;
    }
}
