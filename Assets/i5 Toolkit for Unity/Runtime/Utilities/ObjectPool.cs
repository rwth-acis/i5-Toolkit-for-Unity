using System;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.Utilities
{
    /// <summary>
    /// Object pool which can store instances so that they can be reused
    /// This is useful for objects like GameObjects or Meshes where the creation and destruction drops the performance
    /// This should definitely be used for meshes since they are not automatically cleaned up by the garbage collector
    /// </summary>
    /// <typeparam name="T">The object type which should be stored in the pool</typeparam>
    public static class ObjectPool<T>
    {
        private static Dictionary<int, Queue<T>> pools = new Dictionary<int, Queue<T>>();

        private static int id = 1;

        public static int CountPools()
        {
            return pools.Count;
        }

        /// <summary>
        /// Opens a new pool and returns the id of the pool
        /// </summary>
        /// <returns>The id of the created pool</returns>
        public static int CreateNewPool()
        {
            return CreateNewPool(1);
        }

        /// <summary>
        /// Opens a new pool and returns the id of the pool
        /// </summary>
        /// <param name="capacity">Define a (soft) capacity of the pool for which memory is allocated in advance</param>
        /// <returns>The id of the created pool</returns>
        public static int CreateNewPool(int capacity)
        {
            int res = id;
            pools.Add(res, new Queue<T>(capacity));
            id++;
            return res;
        }

        /// <summary>
        /// First cleans and then removes the pool with the given id
        /// </summary>
        /// <param name="poolId"></param>
        /// <param name="destroyAction"></param>
        public static void RemovePool(int poolId, Action<T> destroyAction = null)
        {
            if (poolId == 0)
            {
                Debug.LogError("Cannot remove default pool");
                return;
            }

            if (!pools.ContainsKey(poolId))
            {
                Debug.LogWarning("The pool with the given id could not be destroyed because it does not exist anymore.");
                return;
            }

            ClearPool(poolId, destroyAction);
            pools.Remove(poolId);
        }

        /// <summary>
        /// Requests a resource from the default pool
        /// If no resource is left, the code in the creationFactory function will be executed to create a new object
        /// The creationFactory should probably use Unity's Instantiate method
        /// </summary>
        /// <param name="creationFactory">Function which should create a new instance of the pooled object</param>
        /// <returns>An instance of the object from the pool</returns>
        public static T RequestResource(Func<T> creationFactory)
        {
            return RequestResource(0, creationFactory);
        }

        /// <summary>
        /// Requests a resource from the pool
        /// If no resource is left, the code in the creationFactory function will be executed to create a new object
        /// The creationFactory should probably use Unity's Instantiate method
        /// </summary>
        /// <param name="creationFactory">Function which should create a new instance of the pooled object</param>
        /// <returns>An instance of the object from the pool</returns>
        public static T RequestResource(int poolId, Func<T> creationFactory)
        {
            EnsurePoolIdValid(poolId);

            if (pools[poolId].Count > 0)
            {
                return pools[poolId].Dequeue();
            }
            else
            {
                return creationFactory();
            }
        }

        /// <summary>
        /// Returns the resource to the default pool so that it can be requested again
        /// This should return all control over this object back to the pool
        /// </summary>
        /// <param name="resource">The resource which is returned to the pool</param>
        public static void ReleaseResource(T resource)
        {
            ReleaseResource(0, resource);
        }

        /// <summary>
        /// Returns the resource to the pool so that it can be requested again
        /// This should return all control over this object back to the pool
        /// </summary>
        /// <param name="poolId">The id of the pool</param>
        /// <param name="resource">The resource which is returned to the pool</param>
        public static void ReleaseResource(int poolId, T resource)
        {
            EnsurePoolIdValid(poolId);
            pools[poolId].Enqueue(resource);
        }

        /// <summary>
        /// Clears the default pool by removing every instance in the queue
        /// Performs the given destroyAction on each instance to destroy it
        /// You probably want to use Destroy() inside the destroyAction
        /// </summary>
        /// <param name="destroyAction"></param>
        public static void ClearPool(Action<T> destroyAction = null)
        {
            ClearPool(0, destroyAction);
        }

        /// <summary>
        /// Clears the pool by removing every instance in the queue
        /// Performs the given destroyAction on each instance to destroy it
        /// You probably want to use Destroy() inside the destroyAction
        /// </summary>
        /// <param name="poolId">The id of the pool which should be cleared</param>
        /// <param name="destroyAction">The action which should be performed to destroy on object</param>
        public static void ClearPool(int poolId, Action<T> destroyAction = null)
        {
            EnsurePoolIdValid(poolId);

            while (pools[poolId].Count > 0)
            {
                T obj = pools[poolId].Dequeue();
                destroyAction?.Invoke(obj);
            }
        }

        /// <summary>
        /// Checks if the pool id is valid
        /// If not, it throws a invalid operation exception
        /// </summary>
        /// <param name="poolId">The id of the pool which should be accessed</param>
        private static void EnsurePoolIdValid(int poolId)
        {
            if (!pools.ContainsKey(poolId))
            {
                // the default pool should always exist
                // if it is accessed for the first time, create it
                if (poolId == 0)
                {
                    pools.Add(0, new Queue<T>());
                }
                // all other pools must be created explicitly; if they do not exist: throw exception
                else
                {
                    throw new InvalidOperationException("You are trying to access an object pool which does not exist. Make sure to call CreateNewPool before accessing it.");
                }
            }
        }
    }
}