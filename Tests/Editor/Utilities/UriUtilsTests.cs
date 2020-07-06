using System;
using System.Collections;
using System.Collections.Generic;
using i5.Toolkit.Core.Utilities;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Core.Tests.Utilities
{
    public class UriUtilsTests
    {
        [Test]
        public void GetUriParameters_NoParameters_ReturnsEmptyDictionary()
        {
            Uri uri = new Uri("http://www.google.de");
            Dictionary<string, string> parameters = UriUtils.GetUriParameters(uri);

            Assert.NotNull(parameters);
            Assert.AreEqual(0, parameters.Count);
        }

        [Test]
        public void GetUriParameters_OneParameter_DictionaryContainsKeyValuePair()
        {
            Uri uri = new Uri("http://www.google.de?param=test");
            Dictionary<string, string> parameters = UriUtils.GetUriParameters(uri);

            Assert.IsTrue(parameters.ContainsKey("param"));
            string retrieved = parameters["param"];
            Assert.AreEqual("test", retrieved);
        }

        [Test]
        public void GetUriParameters_MultipleParameters_DictionaryContainsKeyValuePairs()
        {
            Uri uri = new Uri("http://www.google.de?param=test&number=1");
            Dictionary<string, string> parameters = UriUtils.GetUriParameters(uri);

            Assert.AreEqual(2, parameters.Count);

            Assert.IsTrue(parameters.ContainsKey("param"));
            string retrieved = parameters["param"];
            Assert.AreEqual("test", retrieved);
            Assert.IsTrue(parameters.ContainsKey("number"));
            retrieved = parameters["number"];
            Assert.AreEqual("1", retrieved);
        }
    }
}
