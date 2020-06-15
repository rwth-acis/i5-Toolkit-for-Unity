using System.Collections;
using System.Collections.Generic;
using i5.Toolkit.Utilities;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Tests.Utilities
{
    /// <summary>
    /// Tests for the conversion utils class
    /// </summary>
    public class ConversionUtilsTests
    {
        /// <summary>
        /// Resets the scene before each test
        /// </summary>
        [SetUp]
        public void ResetScene()
        {
            EditorSceneManager.OpenScene("Assets/i5 Toolkit/Tests/TestResources/SetupTestScene.unity");
        }

        /// <summary>
        /// Checks if the zero vector is correctly converted
        /// </summary>
        [Test]
        public void Vector3ToColor_Zero_ColorComponentsMatch()
        {
            Vector3 v = Vector3.zero;
            Color c = v.ToColor();

            Assert.AreEqual(v.x, c.r);
            Assert.AreEqual(v.y, c.g);
            Assert.AreEqual(v.z, c.b);
            Assert.AreEqual(1, c.a);
        }

        /// <summary>
        /// Checks if components smaller or equal to one are converted correctly
        /// </summary>
        [Test]
        public void Vector3ToColor_Components1_ColorComponentsMatch()
        {
            Vector3 v = new Vector3(1, 1, 1);
            Color c = v.ToColor();

            Assert.AreEqual(v.x, c.r);
            Assert.AreEqual(v.y, c.g);
            Assert.AreEqual(v.z, c.b);
            Assert.AreEqual(1, c.a);
        }

        /// <summary>
        /// Checks if components bigger than one are converted correctly
        /// </summary>
        [Test]
        public void Vector3ToColor_ComponentsBigger1_ColorComponentsMatch()
        {
            Vector3 v = new Vector3(10, 100, 1000);
            Color c = v.ToColor();

            Assert.AreEqual(v.x, c.r);
            Assert.AreEqual(v.y, c.g);
            Assert.AreEqual(v.z, c.b);
            Assert.AreEqual(1, c.a);
        }

        /// <summary>
        /// Checks if negative components are converted correctly
        /// </summary>
        [Test]
        public void Vector3ToColor_ComponentsNegative_ColorComponentsMatch()
        {
            Vector3 v = new Vector3(-1, -1, -1);
            Color c = v.ToColor();

            Assert.AreEqual(v.x, c.r);
            Assert.AreEqual(v.y, c.g);
            Assert.AreEqual(v.z, c.b);
            Assert.AreEqual(1, c.a);
        }


        /// <summary>
        /// Checks if the zero vector is correctly converted
        /// </summary>
        [Test]
        public void Vector4ToColor_Zero_ColorComponentsMatch()
        {
            Vector4 v = Vector4.zero;
            Color c = v.ToColor();

            Assert.AreEqual(v.x, c.r);
            Assert.AreEqual(v.y, c.g);
            Assert.AreEqual(v.z, c.b);
            Assert.AreEqual(v.w, c.a);
        }

        /// <summary>
        /// Checks if components smaller or equal to one are converted correctly
        /// </summary>
        [Test]
        public void Vector4ToColor_Components1_ColorComponentsMatch()
        {
            Vector4 v = new Vector4(1, 1, 1, 1);
            Color c = v.ToColor();

            Assert.AreEqual(v.x, c.r);
            Assert.AreEqual(v.y, c.g);
            Assert.AreEqual(v.z, c.b);
            Assert.AreEqual(v.w, c.a);
        }

        /// <summary>
        /// Checks if components bigger than one are converted correctly
        /// </summary>
        [Test]
        public void Vector4ToColor_ComponentsBigger1_ColorComponentsMatch()
        {
            Vector4 v = new Vector4(10, 100, 1000, 10000);
            Color c = v.ToColor();

            Assert.AreEqual(v.x, c.r);
            Assert.AreEqual(v.y, c.g);
            Assert.AreEqual(v.z, c.b);
            Assert.AreEqual(v.w, c.a);
        }

        /// <summary>
        /// Checks if negative components are converted correctly
        /// </summary>
        [Test]
        public void Vector4ToColor_ComponentsNegative_ColorComponentsMatch()
        {
            Vector4 v = new Vector4(-1, -1, -1, -1);
            Color c = v.ToColor();

            Assert.AreEqual(v.x, c.r);
            Assert.AreEqual(v.y, c.g);
            Assert.AreEqual(v.z, c.b);
            Assert.AreEqual(v.w, c.a);
        }

        /// <summary>
        /// Checks if black is correctly converted
        /// </summary>
        [Test]
        public void ColorToVector3_Zero_VectorComponentsMatch()
        {
            Color c = new Color(0, 0, 0, 0);
            Vector3 v = c.ToVector3();

            Assert.AreEqual(c.r, v.x);
            Assert.AreEqual(c.g, v.y);
            Assert.AreEqual(c.b, v.z);
        }

        /// <summary>
        /// Checks if components smaller or equal to one are converted correctly
        /// </summary>
        [Test]
        public void ColorToVector3_Components1_VectorComponentsMatch()
        {
            Color c = new Color(1, 1, 1, 1);
            Vector3 v = c.ToVector3();

            Assert.AreEqual(c.r, v.x);
            Assert.AreEqual(c.g, v.y);
            Assert.AreEqual(c.b, v.z);
        }

        /// <summary>
        /// Checks if components bigger than one are converted correctly
        /// </summary>
        [Test]
        public void ColorToVector3_ComponentsBigger1_VectorComponentsMatch()
        {
            Color c = new Color(10, 100, 1000, 10000);
            Vector3 v = c.ToVector3();

            Assert.AreEqual(c.r, v.x);
            Assert.AreEqual(c.g, v.y);
            Assert.AreEqual(c.b, v.z);
        }

        /// <summary>
        /// Checks if negative components are converted correctly
        /// </summary>
        [Test]
        public void ColorToVector3_ComponentsNegative_VectorComponentsMatch()
        {
            Color c = new Color(-1, -1, -1, -1);
            Vector3 v = c.ToVector3();

            Assert.AreEqual(c.r, v.x);
            Assert.AreEqual(c.g, v.y);
            Assert.AreEqual(c.b, v.z);
        }

        /// <summary>
        /// Checks if black is correctly converted
        /// </summary>
        [Test]
        public void ColorToVector4_Zero_VectorComponentsMatch()
        {
            Color c = new Color(0, 0, 0, 0);
            Vector4 v = c.ToVector4();

            Assert.AreEqual(c.r, v.x);
            Assert.AreEqual(c.g, v.y);
            Assert.AreEqual(c.b, v.z);
            Assert.AreEqual(c.a, v.w);
        }

        /// <summary>
        /// Checks if components smaller or equal to one are converted correctly
        /// </summary>
        [Test]
        public void ColorToVector4_Components1_VectorComponentsMatch()
        {
            Color c = new Color(1, 1, 1, 1);
            Vector4 v = c.ToVector4();

            Assert.AreEqual(c.r, v.x);
            Assert.AreEqual(c.g, v.y);
            Assert.AreEqual(c.b, v.z);
            Assert.AreEqual(c.a, v.w);
        }

        /// <summary>
        /// Checks if components bigger than one are converted correctly
        /// </summary>
        [Test]
        public void ColorToVector4_ComponentsBigger1_VectorComponentsMatch()
        {
            Color c = new Color(10, 100, 1000, 10000);
            Vector4 v = c.ToVector4();

            Assert.AreEqual(c.r, v.x);
            Assert.AreEqual(c.g, v.y);
            Assert.AreEqual(c.b, v.z);
            Assert.AreEqual(c.a, v.w);
        }

        /// <summary>
        /// Checks if negative components are converted correctly
        /// </summary>
        [Test]
        public void ColorToVector4_ComponentsNegative_VectorComponentsMatch()
        {
            Color c = new Color(-1, -1, -1, -1);
            Vector4 v = c.ToVector4();

            Assert.AreEqual(c.r, v.x);
            Assert.AreEqual(c.g, v.y);
            Assert.AreEqual(c.b, v.z);
            Assert.AreEqual(c.a, v.w);
        }
    }
}
