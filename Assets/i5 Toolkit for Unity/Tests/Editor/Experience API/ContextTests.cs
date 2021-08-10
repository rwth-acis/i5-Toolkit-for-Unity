using i5.Toolkit.Core.ExperienceAPI;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using UnityEngine;

namespace i5.Toolkit.Core.Tests.ExperienceAPI
{
    public class ContextTests
    {
        private Context context;

        [SetUp]
        public void SetUp()
        {
            context = new Context();
        }

        [Test]
        public void Constr_ParentActivityIdsInitiliaized()
        {
            Assert.NotNull(context.ParentActivityIDs);
        }

        [Test]
        public void AddParentActivity_ValueNonWhitespace_AddsValue()
        {
            string id = "sampleId";

            context.AddParentActivity(id);

            Assert.AreEqual(1, context.ParentActivityIDs.Count);
            Assert.AreEqual(id, context.ParentActivityIDs[0]);
        }

        [Test]
        public void AddParentActivity_ValueNull_DoesNotAddValue()
        {
            context.AddParentActivity(null);

            Assert.AreEqual(0, context.ParentActivityIDs.Count);
        }

        [Test]
        public void AddParentActivity_ValueWhitespace_DoesNotAddValue()
        {
            context.AddParentActivity(" ");

            Assert.AreEqual(0, context.ParentActivityIDs.Count);
        }


#if NEWTONSOFT_JSON
        [Test]
        public void ToJObject_NoParentActivityIDs_DoesNotAddContextActivities()
        {
            JObject result = context.ToJObject();

            Assert.False(result.ContainsKey("contextActivities"));
        }

        [Test]
        public void ToJObject_ParentActivityIDsSet_AddsContextActivities()
        {
            context.AddParentActivity("testId");

            JObject result = context.ToJObject();

            Assert.True(result.ContainsKey("contextActivities"));
        }
#endif
    }
}
