using System;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.Utilities
{
    public static class JsonDictionaryUtility
    {
        /// <summary>
        /// Deserializes a JSON string to a dictionary
        /// </summary>
        /// <typeparam name="K">The type of the dictionary's keys</typeparam>
        /// <typeparam name="V">The type of the dictionary's values</typeparam>
        /// <param name="json">The json string which should be parsed</param>
        /// <returns>Returns the deserialized dictionary</returns>
        public static Dictionary<K, V> FromJson<K, V>(string json)
        {
            JsonDictionary<K, V> dictionary = JsonUtility.FromJson<JsonDictionary<K, V>>(json);
            return dictionary.ToDictionary();
        }

        /// <summary>
        /// Serializes a dictionary to a JSON string
        /// </summary>
        /// <typeparam name="K">The type of the dictionary's keys</typeparam>
        /// <typeparam name="V">The type of the dictionary's values</typeparam>
        /// <param name="dict">The dictionary which should be serialized</param>
        /// <param name="prettyPrint">If true, the JSON string will be formatted to be better human-readable</param>
        /// <returns>Returns the serialized JSON string</returns>
        public static string ToJson<K,V>(Dictionary<K,V> dict, bool prettyPrint = false)
        {
            JsonDictionary<K, V> serialized = JsonDictionary<K,V>.FromDictionary(dict);
            return JsonUtility.ToJson(serialized, prettyPrint);
        }

        // representation of the dictionary in the JSON string
        [Serializable]
        private class JsonDictionary<K, V>
        {
            [SerializeField] private List<K> keys = new List<K>();
            [SerializeField] private List<V> values = new List<V>();

            // adds an entry
            public void Add(K key, V value)
            {
                keys.Add(key);
                values.Add(value);
            }

            // converts the internal representation to a dictionary
            public Dictionary<K, V> ToDictionary()
            {
                if (keys.Count != values.Count)
                {
                    i5Debug.LogError("JSON Dictionary's key and value arrays have different lengths. Cannot convert to native dictionary.", this);
                }

                Dictionary<K, V> result = new Dictionary<K, V>();
                for (int i=0;i<keys.Count;i++)
                {
                    result.Add(keys[i], values[i]);
                }
                return result;
            }

            // converts from a dictionary to this internal representation
            public static JsonDictionary<K,V> FromDictionary(Dictionary<K,V> dict)
            {
                JsonDictionary<K, V> result = new JsonDictionary<K, V>();

                foreach(KeyValuePair<K,V> pair in dict)
                {
                    result.Add(pair.Key, pair.Value);
                }

                return result;
            }
        }
    }
}