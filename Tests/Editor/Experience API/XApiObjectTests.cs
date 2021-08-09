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
    public class XApiObjectTests
    {
        private XApiObject obj;

        private const string objId = "myId";


        [SetUp]
        public void SetUp()
        {
            obj = new XApiObject(objId);
        }

        [Test]
        public void Constr_NameDisplayInitialized()
        {
            Assert.NotNull(obj.nameDisplay);
        }

        [Test]
        public void Constr_DescriptionDisplayInitialized()
        {
            Assert.NotNull(obj.descriptionDisplay);
        }

        [Test]
        public void Constr_IdSet()
        {
            Assert.AreEqual(objId, obj.id);
        }

        [Test]
        public void AddName_EmptyValue_NotAdded()
        {
            obj.AddName("");

            Assert.AreEqual(0, obj.nameDisplay.Count);
        }

        [Test]
        public void AddName_WhitespaceValue_NotAdded()
        {
            obj.AddName("  ");

            Assert.AreEqual(0, obj.nameDisplay.Count);
        }

        [Test]
        public void AddName_NonEmptyValue_Added()
        {
            obj.AddName("myName");

            Assert.AreEqual(1, obj.nameDisplay.Count);
        }

        [Test]
        public void AddName_NoLanguageGiven_UsesEnUs()
        {
            obj.AddName("myName");

            Assert.AreEqual("myName", obj.nameDisplay["en-us"]);
        }

        [Test]
        public void AddName_LanguageGiven_UsesIt()
        {
            obj.AddName("meinName", "de");

            Assert.AreEqual("meinName", obj.nameDisplay["de"]);
        }

        [Test]
        public void AddDescription_EmptyValue_NotAdded()
        {
            obj.AddDescription("");

            Assert.AreEqual(0, obj.descriptionDisplay.Count);
        }

        [Test]
        public void AddDescription_WhitespaceValue_NotAdded()
        {
            obj.AddDescription("  ");

            Assert.AreEqual(0, obj.descriptionDisplay.Count);
        }

        [Test]
        public void AddDescription_NonEmptyValue_Added()
        {
            obj.AddDescription("myDescription");

            Assert.AreEqual(1, obj.descriptionDisplay.Count);
        }

        [Test]
        public void AddDescription_NoLanguageGiven_UsesEnUs()
        {
            obj.AddDescription("myDescription");

            Assert.AreEqual("myDescription", obj.descriptionDisplay["en-us"]);
        }

        [Test]
        public void AddDescription_LanguageGiven_UsesIt()
        {
            obj.AddDescription("meineBeschreibung", "de");

            Assert.AreEqual("meineBeschreibung", obj.descriptionDisplay["de"]);
        }

#if NEWTONSOFT_JSON
        [Test]
        public void ToJObject_SetsId()
        {
            JObject res = obj.ToJObject();

            Assert.AreEqual(objId, (string)res["id"]);
        }

        [Test]
        public void ToJObject_SetsObjectTypeToActivity()
        {
            JObject res = obj.ToJObject();

            Assert.AreEqual("Activity", (string)res["objectType"]);
        }

        [Test]
        public void ToJObject_NoType_NotAdded()
        {
            JObject res = obj.ToJObject();

            Assert.False(res.ContainsKey("type"));
        }

        [Test]
        public void ToJObject_TypeGiven_Added()
        {
            string typeId = "http://example.com/unitTest";
            obj.type = typeId;
            JObject res = obj.ToJObject();

            Assert.AreEqual(typeId, (string)res["definition"]["type"]);
        }

        [Test]
        public void ToJObject_NoNameDisplays_NotAdded()
        {
            JObject res = obj.ToJObject();

            Assert.Null(res["definition"]);
        }

        [Test]
        public void ToJObject_NameGiven_Added()
        {
            obj.AddName("myName");
            obj.AddName("meinName", "de");

            JObject res = obj.ToJObject();

            Assert.AreEqual("myName", (string)res["definition"]["name"]["en-us"]);
            Assert.AreEqual("meinName", (string)res["definition"]["name"]["de"]);
        }

        [Test]
        public void ToJObject_NoDescription_NotAdded()
        {
            JObject res = obj.ToJObject();

            Assert.Null(res["definition"]);
        }

        [Test]
        public void ToJObject_DescriptionAdded_Added()
        {
            obj.AddDescription("myDescription");
            obj.AddDescription("meineBeschreibung", "de");

            JObject res = obj.ToJObject();

            Assert.AreEqual("myDescription", (string)res["definition"]["description"]["en-us"]);
            Assert.AreEqual("meineBeschreibung", (string)res["definition"]["description"]["de"]);
        }
#endif
    }
}
