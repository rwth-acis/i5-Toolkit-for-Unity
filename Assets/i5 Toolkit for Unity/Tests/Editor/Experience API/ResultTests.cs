using i5.Toolkit.Core.ExperienceAPI;
#if NEWTONSOFT_JSON
using Newtonsoft.Json.Linq;
#endif
using NUnit.Framework;
using System;
using UnityEngine;

namespace i5.Toolkit.Core.Tests.ExperienceAPI
{
    public class ResultTests
    {
        private Result result;

        [SetUp]
        public void SetUp()
        {
            result = new Result();
        }

        [Test]
        public void Constr_InitializesExtensionsDictionary()
        {
            Assert.NotNull(result.extensions);
        }

        [Test]
        public void AddMeasurementAttempt_AddedToExtensions()
        {
            result.AddMeasurementAttempt("myIRI", "someValue");

            Assert.AreEqual(1, result.extensions.Count);
            Assert.True(result.extensions.ContainsKey("myIRI"));
        }

#if NEWTONSOFT_JSON
        [Test]
        public void ToJObject_SuccessNull_NotAdded()
        {
            JObject json = result.ToJObject();

            Assert.False(json.ContainsKey("success"));
        }

        [Test]
        public void ToJObject_SuccessSet_Added()
        {
            result.success = true;
            JObject json = result.ToJObject();

            Assert.True(json.ContainsKey("success"));
        }

        [Test]
        public void ToJObject_SuccessSet_ValueCorrect()
        {
            result.success = true;
            JObject json = result.ToJObject();

            Assert.True((bool)json["success"]);
        }

        [Test]
        public void ToJObject_CompletionNull_NotAdded()
        {
            JObject json = result.ToJObject();

            Assert.False(json.ContainsKey("completion"));
        }

        [Test]
        public void ToJObject_CompletionSet_Added()
        {
            result.completion = true;
            JObject json = result.ToJObject();

            Assert.True(json.ContainsKey("completion"));
        }

        [Test]
        public void ToJObject_CompletionSet_ValueCorrect()
        {
            result.completion = true;
            JObject json = result.ToJObject();

            Assert.True((bool)json["completion"]);
        }

        [Test]
        public void ToJObject_ResponseNull_NotAdded()
        {
            JObject json = result.ToJObject();

            Assert.False(json.ContainsKey("response"));
        }

        [Test]
        public void ToJObject_ResponseSet_Added()
        {
            result.response = "myResp";
            JObject json = result.ToJObject();

            Assert.True(json.ContainsKey("response"));
        }

        [Test]
        public void ToJObject_ResponseSet_ValueCorrect()
        {
            result.response = "myResp";
            JObject json = result.ToJObject();

            Assert.AreEqual("myResp",(string)json["response"]);
        }

        [Test]
        public void ToJObject_DurationNull_NotAdded()
        {
            JObject json = result.ToJObject();

            Assert.False(json.ContainsKey("duration"));
        }

        [Test]
        public void ToJObject_DurationSet_Added()
        {
            result.duration = new TimeSpan(0, 15, 0);
            JObject json = result.ToJObject();

            Assert.True(json.ContainsKey("duration"));
        }

        [Test]
        public void ToJObject_DurationSet_ValueCorrect()
        {
            result.duration = new TimeSpan(0, 15, 0);
            JObject json = result.ToJObject();

            Assert.AreEqual(new TimeSpan(0, 15, 0), (TimeSpan)json["duration"]);
        }

        [Test]
        public void ToJObject_NoExtensions_NotAdded()
        {
            JObject json = result.ToJObject();

            Assert.False(json.ContainsKey("extensions"));
        }

        [Test]
        public void ToJObject_ExtensionSet_Added()
        {
            result.extensions.Add("myKey", "myValue");
            JObject json = result.ToJObject();

            Assert.True(json.ContainsKey("extensions"));
        }

        [Test]
        public void ToJObject_ExtensionSet_CountCorrect()
        {
            result.extensions.Add("myKey", "myValue");
            JObject json = result.ToJObject();

            Assert.AreEqual("myValue", (string)json["extensions"]["myKey"]);
        }
#endif
    }
}
