using i5.Toolkit.Core.ExperienceAPI;
#if NEWTONSOFT_JSON
using Newtonsoft.Json.Linq;
#endif
using NUnit.Framework;

namespace i5.Toolkit.Core.Tests.ExperienceAPI
{
    public class VerbTests
    {
        private Verb verb;

        private const string verbId = "http://example.com/tested";

        [SetUp]
        public void SetUp()
        {
            verb = new Verb(verbId);
        }

        [Test]
        public void Constr_DisplayLanguageDictionaryInitialized()
        {
            Assert.NotNull(verb.displayLanguageDictionary);
        }

        [Test]
        public void Constr_VerbIdSet()
        {
            Assert.AreEqual(verbId, verb.id);
        }

        [Test]
        public void CutToVerbName_ReturnsVerb()
        {
            string result = Verb.CutToVerbName(verbId);
            Assert.AreEqual("tested", result);
        }

        [Test]
        public void CutToVerbName_InvalidId_ReturnsOriginalString()
        {
            string invalidId = "tested";
            string result = Verb.CutToVerbName(invalidId);
            Assert.AreEqual(invalidId, result);
        }

#if NEWTONSOFT_JSON
        [Test]
        public void ToJObject_IdCorrect()
        {
            JObject result = verb.ToJObject();

            Assert.AreEqual(verbId, (string)result["id"]);
        }

        [Test]
        public void ToJObject_NoDisplayLanguageAdded_DisplayNodeCreated()
        {
            JObject result = verb.ToJObject();

            Assert.True(result.ContainsKey("display"));
        }

        [Test]
        public void ToJObject_NoDisplayLanguageAdded_EnUsVerbNameUsed()
        {
            JObject result = verb.ToJObject();

            Assert.AreEqual("tested", (string)result["display"]["en-us"]);
        }

        [Test]
        public void ToJObject_DisplayLanguageAdded_Used()
        {
            verb.displayLanguageDictionary.Add("de", "getestet");
            JObject result = verb.ToJObject();

            Assert.AreEqual("getestet", (string)result["display"]["de"]);
        }

#endif
    }
}
