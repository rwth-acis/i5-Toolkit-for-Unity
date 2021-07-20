using i5.Toolkit.Core.Editor.TestHelpers;
using i5.Toolkit.Core.Utilities.UnityAdapters;
using NUnit.Framework;
using UnityEngine;

namespace i5.Toolkit.Core.Tests.Utilities.UnityAdapters
{
    public class RectTransformAdapterTests
    {
        private RectTransform rectTransform;
        private RectTransformAdapter rectTransformAdapter;

        [SetUp]
        public void SetUp()
        {
            EditModeTestUtilities.ResetScene();
            GameObject go = new GameObject("GameObject", typeof(RectTransform));
            rectTransform = go.GetComponent<RectTransform>();
            rectTransformAdapter = new RectTransformAdapter(rectTransform);
        }

        [Test]
        public void GetSize()
        {
            rectTransform.sizeDelta = new Vector2(0.5f, 2f);

            Assert.AreEqual(rectTransform.sizeDelta, rectTransformAdapter.Size);
        }

        [Test]
        public void SetSize()
        {
            Vector2 targetSize = new Vector2(2f, 3f);
            rectTransformAdapter.Size = targetSize;

            Assert.AreEqual(targetSize, rectTransform.sizeDelta);
        }
    }
}
