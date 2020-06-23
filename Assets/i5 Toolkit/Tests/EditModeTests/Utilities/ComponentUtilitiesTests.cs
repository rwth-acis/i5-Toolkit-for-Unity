using i5.Toolkit.TestUtilities;
using i5.Toolkit.Utilities;
using NUnit.Framework;
using UnityEngine;

namespace i5.Toolkit.Tests.Utilities
{
    /// <summary>
    /// Tests for the Component Utilities
    /// </summary>
    public class ComponentUtilitiesTests
    {
        /// <summary>
        /// Test object which is generated before each test
        /// </summary>
        private GameObject testObject;

        /// <summary>
        /// Resets the scene to the standard test scene before executed each test
        /// </summary>
        [SetUp]
        public void ResetScene()
        {
            EditModeTestUtilities.ResetScene();
            testObject = new GameObject("TestObject");
        }

        /// <summary>
        /// Checks if a component is added if it does not exist
        /// </summary>
        [Test]
        public void GetOrAddComponent_ComponentDoesNotExist_ComponentAdded()
        {
            MeshFilter res = ComponentUtilities.GetOrAddComponent<MeshFilter>(testObject);
            Assert.NotNull(res);
            MeshFilter checkRes = testObject.GetComponent<MeshFilter>();
            Assert.NotNull(checkRes);
            Assert.AreEqual(res, checkRes);
        }

        /// <summary>
        /// Checks if a component is found if it already exists
        /// </summary>
        [Test]
        public void GetOrAddComponent_ComponentExists_FindsComponent()
        {
            MeshFilter addedComponent = testObject.AddComponent<MeshFilter>();

            MeshFilter res = ComponentUtilities.GetOrAddComponent<MeshFilter>(testObject);
            Assert.NotNull(res);
            Assert.AreEqual(addedComponent, res);
        }
    }
}
