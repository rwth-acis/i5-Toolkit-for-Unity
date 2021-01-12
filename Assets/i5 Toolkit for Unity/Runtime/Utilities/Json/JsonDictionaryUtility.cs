using System;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.Utilities
{
    public static class JsonDictionaryUtility
    {
        public static Dictionary<K, V> FromJson<K, V>(string json)
        {
            JsonDictionary<K, V> dictionary = JsonUtility.FromJson<JsonDictionary<K, V>>(json);
            return dictionary.ToDictionary();
        }

        public static string ToJson<K,V>(Dictionary<K,V> dict, bool prettyPrint = false)
        {
            JsonDictionary<K, V> serialized = JsonDictionary<K,V>.FromDictionary(dict);
            return JsonUtility.ToJson(serialized, prettyPrint);
        }

        [Serializable]
        private class JsonDictionary<K, V>
        {
            [SerializeField] private List<K> keys = new List<K>();
            [SerializeField] private List<V> values = new List<V>();

            public void Add(K key, V value)
            {
                keys.Add(key);
                values.Add(value);
            }

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