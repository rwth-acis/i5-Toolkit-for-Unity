using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class DeepLinkingService : IService
{
    private object[] registeredListeners;

    private Dictionary<string, InstancedMethod> paths = new Dictionary<string, InstancedMethod>();

    public DeepLinkingService(object[] registeredListeners)
    {
        this.registeredListeners = registeredListeners;
    }

    public void Initialize(IServiceManager owner)
    {
        ConstructMapping();

        Application.deepLinkActivated += OnDeepLinkActivated;
        if (!string.IsNullOrEmpty(Application.absoluteURL))
        {
            OnDeepLinkActivated(Application.absoluteURL);
        }
    }

    private void ConstructMapping()
    {
        foreach (object listener in registeredListeners)
        {
            MethodInfo[] methods = listener.GetType().GetMethods()
                .Where(m => m.GetCustomAttributes(typeof(DeepLinkAttribute), false).Length > 0)
                .ToArray();

            foreach (MethodInfo info in methods)
            {
                Attribute[] attributes = info.GetCustomAttributes(typeof(DeepLinkAttribute)).ToArray();
                foreach (DeepLinkAttribute attribute in attributes)
                {
                    paths.Add(attribute.Path.ToLower(), new InstancedMethod(listener, info));
                }
            }
        }

        foreach (string key in paths.Keys)
        {
            Debug.Log("Registered path " + key);
        }
    }

    public void Cleanup()
    {
        Application.deepLinkActivated -= OnDeepLinkActivated;
    }

    public void OnDeepLinkActivated(string deepLink)
    {
        Debug.Log("Got deep link for " + deepLink);

        Uri uri = new Uri(deepLink);

        InstancedMethod targetMethod = paths[uri.Authority.ToLower()];
        Dictionary<string, string> fragments = UriUtils.GetUriParameters(uri);
        ParameterInfo[] parameters = targetMethod.Method.GetParameters();

        // convert strings to objects
        object[] convertedArguments = new object[parameters.Length];
        for (int i = 0; i < parameters.Length; i++)
        {
            string value = fragments[parameters[i].Name];
            object convertedValue = Convert.ChangeType(value, parameters[i].ParameterType);
            convertedArguments[i] = convertedValue;
        }

        targetMethod.Method.Invoke(
            targetMethod.ClassInstance,
            convertedArguments);
    }
}
