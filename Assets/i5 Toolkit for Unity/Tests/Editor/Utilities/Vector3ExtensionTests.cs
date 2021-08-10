using System.Collections;
using System.Collections.Generic;
using i5.Toolkit.Core.Utilities;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Core.Tests.Utilities
{
    public class Vector3ExtensionTests
    {
        [TestCase(1, 1, 1)]
        [TestCase(0, 2, 0)]
        [TestCase(3, 0, 0)]
        [TestCase(2, 3, 6)]
        [TestCase(5, 2, 10)]
        public void MultiplyComponentWise_XValueCorrect(float value1, float value2, float expected)
        {
            Vector3 vector1 = new Vector3(value1, 0, 0);
            Vector3 vector2 = new Vector3(value2, 0, 0);

            Vector3 result = vector1.MultiplyComponentWise(vector2);

            Assert.AreEqual(expected, result.x);
        }

        [TestCase(1, 1, 1)]
        [TestCase(0, 2, 0)]
        [TestCase(3, 0, 0)]
        [TestCase(2, 3, 6)]
        [TestCase(5, 2, 10)]
        public void MultiplyComponentWise_YValueCorrect(float value1, float value2, float expected)
        {
            Vector3 vector1 = new Vector3(0, value1, 0);
            Vector3 vector2 = new Vector3(0, value2, 0);

            Vector3 result = vector1.MultiplyComponentWise(vector2);

            Assert.AreEqual(expected, result.y);
        }

        [TestCase(1, 1, 1)]
        [TestCase(0, 2, 0)]
        [TestCase(3, 0, 0)]
        [TestCase(2, 3, 6)]
        [TestCase(5, 2, 10)]
        public void MultiplyComponentWise_ZValueCorrect(float value1, float value2, float expected)
        {
            Vector3 vector1 = new Vector3(0, 0, value1);
            Vector3 vector2 = new Vector3(0, 0, value2);

            Vector3 result = vector1.MultiplyComponentWise(vector2);

            Assert.AreEqual(expected, result.z);
        }

        [TestCase(1, 1, 1)]
        [TestCase(0, 2, 0)]
        [TestCase(3, 1, 3)]
        [TestCase(10, 2, 5)]
        [TestCase(16, 4, 4)]
        public void DivideComponentWiseBy_XValueCorrect(float value1, float value2, float expected)
        {
            Vector3 vector1 = new Vector3(value1, 0, 0);
            Vector3 vector2 = new Vector3(value2, 0, 0);

            Vector3 result = vector1.DivideComponentWiseBy(vector2);

            Assert.AreEqual(expected, result.x);
        }

        [TestCase(1, 1, 1)]
        [TestCase(0, 2, 0)]
        [TestCase(3, 1, 3)]
        [TestCase(10, 2, 5)]
        [TestCase(16, 4, 4)]
        public void DivideComponentWiseBy_YValueCorrect(float value1, float value2, float expected)
        {
            Vector3 vector1 = new Vector3(0, value1, 0);
            Vector3 vector2 = new Vector3(0, value2, 0);

            Vector3 result = vector1.DivideComponentWiseBy(vector2);

            Assert.AreEqual(expected, result.y);
        }

        [TestCase(1, 1, 1)]
        [TestCase(0, 2, 0)]
        [TestCase(3, 1, 3)]
        [TestCase(10, 2, 5)]
        [TestCase(16, 4, 4)]
        public void DivideComponentWiseBy_ZValueCorrect(float value1, float value2, float expected)
        {
            Vector3 vector1 = new Vector3(0, 0, value1);
            Vector3 vector2 = new Vector3(0, 0, value2);

            Vector3 result = vector1.DivideComponentWiseBy(vector2);

            Assert.AreEqual(expected, result.z);
        }

        [TestCase(0, 1, 2)]
        [TestCase(-1, 1, 2)]
        [TestCase(-5, -3, -1)]
        public void MinimumComponent_MinimumInX_GetsMinimum(int x, int y, int z)
        {
            Vector3 vector = new Vector3(x, y, z);

            Assert.AreEqual(Mathf.Min(x, y, z), vector.MinimumComponent());
        }

        [TestCase(1, 0, 2)]
        [TestCase(0, -1, 2)]
        [TestCase(-3, -5, -1)]
        public void MinimumComponent_MinimumInY_GetsMinimum(int x, int y, int z)
        {
            Vector3 vector = new Vector3(x, y, z);

            Assert.AreEqual(Mathf.Min(x, y, z), vector.MinimumComponent());
        }

        [TestCase(2, 1, 0)]
        [TestCase(1, 2, 0)]
        [TestCase(-1, -3, -5)]
        public void MinimumComponent_MinimumInZ_GetsMinimum(int x, int y, int z)
        {
            Vector3 vector = new Vector3(x, y, z);

            Assert.AreEqual(Mathf.Min(x, y, z), vector.MinimumComponent());
        }

        [TestCase(2, 1, 0)]
        [TestCase(2, 1, -2)]
        [TestCase(-1, -3, -5)]
        public void MaximumComponent_MaximumInX_GetsMaximum(int x, int y, int z)
        {
            Vector3 vector = new Vector3(x, y, z);

            Assert.AreEqual(Mathf.Max(x, y, z), vector.MaximumComponent());
        }

        [TestCase(1, 2, 0)]
        [TestCase(0, 2, -1)]
        [TestCase(-3, -1, -5)]
        public void MaximumComponent_MaximumInY_GetsMaximum(int x, int y, int z)
        {
            Vector3 vector = new Vector3(x, y, z);

            Assert.AreEqual(Mathf.Max(x, y, z), vector.MaximumComponent());
        }

        [TestCase(1, 2, 0)]
        [TestCase(-1, 2, 0)]
        [TestCase(-5, -1, -3)]
        public void MaximumComponent_MaximumInZ_GetsMaximum(int x, int y, int z)
        {
            Vector3 vector = new Vector3(x, y, z);

            Assert.AreEqual(Mathf.Max(x, y, z), vector.MaximumComponent());
        }
    }
}
