using i5.Toolkit.Core.Editor.TestHelpers;
using i5.Toolkit.Core.Experimental.UnityAdapters;
using NUnit.Framework;
using TMPro;
using UnityEngine;

namespace i5.Toolkit.Core.Tests.Experimental.UnityAdapters
{
    public class TextMeshProTextAdapterTests
    {
        private TextMeshPro tmp;
        private TextMeshProTextAdapter tmpAdapter;

        private const string sampleText = "This is a test";

        [SetUp]
        public void SetUp()
        {
            EditModeTestUtilities.ResetScene();
            GameObject go = new GameObject();
            tmp = go.AddComponent<TextMeshPro>();
            tmpAdapter = new TextMeshProTextAdapter(tmp);
        }

        [Test]
        public void GetText_SameAsTextMeshPro()
        {
            tmp.text = sampleText;

            Assert.AreEqual(sampleText, tmpAdapter.Text);
        }

        [Test]
        public void SetText_AppliesText()
        {
            tmpAdapter.Text = sampleText;

            Assert.AreEqual(sampleText, tmp.text);
        }
    }
}
