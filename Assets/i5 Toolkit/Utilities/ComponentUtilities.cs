using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Utilities
{
    public static class ComponentUtilities
    {
        public static T GetOrAddComponent<T>(GameObject gameObject) where T : Component
        {
            T result = gameObject.GetComponent<T>();
            if (result == null)
            {
                result = gameObject.AddComponent<T>();
            }
            return result;
        }
    }
}