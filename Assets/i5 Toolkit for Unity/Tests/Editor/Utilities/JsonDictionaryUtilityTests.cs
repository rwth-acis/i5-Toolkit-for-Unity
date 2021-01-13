using System.Collections;
using System.Collections.Generic;
using i5.Toolkit.Core.Utilities;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Core.Tests.Utilities
{
    public class JsonDictionaryUtilityTests
    {
        [Test]
        public void ToJson_NoEntry_RightFormat()
        {
            Dictionary<string, int> testDictionary = new Dictionary<string, int>();

            string result = JsonDictionaryUtility.ToJson(testDictionary);

            string expected = "{\"keys\":[],\"values\":[]}";

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ToJson_SingleEntry_RightFormat()
        {
            Dictionary<string, int> testDictionary = new Dictionary<string, int>();
            testDictionary.Add("testKey", 42);

            string result = JsonDictionaryUtility.ToJson(testDictionary);

            string expected = "{\"keys\":[\"testKey\"],\"values\":[42]}";

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ToJson_MultipleEntries_RightFormat()
        {
            Dictionary<string, int> testDictionary = new Dictionary<string, int>();
            testDictionary.Add("testKey", 42);
            testDictionary.Add("secondKey", 1);

            string result = JsonDictionaryUtility.ToJson(testDictionary);

            string expected = "{\"keys\":[\"testKey\",\"secondKey\"],\"values\":[42,1]}";

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void FromJson_NoEntries_EmptyDictionaryCreated()
        {
            string json = "{\"keys\":[],\"values\":[]}";
            Dictionary<string, int> result = JsonDictionaryUtility.FromJson<string,int>(json);

            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void FromJson_SingleEntry_DictionaryContainsEntry()
        {
            string json = "{\"keys\":[\"testKey\"],\"values\":[42]}";
            Dictionary<string, int> result = JsonDictionaryUtility.FromJson<string, int>(json);

            Assert.IsTrue(result.ContainsKey("testKey"));
            Assert.AreEqual(42, result["testKey"]);
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public void FromJson_MultipleEntries_DictionaryContainsEntries()
        {
            string json = "{\"keys\":[\"testKey\",\"secondKey\"],\"values\":[42,1]}";
            Dictionary<string, int> result = JsonDictionaryUtility.FromJson<string, int>(json);

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.ContainsKey("testKey"));
            Assert.IsTrue(result.ContainsKey("secondKey"));
            Assert.AreEqual(result["testKey"], 42);
            Assert.AreEqual(result["secondKey"], 1);
        }
    }
}
