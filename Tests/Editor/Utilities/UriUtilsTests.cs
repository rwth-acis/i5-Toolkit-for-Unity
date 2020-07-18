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

        [Test]
        public void WordArrayToSpaceEscapedString_EmptyArray_EmptyString()
        {
            string[] array = new string[0];
            string result = UriUtils.WordArrayToSpaceEscapedString(array);
            Assert.AreEqual("", result);
        }

        [Test]
        public void WordArrayToSpaceEscapedString_OneWord_CreatesString()
        {
            string[] array = new string[] { "hello" };
            string result = UriUtils.WordArrayToSpaceEscapedString(array);
            Assert.AreEqual("hello", result);
        }

        [Test]
        public void WordArrayToSpaceEscapedString_MultipleWords_CreatesString()
        {
            string[] array = new string[] { "hello", "world" };
            string result = UriUtils.WordArrayToSpaceEscapedString(array);
            Assert.AreEqual("hello%20world", result);
        }

        [Test]
        public void DictionaryToParameterString_EmptyDictionary_EmptyString()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string result = UriUtils.DictionaryToParameterString(dict);
            Assert.AreEqual("", result);
        }

        [Test]
        public void DictionaryToParameterString_OneEntry_ReturnsKeyValue()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("hello", "world");
            string result = UriUtils.DictionaryToParameterString(dict);
            Assert.AreEqual("hello=world", result);
        }

        [Test]
        public void DictionaryToParameterString_MultipleEntries_SplitByAnd()
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            dict.Add("one", 1);
            dict.Add("two", 2);
            string result = UriUtils.DictionaryToParameterString(dict);
            Assert.AreEqual("one=1&two=2", result);
        }
    }
}
