using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities;
using i5.Toolkit.Core.Utilities.UnityAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace i5.Toolkit.Core.DeepLinkAPI
{
    public class DeepLinkingService : IService
    {
        private List<WeakReference<object>> registeredListeners = new List<WeakReference<object>>();

        private Dictionary<string, InstancedMethod> paths = new Dictionary<string, InstancedMethod>();

        public IApplication ApplicationAPI { get; set; } = new ApplicationWrapper();

        public void Initialize(IServiceManager owner)
        {
            ApplicationAPI.DeepLinkActivated += OnDeepLinkActivated;
            if (!string.IsNullOrEmpty(ApplicationAPI.AbsoluteURL))
            {
                OnDeepLinkActivated(ApplicationAPI.AbsoluteURL);
            }
        }

        public void AddDeepLinkListener(object listener)
        {
            if (registeredListeners.Contains(listener))
            {
                return;
            }

            registeredListeners.Add(new WeakReference<object>(listener));
            if (!string.IsNullOrEmpty(ApplicationAPI.AbsoluteURL))
            {
                CheckInstanceMethods(ApplicationAPI.AbsoluteURL, listener);
            }
        }

        public void RemoveDeepLinkListener(object listener)
        {
            for (int i = registeredListeners.Count-1; i >= 0; i--)
            {
                if (registeredListeners[i].TryGetTarget(out object item))
                {
                    if (item == listener)
                    {
                        registeredListeners.RemoveAt(i);
                    }
                }
                else
                {
                    registeredListeners.RemoveAt(i);
                }
            }
        }

        public void Cleanup()
        {
            ApplicationAPI.DeepLinkActivated -= OnDeepLinkActivated;
        }

        private void OnDeepLinkActivated(string deepLink)
        {
            Debug.Log("Got deep link for " + deepLink);

            for (int i=registeredListeners.Count-1;i>=0;i--)
            {
                if (registeredListeners[i].TryGetTarget(out object listener))
                {
                    CheckInstanceMethods(deepLink, listener);
                }
                else
                {
                    registeredListeners.RemoveAt(i);
                }
            }
        }

        private void CheckInstanceMethods(string deepLink, object instance)
        {
            Uri uri = new Uri(deepLink);

            string path = uri.Authority.ToLower();

            MethodInfo[] methods = instance.GetType().GetMethods()
                        .Where(m => m.GetCustomAttributes(typeof(DeepLinkAttribute), false).Length > 0)
                        .ToArray();

            foreach (MethodInfo method in methods)
            {
                Attribute[] attributes = method.GetCustomAttributes(typeof(DeepLinkAttribute)).ToArray();
                foreach (DeepLinkAttribute attribute in attributes)
                {
                    if (path.Equals(attribute.Path.ToLower()))
                    {
                        Dictionary<string, string> fragments = UriUtils.GetUriParameters(uri);
                        ParameterInfo[] parameters = method.GetParameters();

                        // convert strings to objects
                        object[] convertedArguments = new object[parameters.Length];
                        for (int paramIndex = 0; paramIndex < parameters.Length; paramIndex++)
                        {
                            string value = fragments[parameters[paramIndex].Name];
                            object convertedValue = Convert.ChangeType(value, parameters[paramIndex].ParameterType);
                            convertedArguments[paramIndex] = convertedValue;
                        }

                        method.Invoke(
                            instance,
                            convertedArguments);
                    }
                }
            }
        }
    }
}