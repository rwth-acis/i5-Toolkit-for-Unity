using i5.Toolkit.Core.Editor.TestHelpers;
using i5.Toolkit.Core.Experimental.UnityAdapters;
using NUnit.Framework;
using UnityEngine;

namespace i5.Toolkit.Core.Tests.Experimental.UnityAdapters
{
    public class TextMeshTextAdapterTests
    {
        private TextMesh tm;
        private TextMeshTextAdapter tmAdapter;

        private const string sampleText = "This is a test";

        [SetUp]
        public void SetUp()
        {
            EditModeTestUtilities.ResetScene();
            GameObject go = new GameObject();
            tm = go.AddComponent<TextMesh>();
            tmAdapter = new TextMeshTextAdapter(tm);
        }

        [Test]
        public void GetText_SameAsTextMesh()
        {
            tm.text = sampleText;

            Assert.AreEqual(sampleText, tmAdapter.Text);
        }

        [Test]
        public void SetText_AppliesText()
        {
            tmAdapter.Text = sampleText;

            Assert.AreEqual(sampleText, tm.text);
        }
    }
}
