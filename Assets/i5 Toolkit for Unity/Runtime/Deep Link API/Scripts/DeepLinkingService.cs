using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities;
using i5.Toolkit.Core.Experimental.UnityAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace i5.Toolkit.Core.DeepLinkAPI
{
    /// <summary>
    /// Service that allows deep links to access the application
    /// Use <see cref="DeepLinkAttribute"/> to mark methods as targets of deep links.
    /// Moreover, add classes using <see cref="DeepLinkAttribute"/> to the list of listeners in this service using <see cref="AddDeepLinkListener(object)"/>.
    /// </summary>
    public class DeepLinkingService : IDeepLinkingService
    {
        // registered listeners which should be searched for deep link targets
        private List<WeakReference<object>> registeredListeners = new List<WeakReference<object>>();

        /// <summary>
        /// Reference to an application adapter.
        /// In production, this is initialized with an adapter to Unity's Application/>.
        /// </summary>
        public IApplication ApplicationAPI { get; set; } = new ApplicationAdapter();

        /// <summary>
        /// The number of objects that will be searched for deep link targets
        /// </summary>
        public int RegisteredListenersCount => registeredListeners.Count;

        /// <summary>
        /// Initializes the service.
        /// Subscribes to the deep link event and checks if a deep link already occurred.
        /// </summary>
        /// <param name="owner">The service manager that owns this service</param>
        public void Initialize(IServiceManager owner)
        {
            ApplicationAPI.DeepLinkActivated += OnDeepLinkActivated;
            if (!string.IsNullOrEmpty(ApplicationAPI.AbsoluteURL))
            {
                OnDeepLinkActivated(null, ApplicationAPI.AbsoluteURL);
            }
        }

        /// <summary>
        /// Adds the given object instance to the list of objects that are scanned for deep link targets
        /// Add every class that has a method with the <see cref="DeepLinkAttribute"/> so that it can be found.
        /// </summary>
        /// <param name="listener">The object to add the the list of listeners</param>
        public void AddDeepLinkListener(object listener)
        {
            // check if list already contains object
            for (int i = registeredListeners.Count - 1; i >= 0; i--)
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
            // check if the listener targets a deep link that already occurred
            if (!string.IsNullOrEmpty(ApplicationAPI.AbsoluteURL))
            {
                CheckInstanceMethods(ApplicationAPI.AbsoluteURL, listener);
            }
        }

        /// <summary>
        /// Removes the instance from the list of listeners that will be scanned
        /// </summary>
        /// <param name="listener">The object instance to remove from the listener list</param>
        public void RemoveDeepLinkListener(object listener)
        {
            // find the listener instance and clean up along the way
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

        /// <summary>
        /// Cleans up the service
        /// </summary>
        public void Cleanup()
        {
            ApplicationAPI.DeepLinkActivated -= OnDeepLinkActivated;
            registeredListeners.Clear();
        }

        // Called if a deep link was found
        private void OnDeepLinkActivated(object sender, string deepLink)
        {
            Debug.Log("Got deep link for " + deepLink);

            // go over every registered listener instance and check if it contains a target method
            // clean up garbage collected references along the way
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

        // scans the methods of the given instance's class to find deep link targets
        private void CheckInstanceMethods(string deepLink, object instance)
        {
            // fix links that only use ":/" so that System.Uri is able to analyze them
            if (!deepLink.Contains("://") && deepLink.Contains(":/"))
            {
                deepLink = deepLink.Replace(":/", "://");
            }

            Uri uri = new Uri(deepLink);

            string path = uri.Authority.ToLower();

            // scan every method that is marked with a DeepLinkAttribute
            MethodInfo[] methods = instance.GetType().GetMethods()
                        .Where(m => m.GetCustomAttributes(typeof(DeepLinkAttribute), false).Length > 0)
                        .ToArray();

            foreach (MethodInfo method in methods)
            {
                Attribute[] attributes = method.GetCustomAttributes(typeof(DeepLinkAttribute)).ToArray();
                // check every DeepLinkAttribute on the method
                foreach (DeepLinkAttribute attribute in attributes)
                {
                    bool schemeMatches = string.IsNullOrEmpty(attribute.Scheme)
                        || uri.Scheme == attribute.Scheme.Replace("://", "");

                    if (schemeMatches && path.Equals(attribute.Path.ToLower()))
                    {
                        ParameterInfo[] parameters = method.GetParameters();

                        // if the method provides a DeepLinkArgs argument, fill it, otherwise do not provide arguments
                        if (parameters.Length == 0)
                        {
                            method.Invoke(instance, null);
                        }
                        else if (parameters.Length == 1 && parameters[0].ParameterType == typeof(DeepLinkArgs))
                        {
                            Dictionary<string, string> fragments = UriUtils.GetUriParameters(uri);
                            DeepLinkArgs args = new DeepLinkArgs(fragments, uri);

                            method.Invoke(instance, new object[] { args });
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