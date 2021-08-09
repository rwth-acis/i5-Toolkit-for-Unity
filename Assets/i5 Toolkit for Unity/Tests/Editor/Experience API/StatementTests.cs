using System;
using System.Collections;
using System.Collections.Generic;
using i5.Toolkit.Core.ExperienceAPI;
#if NEWTONSOFT_JSON
using Newtonsoft.Json.Linq;
#endif
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Core.Tests.ExperienceAPI
{
    public class StatementTests
    {
        private Statement statement;

        private const string actorId = "mailto:tester@i5tookit.com";
        private const string verbId = "http://example.com/tested";
        private const string objectId = "http://example.com/module";

        [SetUp]
        public void SetUp()
        {
            statement = new Statement(actorId, verbId, objectId);
        }

        [Test]
        public void Constr_StringIdsProvided_ActorGenerated()
        {
            Assert.AreEqual(actorId, statement.actor.Mbox);
        }

        [Test]
        public void Constr_StringIdsProvided_VerbGenerated()
        {
            Assert.AreEqual(verbId, statement.verb.id);
        }

        [Test]
        public void Constr_StringIdsProvided_ObjectGenerated()
        {
            Assert.AreEqual(objectId, statement.@object.id);
        }

        [Test]
        public void Constr_ActorProvided_ActorSet()
        {
            Actor actor = new Actor(actorId, "myName");
            statement = new Statement(actor, new Verb(verbId), new XApiObject(objectId));

            Assert.AreEqual(actor, statement.actor);
        }

        [Test]
        public void Constr_VerbProvided_VerbSet()
        {
            Verb verb = new Verb(verbId);
            statement = new Statement(new Actor(actorId), verb, new XApiObject(objectId));

            Assert.AreEqual(verb, statement.verb);
        }

        [Test]
        public void Constr_ObjectProvided_ObjectSet()
        {
            XApiObject obj = new XApiObject(objectId);
            statement = new Statement(new Actor(actorId), new Verb(verbId), obj);

            Assert.AreEqual(obj, statement.@object);
        }

#if NEWTONSOFT_JSON
        [Test]
        public void ToJObject_AddsActor()
        {
            JObject res = statement.ToJObject();

            Assert.True(res.ContainsKey("actor"));
        }

        [Test]
        public void ToJObject_AddsVerb()
        {
            JObject res = statement.ToJObject();

            Assert.True(res.ContainsKey("verb"));
        }

        [Test]
        public void ToJObject_AddsObject()
        {
            JObject res = statement.ToJObject();

            Assert.True(res.ContainsKey("object"));
        }

        [Test]
        public void ToJObject_NoResult_NotAdded()
        {
            JObject res = statement.ToJObject();

            Assert.False(res.ContainsKey("result"));
        }

        [Test]
        public void ToJObject_ResultSet_Added()
        {
            statement.result = new Result();
            statement.result.success = true;

            JObject res = statement.ToJObject();

            Assert.True(res.ContainsKey("result"));
        }

        [Test]
        public void ToJObject_NoContext_NotAdded()
        {
            JObject res = statement.ToJObject();

            Assert.False(res.ContainsKey("context"));
        }

        [Test]
        public void ToJObject_ContextSet_Added()
        {
            statement.context = new Context();
            statement.context.AddParentActivity("myParentActivity");

            JObject res = statement.ToJObject();

            Assert.True(res.ContainsKey("context"));
        }

        [Test]
        public void ToJObject_NoTimestamp_NotAdded()
        {
            JObject res = statement.ToJObject();

            Assert.False(res.ContainsKey("timestamp"));
        }

        [Test]
        public void ToJObject_TimestampSet_Added()
        {
            statement.timestamp = new DateTime(2021, 1, 1);

            JObject res = statement.ToJObject();

            Assert.True(res.ContainsKey("timestamp"));
        }

        [Test]
        public void ToJObject_TimestampSet_CorrectValue()
        {
            DateTime time = new DateTime(2021, 1, 1);
            statement.timestamp = time;

            JObject res = statement.ToJObject();

            Assert.AreEqual(time, (DateTime)res["timestamp"]);
        }

        [Test]
        public void ToJsonString_ContainsActor()
        {
            string json = statement.ToJSONString();

            Assert.True(json.Contains("actor"));
            Assert.True(json.Contains(actorId));
        }

        [Test]
        public void ToJsonString_ContainsVerb()
        {
            string json = statement.ToJSONString();

            Assert.True(json.Contains("verb"));
            Assert.True(json.Contains(verbId));
        }

        [Test]
        public void ToJsonString_ContainsObject()
        {
            string json = statement.ToJSONString();

            Assert.True(json.Contains("object"));
            Assert.True(json.Contains(objectId));
        }

        [Test]
        public void ToJsonString_NoResult_NotContained()
        {
            string json = statement.ToJSONString();

            Assert.False(json.Contains("result"));
        }

        [Test]
        public void ToJsonString_ResultSet_ContainsResult()
        {
            statement.result = new Result();
            statement.result.success = true;

            string json = statement.ToJSONString();

            Assert.True(json.Contains("result"));
        }

        [Test]
        public void ToJsonString_NoConext_NotContained()
        {
            string json = statement.ToJSONString();

            Assert.False(json.Contains("context"));
        }

        [Test]
        public void ToJsonString_ContextSet_ContainsResult()
        {
            statement.context = new Context();
            statement.context.AddParentActivity("myParentActivity");

            string json = statement.ToJSONString();

            Assert.True(json.Contains("context"));
        }

        [Test]
        public void ToJsonString_NoTimestamp_NotContained()
        {
            string json = statement.ToJSONString();

            Assert.False(json.Contains("timestamp"));
        }

        [Test]
        public void ToJsonString_TimestampSet_ContainsTimestamp()
        {
            statement.timestamp = new DateTime(2021, 1, 1);

            string json = statement.ToJSONString();

            Assert.True(json.Contains("timestamp"));
        }
#endif
    }
}
