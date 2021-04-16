using i5.Toolkit.Core.ExperienceAPI;
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

            Assert.AreEqual($"mailto:{mailAddress}", actor.mbox);
        }

        [Test]
        public void Constr_MailWithMailto_Unmodified()
        {
            string mailto = $"mailto:{mailAddress}";

            Actor actor = new Actor(mailto);

            Assert.AreEqual(mailto, actor.mbox);
        }
    }
}
