using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

namespace i5.Toolkit.Core.Utilities
{
    public static class NameValueCollectionExtensions
    {
        public static Dictionary<string, string> ToDictionary(this NameValueCollection collection)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (string key in collection.AllKeys)
            {
                result.Add(key, collection[key]);
            }
            return result;
        }
    }
}