using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using i5.Toolkit.Core.TestUtilities;
using i5.Toolkit.Core.Utilities;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Core.Tests.Utilities
{
    /// <summary>
    /// Tests for the ObjectPool
    /// </summary>
    public class ObjectPoolTests
    {
        /// <summary>
        /// Loads a new scene for each test
        /// </summary>
        [SetUp]
        public void ResetScene()
        {
            EditModeTestUtilities.ResetScene();
        }

        /// <summary>
        /// Checks if an id in the correct range is created if a new pool is created
        /// </summary>
        [Test]
        public void CreatePool_SinglePoolNoCapacity_ReturnsIdBigger0()
        {
            int poolId = ObjectPool<TestObject>.CreateNewPool();

            Assert.IsTrue(poolId > 0);

            // teardown
            ObjectPool<TestObject>.RemovePool(poolId, null);
        }

        /// <summary>
        /// Checks if an id in the correct range is created if a new pool with a pre-defined capacity is created
        /// </summary>
        [Test]
        public void CreatePool_SinglePoolCapacity_ReturnsIdBigger0()
        {
            int poolId = ObjectPool<TestObject>.CreateNewPool(10);

            Assert.IsTrue(poolId > 0);

            // teardown
            ObjectPool<TestObject>.RemovePool(poolId, null);
        }

        /// <summary>
        /// Checks if the pool ids are incremented if multiple pools are created
        /// </summary>
        [Test]
        public void CreatePool_MultiplePools_IdsIncrement()
        {
            int pool1 = ObjectPool<TestObject>.CreateNewPool();
            int pool2 = ObjectPool<TestObject>.CreateNewPool();
            Assert.IsTrue(pool1 == (pool2 - 1));

            // teardown
            ObjectPool<TestObject>.RemovePool(pool1, null);
            ObjectPool<TestObject>.RemovePool(pool2, null);
        }

        /// <summary>
        /// Checks if the created pool is accessible
        /// </summary>
        [Test]
        public void CreatePool_SinglePool_Accessible()
        {
            int poolId = ObjectPool<TestObject>.CreateNewPool();
            Assert.DoesNotThrow(delegate
           {
               ObjectPool<TestObject>.RequestResource(poolId, () => { return new TestObject(1); });
           });
        }

        /// <summary>
        /// Checks if a removed pool is not accessible anymore
        /// </summary>
        [Test]
        public void RemovePool_CreateAndRemovePool_PoolNotAccessibleAnymore()
        {
            int poolId = ObjectPool<TestObject>.CreateNewPool();
            ObjectPool<TestObject>.RemovePool(poolId);
            Assert.Throws<InvalidOperationException>(delegate
            {
                ObjectPool<TestObject>.RequestResource(poolId, () => { return new TestObject(1); });
            });
        }

        /// <summary>
        /// Checks that the default pool cannot be removed
        /// </summary>
        [Test]
        public void RemovePool_RemovePool0_LogsError()
        {
            ObjectPool<TestObject>.RemovePool(0);
            LogAssert.Expect(LogType.Error, new Regex(@"\w*Cannot remove default pool\w*"));
        }

        /// <summary>
        /// Checks that a warning is logged if a pool is removed that did not exist
        /// </summary>
        [Test]
        public void RemovePool_PoolDoesNotExist_LogsWarning()
        {
            ObjectPool<TestObject>.RemovePool(-1);
            LogAssert.Expect(LogType.Warning, new Regex(@"\w*The pool with the given id could not be destroyed because it does not exist anymore.\w*"));
        }

        /// <summary>
        /// Checks that the factory method is used if a resource is requested from the empty default pool
        /// </summary>
        [Test]
        public void RequestResource_DefaultPoolAndEmpty_UsesFactory()
        {
            TestObject testObj = ObjectPool<TestObject>.RequestResource(() => { return new TestObject(42); });
            Assert.AreEqual(42, testObj.Id);
        }
        
        /// <summary>
        /// Checks that the object is returned if there is an object in the default pool and a resource is requested
        /// </summary>
        [Test]
        public void RequestResource_DefaultPoolContainsObj_ReturnsObj()
        {
            TestObject storedInPool = new TestObject(7);
            ObjectPool<TestObject>.ReleaseResource(storedInPool);

            TestObject retrieved = ObjectPool<TestObject>.RequestResource(() => { return new TestObject(42); });
            Assert.AreEqual(storedInPool, retrieved);
        }

        /// <summary>
        /// Checks that an object is selected based on FIFO if multiple objects are present in the default pool and one is requested
        /// </summary>
        [Test]
        public void RequestResource_DefaultPoolContainsMultipleObj_UsesFIFO()
        {
            TestObject obj1 = new TestObject(1);
            ObjectPool<TestObject>.ReleaseResource(obj1);
            TestObject obj2 = new TestObject(2);
            ObjectPool<TestObject>.ReleaseResource(obj2);

            TestObject retrieved = ObjectPool<TestObject>.RequestResource(() => { return new TestObject(42); });
            Assert.AreEqual(obj1, retrieved);

            TestObject retrieved2 = ObjectPool<TestObject>.RequestResource(() => { return new TestObject(42); });
            Assert.AreEqual(obj2, retrieved2);
        }

        /// <summary>
        /// Checks that the factory method is used if an object is requested from an empt custom pool
        /// </summary>
        [Test]
        public void RequestResource_CustomPoolAndEmpty_UsesFactory()
        {
            int poolId = ObjectPool<TestObject>.CreateNewPool();
            TestObject testObj = ObjectPool<TestObject>.RequestResource(poolId, () => { return new TestObject(42); });
            Assert.AreEqual(42, testObj.Id);

            // teardown
            ObjectPool<TestObject>.RemovePool(poolId);
        }

        /// <summary>
        /// Checks that the object is returned that exists in a custom pool if it is requested
        /// </summary>
        [Test]
        public void RequestResource_CustomPoolContainsObj_ReturnsObj()
        {
            int poolId = ObjectPool<TestObject>.CreateNewPool();
            TestObject storedInPool = new TestObject(7);
            ObjectPool<TestObject>.ReleaseResource(poolId, storedInPool);

            TestObject retrieved = ObjectPool<TestObject>.RequestResource(poolId, () => { return new TestObject(42); });
            Assert.AreEqual(storedInPool, retrieved);

            // teardown
            ObjectPool<TestObject>.RemovePool(poolId);
        }

        /// <summary>
        /// Checks that the custom pool uses FIFO if it contains multiple objects and one is requested
        /// </summary>
        [Test]
        public void RequestResource_CustomPoolContainsMultipleObj_UsesFIFO()
        {
            int poolId = ObjectPool<TestObject>.CreateNewPool();
            TestObject obj1 = new TestObject(1);
            ObjectPool<TestObject>.ReleaseResource(poolId, obj1);
            TestObject obj2 = new TestObject(2);
            ObjectPool<TestObject>.ReleaseResource(poolId, obj2);

            TestObject retrieved = ObjectPool<TestObject>.RequestResource(poolId, () => { return new TestObject(42); });
            Assert.AreEqual(obj1, retrieved);

            TestObject retrieved2 = ObjectPool<TestObject>.RequestResource(poolId, () => { return new TestObject(42); });
            Assert.AreEqual(obj2, retrieved2);

            // teardown
            ObjectPool<TestObject>.RemovePool(poolId);
        }

        /// <summary>
        /// Checks that an exception is thrown if an object is requested from a pool that does not exist
        /// </summary>
        [Test]
        public void RequestResource_PoolDoesNotExist_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(delegate
           {
               ObjectPool<TestObject>.RequestResource(-1, () => { return new TestObject(42); });
           });
        }

        /// <summary>
        /// Checks that a released object is stored in the default pool
        /// </summary>
        [Test]
        public void ReleaseResource_DefaultPool_StoredInPool()
        {
            TestObject testObj = new TestObject(1);
            ObjectPool<TestObject>.ReleaseResource(testObj);

            TestObject retrieved = ObjectPool<TestObject>.RequestResource(() => { return new TestObject(42); });
            Assert.AreEqual(testObj, retrieved);
        }

        /// <summary>
        /// Checks that a release object is stored in a custom pool
        /// </summary>
        [Test]
        public void ReleaseResource_CustomPool_StoredInPool()
        {
            int poolId = ObjectPool<TestObject>.CreateNewPool();

            TestObject testObj = new TestObject(1);
            ObjectPool<TestObject>.ReleaseResource(poolId, testObj);

            TestObject retrieved = ObjectPool<TestObject>.RequestResource(poolId, () => { return new TestObject(42); });
            Assert.AreEqual(testObj, retrieved);

        }

        /// <summary>
        /// Checks that an exception is thrown if an object is released to a pool that does not exist
        /// </summary>
        [Test]
        public void ReleaseResource_PoolDoesNotExist_ThrowsInvalidOperationException()
        {
            TestObject testObj = new TestObject(1);
            Assert.Throws<InvalidOperationException>(delegate
            {
                ObjectPool<TestObject>.ReleaseResource(-1, testObj);
            });
        }

        /// <summary>
        /// Checks that the factory method is used after a non-empty default pool was cleared (meaning that it is now empty)
        /// </summary>
        [Test]
        public void ClearPool_ClearedDefaultPool_UsesFactoryAfterClean()
        {
            TestObject obj = new TestObject(1);
            ObjectPool<TestObject>.ReleaseResource(obj);

            ObjectPool<TestObject>.ClearPool();

            TestObject retrieved = ObjectPool<TestObject>.RequestResource(() => { return new TestObject(42); });
            Assert.AreNotEqual(obj, retrieved);
            Assert.AreEqual(42, retrieved.Id);
        }

        /// <summary>
        /// Checks that the factory method is used after a non-empty custom pool was cleared (meaning that it is now empty)
        /// </summary>
        [Test]
        public void ClearPool_ClearedCustomPool_UsesFactoryAfterClean()
        {
            int poolId = ObjectPool<TestObject>.CreateNewPool();
            TestObject obj = new TestObject(1);
            ObjectPool<TestObject>.ReleaseResource(poolId, obj);

            ObjectPool<TestObject>.ClearPool(poolId);

            TestObject retrieved = ObjectPool<TestObject>.RequestResource(1, () => { return new TestObject(42); });
            Assert.AreNotEqual(obj, retrieved);
            Assert.AreEqual(42, retrieved.Id);
        }

        /// <summary>
        /// Checks that an exception is thrown if a pool is cleared that does not exist
        /// </summary>
        [Test]
        public void ClearPool_PoolDoesNotExist_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(delegate
            {
                ObjectPool<TestObject>.ClearPool(-1);
            });
        }
    }
}
