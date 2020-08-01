using i5.Toolkit.Core.Utilities;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace i5.Toolkit.Core.Tests.Utilities
{
    public class NameValueCollectionExtensionsTests
    {
        [Test]
        public void ToDictionary_EmptyCollection_ReturnsEmptyDictionary()
        {
            NameValueCollection nvc = new NameValueCollection();

            Dictionary<string, string> dictionary = nvc.ToDictionary();
            Assert.AreEqual(0, dictionary.Count);
        }

        [Test]
        public void ToDictionary_EntryAdded_DictionaryContainsKey()
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("test", "value");

            Dictionary<string, string> dictionary = nvc.ToDictionary();

            Assert.IsTrue(dictionary.ContainsKey("test"));
        }

        [Test]
        public void ToDictionary_EntryAdded_DictionaryContainsValue()
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("test", "value");

            Dictionary<string, string> dictionary = nvc.ToDictionary();

            string retrieved = dictionary["test"];
            Assert.AreEqual("value", retrieved);
        }
    }
}
