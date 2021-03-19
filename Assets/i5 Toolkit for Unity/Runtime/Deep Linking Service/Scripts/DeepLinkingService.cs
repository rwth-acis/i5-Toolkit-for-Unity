using i5.Toolkit.Core.ServiceCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class DeepLinkingService : IService
{
    private List<object> registeredListeners = new List<object>();

    private Dictionary<string, Tuple<object, MethodInfo>> paths = new Dictionary<string, Tuple<object, MethodInfo>>();

    public void Initialize(IServiceManager owner)
    {
        Application.deepLinkActivated += OnDeepLinkActivated;
        if (!string.IsNullOrEmpty(Application.absoluteURL))
        {
            OnDeepLinkActivated(Application.absoluteURL);
        }

        ConstructMapping();
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
                    paths.Add(attribute.Path, new Tuple<object, MethodInfo>(listener, info));
                }
            }
        }
    }

    public void Cleanup()
    {
        Application.deepLinkActivated += OnDeepLinkActivated;
    }

    public void AddListenerClass(object obj)
    {
        registeredListeners.Add(obj);
    }

    public void RemoveListenerClass(object obj)
    {
        registeredListeners.Remove(obj);
    }

    public void OnDeepLinkActivated(string path)
    {
        // extract path and attributes
        paths[path].Item2.Invoke(paths[path].Item1, new object[0]);
    }
}
