using i5.Toolkit.Core.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.Spawners
{
    /// <summary>
    /// Spawner which can instantiate one prefab object in the scene
    /// </summary>
    public class Spawner : MonoBehaviour
    {
        [Tooltip("The prefab which should be created in the scene")]
        [SerializeField] protected GameObject prefab;
        [Tooltip("If true, the object will be spawned immediately")]
        [SerializeField] protected bool spawnOnAwake = true;
        [Tooltip("If true, all spawned instances will be destroyed if the spawner is destroyed")]
        [SerializeField] protected bool destroyWithSpawner = true;
        [Tooltip("If true and the maxNumberOfInstances limit was reached, existing instances will be overwritten according to the fifo principle")]
        [SerializeField] protected bool overwriteExistingInstances = true;
        [Tooltip("Limits the amount of instances which can be created; if 0, the amount is unlimited")]
        [SerializeField] protected int maxNumberOfInstances = 0;

        /// <summary>
        /// The instance which was created by this spawner
        /// </summary>
        public GameObject[] SpawnedInstances
        {
            get
            {
                GarbageCollect();
                return spawnedInstanceQueue.ToArray();
            }
        }

        /// <summary>
        /// Returns the most recently spawned object
        /// </summary>
        public GameObject MostRecentlySpawnedObject { get => SpawnedInstances[spawnedInstanceQueue.Count - 1]; }

        /// <summary>
        /// If true, the spawned instance will be destroyed if the spawner is destroyed
        /// </summary>
        public bool DestroyWithSpawner { get => destroyWithSpawner; set => destroyWithSpawner = value; }

        /// <summary>
        /// If true, multiple calls of Spawn() will destroy existing instances in the scene
        /// Otherwise Spawn() will only work if no instance exists in the scene
        /// </summary>
        public bool OverwriteExistingInstances { get => overwriteExistingInstances; set => overwriteExistingInstances = value; }

        protected Queue<GameObject> spawnedInstanceQueue;

        /// <summary>
        /// Initiates a spawn if spawnOnAwake is true
        /// </summary>
        protected virtual void Awake()
        {
            spawnedInstanceQueue = new Queue<GameObject>();
            if (spawnOnAwake)
            {
                Spawn();
            }
        }

        /// <summary>
        /// This method can be overwritten to setup the spawned instance immediately after it has been created
        /// </summary>
        protected virtual void Setup(GameObject instance)
        {
            instance.transform.position = transform.position;
            instance.transform.rotation = transform.rotation;
        }

        /// <summary>
        /// Spawns the object instance
        /// Note that the spawn may fail, e.g. if the instance already existed and may not be overwritten
        /// </summary>
        /// <returns>True if the spawn process was successful</returns>
        public bool Spawn()
        {
            // check if the prefab was set
            if (prefab == null)
            {
                SpecialDebugMessages.LogMissingReferenceError(this, nameof(prefab));
                return false;
            }

            // garbage collect to delete destroyed instances
            GarbageCollect();

            // if the spawn limit has been set and is reached
            if (maxNumberOfInstances > 0 && spawnedInstanceQueue.Count >= maxNumberOfInstances)
            {
                // if we cannot overwrite: cannot spawn
                if (!overwriteExistingInstances)
                {
                    return false;
                }

                // remove instances until we have a free slot
                while (spawnedInstanceQueue.Count >= maxNumberOfInstances && spawnedInstanceQueue.Count > 0)
                {
                    GameObject removedInstance = spawnedInstanceQueue.Dequeue();
                    if (removedInstance != null) // could be null if another script already destroyed it
                    {
                        Destroy(removedInstance);
                    }
                }
            }
            // once we reach this point we can assume that we have a free slot which we can fill with a new instance

            GameObject instance = Instantiate(prefab);
            spawnedInstanceQueue.Enqueue(instance);

            Setup(instance);
            return true;
        }

        /// <summary>
        /// Called if the spawner is destroyed.
        /// If destroyWithSpawner was checked, the spawned instance will also be destroyed
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (destroyWithSpawner)
            {
                while (spawnedInstanceQueue.Count > 0)
                {
                    GameObject removedInstance = spawnedInstanceQueue.Dequeue();
                    if (removedInstance != null) // could be null if another script already destroyed it
                    {
                        Destroy(removedInstance);
                    }
                }
            }
        }

        private void GarbageCollect()
        {
            Queue<GameObject> tempQueue = new Queue<GameObject>();
            while (spawnedInstanceQueue.Count > 0)
            {
                GameObject go = spawnedInstanceQueue.Dequeue();
                if (go != null)
                {
                    tempQueue.Enqueue(go);
                }
            }
            spawnedInstanceQueue = tempQueue;
        }
    }
}