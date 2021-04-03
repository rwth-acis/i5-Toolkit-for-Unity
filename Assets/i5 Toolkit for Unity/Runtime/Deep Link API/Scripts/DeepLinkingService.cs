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

        public IApplication ApplicationAPI { get; set; } = new ApplicationWrapper();

        public int RegisteredListenersCount => registeredListeners.Count;

        public void Initialize(IServiceManager owner)
        {
            ApplicationAPI.DeepLinkActivated += OnDeepLinkActivated;
            if (!string.IsNullOrEmpty(ApplicationAPI.AbsoluteURL))
            {
                OnDeepLinkActivated(null, ApplicationAPI.AbsoluteURL);
            }
        }

        public void AddDeepLinkListener(object listener)
        {
            // check if list already contains object
            for (int i=registeredListeners.Count-1;i>=0;i--)
            {
                if (registeredListeners[i].TryGetTarget(out object item))
                {
                    if (item.Equals(listener))
                    {
                        // if it is contained: return
                        return;
                    }
                }
                // remove garbage collected items
                else
                {
                    registeredListeners.RemoveAt(i);
                }
            }

            registeredListeners.Add(new WeakReference<object>(listener));
            if (!string.IsNullOrEmpty(ApplicationAPI.AbsoluteURL))
            {
                CheckInstanceMethods(ApplicationAPI.AbsoluteURL, listener);
            }
        }

        public void RemoveDeepLinkListener(object listener)
        {
            for (int i = registeredListeners.Count - 1; i >= 0; i--)
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
            registeredListeners.Clear();
        }

        private void OnDeepLinkActivated(object sender, string deepLink)
        {
            Debug.Log("Got deep link for " + deepLink);

            for (int i = registeredListeners.Count - 1; i >= 0; i--)
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
                        ParameterInfo[] parameters = method.GetParameters();

                        if (parameters.Length == 0)
                        {
                            method.Invoke(instance, null);
                        }
                        else if (parameters.Length == 1 && parameters[0].ParameterType == typeof(DeepLinkArgs))
                        {
                            Dictionary<string, string> fragments = UriUtils.GetUriParameters(uri);
                            DeepLinkArgs args = new DeepLinkArgs(fragments, uri);

                            method.Invoke(
                                instance,
                                new object[] { args }
                                );
                        }
                        else
                        {
                            i5Debug.LogError($"Cannot deep-link-invoke method {method.Name} since it must either have 0 arguments or 1 argument of type {nameof(DeepLinkArgs)}.", this);
                        }
                    }
                }
            }
        }
    }
}