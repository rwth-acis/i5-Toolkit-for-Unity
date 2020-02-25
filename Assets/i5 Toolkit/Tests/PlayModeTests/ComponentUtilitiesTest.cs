using System.Collections;
using System.Collections.Generic;
using i5.Toolkit.Utilities;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ComponentUtilitiesTest
    {
        private GameObject testObject;

        [SetUp]
        public void Setup()
        {
            testObject = new GameObject("TestObject");
        }

        [Test]
        public void GetOrAddComponent_ComponentDoesNotExist()
        {
            MeshFilter res = ComponentUtilities.GetOrAddComponent<MeshFilter>(testObject);
            Assert.NotNull(res);
            MeshFilter checkRes = testObject.GetComponent<MeshFilter>();
            Assert.NotNull(checkRes);
            Assert.AreEqual(res, checkRes);

            GameObject.Destroy(checkRes);
        }

        [Test]
        public void GetOrAddComponent_ComponentExists()
        {
            MeshFilter addedComponent = testObject.AddComponent<MeshFilter>();

            MeshFilter res = ComponentUtilities.GetOrAddComponent<MeshFilter>(testObject);
            Assert.NotNull(res);
            Assert.AreEqual(addedComponent, res);

            GameObject.Destroy(addedComponent);
        }

        [TearDown]
        public void TearDown()
        {
            GameObject.Destroy(testObject);
        }
    }
}
