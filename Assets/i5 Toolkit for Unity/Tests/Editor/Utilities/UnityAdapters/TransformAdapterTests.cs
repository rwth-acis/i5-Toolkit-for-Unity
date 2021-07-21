using i5.Toolkit.Core.Editor.TestHelpers;
using i5.Toolkit.Core.Utilities.UnityAdapters;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.Tests.Utilities.UnityAdapters
{
    public class TransformAdapterTests
    {
        private Transform transform;
        private TransformAdapter transformAdapter;

        [SetUp]
        public void SetUp()
        {
            EditModeTestUtilities.ResetScene();
            GameObject go = new GameObject();
            transform = go.transform;
            transformAdapter = new TransformAdapter(transform);
        }

        [Test]
        public void GetPosition_SameAsTransform()
        {
            transform.position = new Vector3(1, 2, 3);

            Assert.AreEqual(transform.position, transformAdapter.Position);
        }

        [Test]
        public void SetPosition_AppliedToTransform()
        {
            transformAdapter.Position = new Vector3(1, 2, 3);

            Assert.AreEqual(transformAdapter.Position, transform.position);
        }

        [Test]
        public void GetLocalPosition_SameAsTransform()
        {
            transform.localPosition = new Vector3(1, 2, 3);

            Assert.AreEqual(transform.localPosition, transformAdapter.LocalPosition);
        }

        [Test]
        public void SetLocalPosition_AppliedToTransform()
        {
            transformAdapter.LocalPosition = new Vector3(1, 2, 3);

            Assert.AreEqual(transformAdapter.LocalPosition, transform.localPosition);
        }

        [Test]
        public void GetRotation_PositiveValues_SameAsTransform()
        {
            transform.rotation = Quaternion.Euler(90, 28, 45);

            Assert.AreEqual(transform.rotation, transformAdapter.Rotation);
        }

        [Test]
        public void GetRotation_NegativeValues_SameAsTransform()
        {
            transform.rotation = Quaternion.Euler(-45, 0, -185);

            Assert.AreEqual(transform.rotation, transformAdapter.Rotation);
        }

        [Test]
        public void SetRotation_PositiveValues_AppliedToTransform()
        {
            transformAdapter.Rotation = Quaternion.Euler(45, 90, 175);

            Assert.AreEqual(transformAdapter.Rotation, transform.rotation);
        }

        [Test]
        public void SetRotation_NegativeValues_AppliedToTransform()
        {
            transformAdapter.Rotation = Quaternion.Euler(-45, -95, -240);

            Assert.AreEqual(transformAdapter.Rotation, transform.rotation);
        }

        [Test]
        public void GetLocalRotation_PositiveValues_SameAsTransform()
        {
            transform.localRotation = Quaternion.Euler(90, 28, 45);

            Assert.AreEqual(transform.localRotation, transformAdapter.LocalRotation);
        }

        [Test]
        public void GetLocalRotation_NegativeValues_SameAsTransform()
        {
            transform.localRotation = Quaternion.Euler(-45, 0, -185);

            Assert.AreEqual(transform.localRotation, transformAdapter.LocalRotation);
        }

        [Test]
        public void SetLocalRotation_PositiveValues_AppliedToTransform()
        {
            transformAdapter.LocalRotation = Quaternion.Euler(45, 90, 175);

            Assert.AreEqual(transformAdapter.LocalRotation, transform.localRotation);
        }

        [Test]
        public void SetLocalRotation_NegativeValues_AppliedToTransform()
        {
            transformAdapter.LocalRotation = Quaternion.Euler(-45, -95, -240);

            Assert.AreEqual(transformAdapter.LocalRotation, transform.localRotation);
        }

        [Test]
        public void GetEulerAngles_PositiveValues_SameAsTransform()
        {
            transform.eulerAngles = new Vector3(90, 28, 45);

            Assert.AreEqual(transform.eulerAngles, transformAdapter.EulerAngles);
        }

        [Test]
        public void GetEulerAngles_NegativeValues_SameAsTransform()
        {
            transform.eulerAngles = new Vector3(-45, 0, -185);

            Assert.AreEqual(transform.eulerAngles, transformAdapter.EulerAngles);
        }

        [Test]
        public void SetEulerAngles_PositiveValues_AppliedToTransform()
        {
            transformAdapter.EulerAngles = new Vector3(45, 90, 175);

            Assert.AreEqual(transformAdapter.EulerAngles, transform.eulerAngles);
        }

        [Test]
        public void SetEulerAngles_NegativeValues_AppliedToTransform()
        {
            transformAdapter.EulerAngles = new Vector3(-45, -95, -240);

            Assert.AreEqual(transformAdapter.EulerAngles, transform.eulerAngles);
        }

        [Test]
        public void GetLocalEulerAngles_PositiveValues_SameAsTransform()
        {
            transform.localEulerAngles = new Vector3(90, 28, 45);

            Assert.AreEqual(transform.localEulerAngles, transformAdapter.LocalEulerAngles);
        }

        [Test]
        public void GetLocalEulerAngles_NegativeValues_SameAsTransform()
        {
            transform.localEulerAngles = new Vector3(-45, 0, -185);

            Assert.AreEqual(transform.localEulerAngles, transformAdapter.LocalEulerAngles);
        }

        [Test]
        public void SetLocalEulerAngles_PositiveValues_AppliedToTransform()
        {
            transformAdapter.LocalEulerAngles = new Vector3(45, 90, 175);

            Assert.AreEqual(transformAdapter.LocalEulerAngles, transform.localEulerAngles);
        }

        [Test]
        public void SetLocalEulerAngles_NegativeValues_AppliedToTransform()
        {
            transformAdapter.LocalEulerAngles = new Vector3(-45, -95, -240);

            Assert.AreEqual(transformAdapter.LocalEulerAngles, transform.localEulerAngles);
        }

        [Test]
        public void GetLocalScale_SameAsTransform()
        {
            transform.localScale = new Vector3(2, 0.2f, 5);

            Assert.AreEqual(transform.localScale, transformAdapter.LocalScale);
        }

        [Test]
        public void SetLocalScale_AppliedToTransform()
        {
            transformAdapter.LocalScale = new Vector3(2, 0.2f, 5);

            Assert.AreEqual(transformAdapter.LocalScale, transform.localScale);
        }

        [Test]
        public void GetLossyScale_SameAsTransform()
        {
            transform.localScale = new Vector3(2, 0.2f, 5);

            Assert.AreEqual(transform.lossyScale, transformAdapter.LossyScale);
        }
    }
}
