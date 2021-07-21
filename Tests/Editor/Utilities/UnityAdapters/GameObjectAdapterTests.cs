using i5.Toolkit.Core.Editor.TestHelpers;
using i5.Toolkit.Core.Experimental.UnityAdapters;
using NUnit.Framework;
using UnityEngine;

namespace i5.Toolkit.Core.Tests.Experimental.UnityAdapters
{
    public class GameObjectAdapterTests
    {
        private GameObject go;
        private GameObjectAdapter goAdapter;

        [SetUp]
        public void SetUp()
        {
            EditModeTestUtilities.ResetScene();
            go = new GameObject();
            goAdapter = new GameObjectAdapter(go);
        }

        [Test]
        public void GetActiveSelf()
        {
            Assert.AreEqual(go.activeSelf, goAdapter.ActiveSelf);
        }

        [Test]
        public void SetActiveSelf_True()
        {
            goAdapter.ActiveSelf = true;

            Assert.IsTrue(go.activeSelf);
        }

        [Test]
        public void SetActiveSelf_False()
        {
            goAdapter.ActiveSelf = false;

            Assert.IsFalse(go.activeSelf);
        }

        [Test]
        public void GetActiveInHierarchy()
        {
            Assert.AreEqual(go.activeInHierarchy, goAdapter.ActiveInHierarchy);
        }

        [Test]
        public void SetActiveInHierarchy_True()
        {
            goAdapter.ActiveInHierarchy = true;

            Assert.IsTrue(go.activeInHierarchy);
        }

        [Test]
        public void SetActiveInHierarchy_False()
        {
            goAdapter.ActiveInHierarchy = false;
            Assert.IsFalse(go.activeInHierarchy);
        }
    }
}
