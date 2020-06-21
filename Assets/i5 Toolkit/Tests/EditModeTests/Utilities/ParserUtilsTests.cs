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
    /// Tests for the parser utils tests
    /// </summary>
    public class ParserUtilsTests
    {
        /// <summary>
        /// Resets the scene before each test
        /// </summary>
        [SetUp]
        public void ResetScene()
        {
            EditorSceneManager.OpenScene(PathUtils.GetPackagePath() + "Tests/TestResources/SetupTestScene.unity");
        }

        #region TryParseSpaceSeparatedVector2

        /// <summary>
        /// Checks that a vector in the correct format is parsed
        /// </summary>
        [Test]
        public void TryParseSpaceSeparatedVector2_CorrectFormat_ReturnsTrue()
        {
            string vString = "1 1";
            bool res = ParserUtils.TryParseSpaceSeparatedVector2(vString, out Vector2 vector);

            Assert.IsTrue(res);
        }

        /// <summary>
        /// Checks that a vector in the correct format gives the correct value
        /// </summary>
        [Test]
        public void TryParseSpaceSeparatedVector2_CorrectFormat_OutParamCorrect()
        {
            string vString = "1 1";
            bool res = ParserUtils.TryParseSpaceSeparatedVector2(vString, out Vector2 vector);

            Assert.AreEqual(new Vector2(1,1), vector);
        }

        /// <summary>
        /// Checks that invalid strings in the right format return false
        /// </summary>
        [Test]
        public void TryParseSpaceSeparatedVector2_InvalidNumber_ReturnsFalse()
        {
            string vString = "1 s";
            bool res = ParserUtils.TryParseSpaceSeparatedVector2(vString, out Vector2 vector);

            Assert.IsFalse(res);
        }

        /// <summary>
        /// Checks that invalid strings in the right format give a default value
        /// </summary>
        [Test]
        public void TryParseSpaceSeparatedVector2_InvalidNumber_OutParamDefault()
        {
            string vString = "1 s";
            bool res = ParserUtils.TryParseSpaceSeparatedVector2(vString, out Vector2 vector);

            Assert.AreEqual(default(Vector2), vector);
        }

        /// <summary>
        /// Checks that strings with just one number return false
        /// </summary>
        [Test]
        public void TryParseSpaceSeparatedVector2_OneNumber_ReturnsFalse()
        {
            string vString = "1";
            bool res = ParserUtils.TryParseSpaceSeparatedVector2(vString, out Vector2 vector);

            Assert.IsFalse(res);
        }

        /// <summary>
        /// Checks that strings with just one number give a default value
        /// </summary>
        [Test]
        public void TryParseSpaceSeparatedVector2_OneNumber_OutParamDefault()
        {
            string vString = "1";
            bool res = ParserUtils.TryParseSpaceSeparatedVector2(vString, out Vector2 vector);

            Assert.AreEqual(default(Vector2), vector);
        }

        /// <summary>
        /// Checks that a string with three numbers returns true
        /// </summary>
        [Test]
        public void TryParseSpaceSeparatedVector2_ThreeNumbers_ReturnsTrue()
        {
            string vString = "1 1 1";
            bool res = ParserUtils.TryParseSpaceSeparatedVector2(vString, out Vector2 vector);

            Assert.IsTrue(res);
        }

        /// <summary>
        /// Checks that a string with three numbers give a default value
        /// </summary>
        [Test]
        public void TryParseSpaceSeparatedVector2_ThreeNumbers_OutParamCorrect()
        {
            string vString = "1 1 1";
            bool res = ParserUtils.TryParseSpaceSeparatedVector2(vString, out Vector2 vector);

            Assert.AreEqual(new Vector2(1,1), vector);
        }

        /// <summary>
        /// Checks that a string with four numbers returns false
        /// </summary>
        [Test]
        public void TryParseSpaceSeparatedVector2_FourNumbers_ReturnsFalse()
        {
            string vString = "1 1 1 1";
            bool res = ParserUtils.TryParseSpaceSeparatedVector2(vString, out Vector2 vector);

            Assert.IsFalse(res);
        }

        /// <summary>
        /// Checks that a string with four numbers gives a default value
        /// </summary>
        [Test]
        public void TryParseSpaceSeparatedVector2_FourNumbers_OutParamDefault()
        {
            string vString = "1 1 1 1";
            bool res = ParserUtils.TryParseSpaceSeparatedVector2(vString, out Vector2 vector);

            Assert.AreEqual(default(Vector2), vector);
        }

        /// <summary>
        /// Checks that a string in the invalid format returns false
        /// </summary>
        [Test]
        public void TryParseSpaceSeparatedVector2_InvalidFormat_ReturnsFalse()
        {
            string vString = "hello";
            bool res = ParserUtils.TryParseSpaceSeparatedVector2(vString, out Vector2 vector);

            Assert.IsFalse(res);
        }

        /// <summary>
        /// Checks that a string in the invalid format give a default value
        /// </summary>
        [Test]
        public void TryParseSpaceSeparatedVector2_InvalidFormat_OutParamDefault()
        {
            string vString = "hello";
            bool res = ParserUtils.TryParseSpaceSeparatedVector2(vString, out Vector2 vector);

            Assert.AreEqual(default(Vector2), vector);
        }

        /// <summary>
        /// Checks that a string where numbers are separated by multiple spaces return true
        /// </summary>
        [Test]
        public void TryParseSpaceSeparatedVector2_VectorWithTwoSpaces_ReturnsTrue()
        {
            string vString = "1  1";
            bool res = ParserUtils.TryParseSpaceSeparatedVector2(vString, out Vector2 vector);

            Assert.IsTrue(res);
        }

        /// <summary>
        /// Checks that a string where numbers are separated by multiple spaces gives the correct value
        /// </summary>
        [Test]
        public void TryParseSpaceSeparatedVector2_VectorWithTwoSpaces_OutParamCorrect()
        {
            string vString = "1  1";
            bool res = ParserUtils.TryParseSpaceSeparatedVector2(vString, out Vector2 vector);

            Assert.AreEqual(new Vector2(1,1), vector);
        }

        #endregion

        #region TryParseSpaceSeparatedVector3

        /// <summary>
        /// Checks that a vector in the correct format is parsed
        /// </summary>
        [Test]
        public void TryParseSpaceSeparatedVector3_CorrectFormat_ReturnsTrue()
        {
            string vString = "1 1 1";
            bool res = ParserUtils.TryParseSpaceSeparatedVector3(vString, out Vector3 vector);

            Assert.IsTrue(res);
        }

        /// <summary>
        /// Checks that a vector in the correct format gives the correct value
        /// </summary>
        [Test]
        public void TryParseSpaceSeparatedVector3_CorrectFormat_OutParamCorrect()
        {
            string vString = "1 1 1";
            bool res = ParserUtils.TryParseSpaceSeparatedVector3(vString, out Vector3 vector);

            Assert.AreEqual(new Vector3(1, 1, 1), vector);
        }

        /// <summary>
        /// Checks that invalid strings in the right format return false
        /// </summary>
        [Test]
        public void TryParseSpaceSeparatedVector3_InvalidNumber_ReturnsFalse()
        {
            string vString = "1 s 1";
            bool res = ParserUtils.TryParseSpaceSeparatedVector3(vString, out Vector3 vector);

            Assert.IsFalse(res);
        }

        /// <summary>
        /// Checks that invalid strings in the right format give a default value
        /// </summary>
        [Test]
        public void TryParseSpaceSeparatedVector3_InvalidNumber_OutParamDefault()
        {
            string vString = "1 s 1";
            bool res = ParserUtils.TryParseSpaceSeparatedVector3(vString, out Vector3 vector);

            Assert.AreEqual(default(Vector3), vector);
        }

        /// <summary>
        /// Checks that strings with just one number return false
        /// </summary>
        [Test]
        public void TryParseSpaceSeparatedVector3_OneNumber_ReturnsFalse()
        {
            string vString = "1";
            bool res = ParserUtils.TryParseSpaceSeparatedVector3(vString, out Vector3 vector);

            Assert.IsFalse(res);
        }

        /// <summary>
        /// Checks that strings with just one number give a default value
        /// </summary>
        [Test]
        public void TryParseSpaceSeparatedVector3_OneNumber_OutParamDefault()
        {
            string vString = "1";
            bool res = ParserUtils.TryParseSpaceSeparatedVector3(vString, out Vector3 vector);

            Assert.AreEqual(default(Vector3), vector);
        }

        /// <summary>
        /// Checks that a string with three numbers returns false
        /// </summary>
        [Test]
        public void TryParseSpaceSeparatedVector3_TwoNumbers_ReturnsFalse()
        {
            string vString = "1 1";
            bool res = ParserUtils.TryParseSpaceSeparatedVector3(vString, out Vector3 vector);

            Assert.IsFalse(res);
        }

        /// <summary>
        /// Checks that a string with two numbers gives a default value
        /// </summary>
        [Test]
        public void TryParseSpaceSeparatedVector3_TwoNumbers_OutParamCorrect()
        {
            string vString = "1 1";
            bool res = ParserUtils.TryParseSpaceSeparatedVector3(vString, out Vector3 vector);

            Assert.AreEqual(default(Vector3), vector);
        }

        /// <summary>
        /// Checks that a string with four numbers returns true
        /// </summary>
        [Test]
        public void TryParseSpaceSeparatedVector3_FourNumbers_ReturnsTrue()
        {
            string vString = "1 1 1 1";
            bool res = ParserUtils.TryParseSpaceSeparatedVector3(vString, out Vector3 vector);

            Assert.IsTrue(res);
        }

        /// <summary>
        /// Checks that a string with four numbers gives the correct value
        /// </summary>
        [Test]
        public void TryParseSpaceSeparatedVector3_FourNumbers_OutParamCorrect()
        {
            string vString = "1 1 1 1";
            bool res = ParserUtils.TryParseSpaceSeparatedVector3(vString, out Vector3 vector);

            Assert.AreEqual(new Vector3(1,1,1), vector);
        }

        /// <summary>
        /// Checks that a string in the invalid format returns false
        /// </summary>
        [Test]
        public void TryParseSpaceSeparatedVector3_InvalidFormat_ReturnsFalse()
        {
            string vString = "hello";
            bool res = ParserUtils.TryParseSpaceSeparatedVector3(vString, out Vector3 vector);

            Assert.IsFalse(res);
        }

        /// <summary>
        /// Checks that a string in the invalid format give a default value
        /// </summary>
        [Test]
        public void TryParseSpaceSeparatedVector3_InvalidFormat_OutParamDefault()
        {
            string vString = "hello";
            bool res = ParserUtils.TryParseSpaceSeparatedVector3(vString, out Vector3 vector);

            Assert.AreEqual(default(Vector3), vector);
        }

        /// <summary>
        /// Checks that a string where numbers are separated by multiple spaces return true
        /// </summary>
        [Test]
        public void TryParseSpaceSeparatedVector3_VectorWithTwoSpaces_ReturnsTrue()
        {
            string vString = "1  1 1";
            bool res = ParserUtils.TryParseSpaceSeparatedVector3(vString, out Vector3 vector);

            Assert.IsTrue(res);
        }

        /// <summary>
        /// Checks that a string where numbers are separated by multiple spaces gives the correct value
        /// </summary>
        [Test]
        public void TryParseSpaceSeparatedVector3_VectorWithTwoSpaces_OutParamCorrect()
        {
            string vString = "1  1 1";
            bool res = ParserUtils.TryParseSpaceSeparatedVector3(vString, out Vector3 vector);

            Assert.AreEqual(new Vector3(1, 1, 1), vector);
        }

        #endregion
    }
}
