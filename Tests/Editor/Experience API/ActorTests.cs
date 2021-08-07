using i5.Toolkit.Core.ExperienceAPI;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace i5.Toolkit.Core.Tests.ExperienceAPI
{
    public class ActorTests
    {
        private const string mailAddress = "tester@i5toolkit.com";

        [Test]
        public void Constr_MailWithoutMailto_AddsMailto()
        {
            Actor actor = new Actor(mailAddress);

            Assert.AreEqual($"mailto:{mailAddress}", actor.Mbox);
        }

        [Test]
        public void Constr_MailWithMailto_Unmodified()
        {
            string mailto = $"mailto:{mailAddress}";

            Actor actor = new Actor(mailto);

            Assert.AreEqual(mailto, actor.Mbox);
        }

        [Test]
        public void Constr_MailtoAndName_NameSet()
        {
            string mailto = $"mailto:{mailAddress}";
            string name = "tester";

            Actor actor = new Actor(mailto, name);

            Assert.AreEqual(name, actor.name);
        }

        [Test]
        public void Constr_NoMailtoAndName_AddsMailTo()
        {
            string name = "tester";

            Actor actor = new Actor(mailAddress, name);

            Assert.AreEqual($"mailto:{mailAddress}", actor.Mbox);
        }

#if NEWTONSOFT_JSON
        [Test]
        public void ToJObject_MboxSet()
        {
            string mailto = $"mailto:{mailAddress}";
            Actor actor = new Actor(mailto);

            JObject result = actor.ToJObject();

            Assert.AreEqual(mailto, result.GetValue("mbox").ToString());
        }

        [Test]
        public void ToJObject_ObjectTypeSetToAgent()
        {
            string mailto = $"mailto:{mailAddress}";
            Actor actor = new Actor(mailto);

            JObject result = actor.ToJObject();

            Assert.AreEqual("Agent", result.GetValue("objectType").ToString());
        }

        [Test]
        public void ToJObject_NoName_NameNotIncluded()
        {
            string mailto = $"mailto:{mailAddress}";
            Actor actor = new Actor(mailto);

            JObject result = actor.ToJObject();

            Assert.False(result.ContainsKey("name"));
        }

        [Test]
        public void ToJObject_NameSet_NameIncluded()
        {
            string mailto = $"mailto:{mailAddress}";
            string name = "Tester";
            Actor actor = new Actor(mailto, name);

            JObject result = actor.ToJObject();

            Assert.AreEqual(name, result.GetValue("name").ToString());
        }
#endif
    }
}
