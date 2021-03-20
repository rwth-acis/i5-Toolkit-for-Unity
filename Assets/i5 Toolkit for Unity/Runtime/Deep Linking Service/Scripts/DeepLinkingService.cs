using i5.Toolkit.Core.ServiceCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class DeepLinkingService : IService
{
    private object[] registeredListeners;

    private Dictionary<string, Tuple<object, MethodInfo>> paths = new Dictionary<string, Tuple<object, MethodInfo>>();

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
                    paths.Add(attribute.Path.ToLower(), new Tuple<object, MethodInfo>(listener, info));
                }
            }
        }

        foreach(string key in paths.Keys)
        {
            Debug.Log("Registered path " + key);
        }
    }

    public void Cleanup()
    {
        Application.deepLinkActivated -= OnDeepLinkActivated;
    }

    public void OnDeepLinkActivated(string path)
    {
        Debug.Log("Got deep link for " + path);

        Uri uri = new Uri(path);

        // extract path and attributes
        paths[uri.Authority.ToLower()].Item2.Invoke(paths[uri.Authority.ToLower()].Item1, new object[0]);
    }
}
