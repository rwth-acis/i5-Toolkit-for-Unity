using i5.Toolkit.Core.Utilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace i5.Toolkit.Core.Tests.Utilities
{
    /// <summary>
    /// Tests for the UriUtils class
    /// </summary>
    public class UriUtilsTests
    {
        /// <summary>
        /// Checks if GetUriParameters returns any empty dictionary if the given Uri does not contain parameters
        /// </summary>
        [Test]
        public void GetUriParameters_NoParameters_ReturnsEmptyDictionary()
        {
            Uri uri = new Uri("http://www.google.de");
            Dictionary<string, string> parameters = UriUtils.GetUriParameters(uri);

            Assert.NotNull(parameters);
            Assert.AreEqual(0, parameters.Count);
        }

        /// <summary>
        /// Checks that GetUriParameters returns the parameter if one is given in a Uri
        /// </summary>
        [Test]
        public void GetUriParameters_OneParameter_DictionaryContainsKeyValuePair()
        {
            Uri uri = new Uri("http://www.google.de?param=test");
            Dictionary<string, string> parameters = UriUtils.GetUriParameters(uri);

            Assert.IsTrue(parameters.ContainsKey("param"));
            string retrieved = parameters["param"];
            Assert.AreEqual("test", retrieved);
        }

        /// <summary>
        /// Checks that all parameters are returned if a Uri contains multiple parameters
        /// </summary>
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
