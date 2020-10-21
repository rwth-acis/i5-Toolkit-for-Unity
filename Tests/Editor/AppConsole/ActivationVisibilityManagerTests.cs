using System.Collections;
using System.Collections.Generic;
using i5.Toolkit.Core.AppConsole;
using i5.Toolkit.Core.Editor.TestHelpers;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Core.Tests.AppConsole
{
    /// <summary>
    /// Tests for the ActivationVisibilityManager
    /// </summary>
    public class ActivationVisibilityManagerTests
    {
        /// <summary>
        /// Resets the scene for each test
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            EditModeTestUtilities.ResetScene();
        }

        /// <summary>
        /// Checks that setting the visibility to true activates the GameObject
        /// </summary>
        [Test]
        public void IsVisible_SetTrue_ActivatesGameObject()
        {
            GameObject go = new GameObject();
            ActivationVisibilityManager visibilityManager = go.AddComponent<ActivationVisibilityManager>();
            go.SetActive(false);

            visibilityManager.IsVisible = true;

            Assert.IsTrue(go.activeSelf);
        }

        /// <summary>
        /// Checks that setting the visibility to false deactivates the GameObject
        /// </summary>
        [Test]
        public void IsVisible_SetFalse_DeactivatesGameObject()
        {
            GameObject go = new GameObject();
            ActivationVisibilityManager visibilityManager = go.AddComponent<ActivationVisibilityManager>();
            go.SetActive(true);

            visibilityManager.IsVisible = false;

            Assert.IsFalse(go.activeSelf);
        }

        /// <summary>
        /// Checks that IsVisible returns the active self state of the GameObject
        /// </summary>
        [Test]
        public void IsVisible_Get_ReturnsActiveSelf()
        {
            GameObject go = new GameObject();
            ActivationVisibilityManager visibilityManager = go.AddComponent<ActivationVisibilityManager>();
            go.SetActive(false);
            Assert.IsFalse(visibilityManager.IsVisible);

            go.SetActive(true);
            Assert.IsTrue(visibilityManager.IsVisible);
        }

        /// <summary>
        /// Checks that IsVisible returns the active self state of the GameObject, even if the parent GameObject is deactivated
        /// </summary>
        [Test]
        public void IsVisible_GetParentDeactivated_ReturnsActiveSelf()
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
