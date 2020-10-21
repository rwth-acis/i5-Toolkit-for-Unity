using System.Collections;
using System.Collections.Generic;
using i5.Toolkit.Core.AppConsole;
using i5.Toolkit.Core.Editor.TestHelpers;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Core.Tests.AppConsole
{
    public class ActivationVisibilityManagerTests
    {
        [SetUp]
        public void SetUp()
        {
            EditModeTestUtilities.ResetScene();
        }

        [Test]
        public void Visibility_SetTrue_ActivatesGameObject()
        {
            GameObject go = new GameObject();
            ActivationVisibilityManager visibilityManager = go.AddComponent<ActivationVisibilityManager>();
            go.SetActive(false);

            visibilityManager.IsVisible = true;

            Assert.IsTrue(go.activeSelf);
        }

        [Test]
        public void Visibility_SetFalse_DeactivatesGameObject()
        {
            GameObject go = new GameObject();
            ActivationVisibilityManager visibilityManager = go.AddComponent<ActivationVisibilityManager>();
            go.SetActive(true);

            visibilityManager.IsVisible = false;

            Assert.IsFalse(go.activeSelf);
        }

        [Test]
        public void Visibility_Get_ReturnsActiveSelf()
        {
            GameObject go = new GameObject();
            ActivationVisibilityManager visibilityManager = go.AddComponent<ActivationVisibilityManager>();
            go.SetActive(false);
            Assert.IsFalse(visibilityManager.IsVisible);

            go.SetActive(true);
            Assert.IsTrue(visibilityManager.IsVisible);
        }

        [Test]
        public void Visibility_GetParentDeactivated_ReturnsActiveSelf()
        {
            GameObject go = new GameObject();
            ActivationVisibilityManager visibilityManager = go.AddComponent<ActivationVisibilityManager>();

            GameObject parent = new GameObject("Parent");
            go.transform.parent = parent.transform;

            parent.SetActive(false);

            Assert.IsTrue(visibilityManager.IsVisible);
        }
    }
}
