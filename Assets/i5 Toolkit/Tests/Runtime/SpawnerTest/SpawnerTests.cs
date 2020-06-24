using i5.Toolkit.Core.Spawners;
using i5.Toolkit.Core.TestUtilities;
using i5.Toolkit.Core.Utilities;
using NUnit.Framework;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Core.Tests.Spawners
{
    public class SpawnerTests
    {
        private const string prefabName = "SpawnedObject";

        [SetUp]
        public void LoadTestScene()
        {
            PlayModeTestUtilities.LoadEmptyTestScene();
        }

        [TearDown]
        public void UnloadTestScene()
        {
            PlayModeTestUtilities.UnloadTestScene();
        }

        /// <summary>
        /// Creates the spawner in the scene based on a prefab name
        /// The prefab must be stored in the location specified under textBasepath
        /// </summary>
        /// <param name="spawnerName">The name of the spawner prefab</param>
        /// <returns>The spawner component which has been created in the scene</returns>
        private Spawner PreTestSetup(string spawnerName)
        {
            string testBasePath = PathUtils.GetPackagePath() + "Tests/Runtime/SpawnerTest";
            GameObject spawnerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(testBasePath + "/" + spawnerName + ".prefab");
            Spawner spawner = GameObject.Instantiate(spawnerPrefab).GetComponent<Spawner>();
            return spawner;
        }

        /// <summary>
        /// Cleans up the spawned object
        /// </summary>
        /// <param name="spawner"></param>
        private void PostTestCleanUp(Spawner spawner)
        {
            for (int i=0;i<spawner.SpawnedInstances.Length;i++)
            {
                GameObject.Destroy(spawner.SpawnedInstances[i]);
            }
            GameObject.Destroy(spawner.gameObject);
        }

        /// <summary>
        /// Checks if a single spawn of an object works
        /// </summary>
        [Test]
        public void Spawn_SingleCall_ReturnsTrue()
        {
            // create the spawner
            Spawner spawner = PreTestSetup("OverwriteSpawner1");

            Assert.AreEqual(0, spawner.SpawnedInstances.Length);
            // spawn an object
            bool res = spawner.Spawn();
            // spawning must be successful
            Assert.IsTrue(res);
        }

        /// <summary>
        /// Checks if a single spawn of an object works and creates the object
        /// </summary>
        [Test]
        public void Spawn_SingleCall_ObjectCreated()
        {
            // create the spawner
            Spawner spawner = PreTestSetup("OverwriteSpawner1");

            Assert.AreEqual(0, spawner.SpawnedInstances.Length);
            // spawn an object
            bool res = spawner.Spawn();

            // the created object must have been added to the spawned instances and it must be based on the prefab
            Assert.AreEqual(1, spawner.SpawnedInstances.Length);
            Assert.IsTrue(spawner.SpawnedInstances[0].name.Contains(prefabName));

            PostTestCleanUp(spawner);
        }

        /// <summary>
        /// Checks if multiple successive spawns work
        /// This test only spawns two objects and the spawner has a limit of 3, i.e. no spawn limitations or overwrites are met in this test
        /// </summary>
        [Test]
        public void Spawn_MultipleCallsWithoutOverwrite_ReturnTrue()
        {
            // create the spawner
            Spawner spawner = PreTestSetup("OverwriteSpawner3");
            Assert.AreEqual(0, spawner.SpawnedInstances.Length);

            bool res = spawner.Spawn();
            // spawning must be successful
            Assert.IsTrue(res);

            // the created object must have been added to the spawned instances and it must be based on the prefab
            Assert.IsTrue(spawner.SpawnedInstances[0].name.Contains(prefabName));

            res = spawner.Spawn();
            // spawning must be successful
            Assert.IsTrue(res);

            PostTestCleanUp(spawner);
        }

        /// <summary>
        /// Checks if multiple successive spawns work
        /// This test only spawns two objects and the spawner has a limit of 3, i.e. no spawn limitations or overwrites are met in this test
        /// </summary>
        [Test]
        public void Spawn_MultipleCallsWithoutOverwrite_ObjectsCreated()
        {
            // create the spawner
            Spawner spawner = PreTestSetup("OverwriteSpawner3");
            Assert.AreEqual(0, spawner.SpawnedInstances.Length);

            bool res = spawner.Spawn();

            // the created object must have been added to the spawned instances and it must be based on the prefab
            Assert.AreEqual(1, spawner.SpawnedInstances.Length);
            Assert.IsTrue(spawner.SpawnedInstances[0].name.Contains(prefabName));

            res = spawner.Spawn();

            // the created object must have been added to the spawned instances and it must be based on the prefab
            Assert.AreEqual(2, spawner.SpawnedInstances.Length);
            Assert.IsTrue(spawner.SpawnedInstances[1].name.Contains(prefabName));

            PostTestCleanUp(spawner);
        }

        /// <summary>
        /// Checks if the spawned object can be overwritten if this option is enabled
        /// </summary>
        [Test]
        public void Spawn_Overwrite_ReturnsTrue()
        {
            // create a spawner that allows overwriting
            Spawner spawner = PreTestSetup("OverwriteSpawner1");

            Assert.AreEqual(0, spawner.SpawnedInstances.Length);
            // spawn an object, spawning should have been successful
            bool res = spawner.Spawn();
            Assert.IsTrue(res);

            // spawn another object, this should be successful and should overwrite the first object
            res = spawner.Spawn();
            Assert.IsTrue(res);

            PostTestCleanUp(spawner);
        }

        /// <summary>
        /// Checks if the spawned object can be overwritten if this option is enabled
        /// </summary>
        [UnityTest]
        public IEnumerator Spawn_Overwrite_ObjectReplaced()
        {
            // create a spawner that allows overwriting
            Spawner spawner = PreTestSetup("OverwriteSpawner1");

            Assert.AreEqual(0, spawner.SpawnedInstances.Length);
            // spawn an object, spawning should have been successful
            bool res = spawner.Spawn();

            Assert.AreEqual(1, spawner.SpawnedInstances.Length);
            Assert.IsTrue(spawner.SpawnedInstances[0].name.Contains(prefabName));

            // modify the name of the spawned object so that we can later identify if it was overwritten
            GameObject firstSpawned = spawner.SpawnedInstances[0];
            firstSpawned.name = "First Spawned";

            // spawn another object, this should be successful and should overwrite the first object
            res = spawner.Spawn();

            // make sure that there is still only one object
            Assert.AreEqual(1, spawner.SpawnedInstances.Length);
            Assert.IsTrue(spawner.SpawnedInstances[0].name.Contains(prefabName));
            // make sure that the first spawned object was destroyed
            // we must wait for one frame so that the object is cleaned up
            yield return null;
            Assert.IsTrue(firstSpawned == null);

            PostTestCleanUp(spawner);
        }

        /// <summary>
        /// Checks that overwrite spawns with a limit higher than 1 work
        /// </summary>
        /// <returns></returns>
        [Test]
        public void Spawn_OverwriteWithLimitBigger1_ReturnsTrue()
        {
            // create a spawner that allows overwriting with a limit of 3
            Spawner spawner = PreTestSetup("OverwriteSpawner3");

            Assert.AreEqual(0, spawner.SpawnedInstances.Length);

            // spawn three objects to fill the object buffer
            for (int i=0;i<3;i++)
            {
                // spawn an object, spawning should have been successful
                bool res = spawner.Spawn();
                Assert.IsTrue(res);
            }

            // spawn another object, this should be successful and should overwrite the first object
            bool overwriteRes = spawner.Spawn();
            Assert.IsTrue(overwriteRes);

            PostTestCleanUp(spawner);
        }

        /// <summary>
        /// Checks that overwrite spawns with a limit higher than 1 work
        /// </summary>
        /// <returns></returns>
        [Test]
        public void Spawn_OverwriteWithLimitBigger1_ObjectReplaced()
        {
            // create a spawner that allows overwriting with a limit of 3
            Spawner spawner = PreTestSetup("OverwriteSpawner3");

            Assert.AreEqual(0, spawner.SpawnedInstances.Length);

            GameObject firstSpawned = null;

            // spawn three objects to fill the object buffer
            for (int i = 0; i < 3; i++)
            {
                // spawn an object, spawning should have been successful
                bool res = spawner.Spawn();

                Assert.AreEqual(i + 1, spawner.SpawnedInstances.Length);
                Assert.IsTrue(spawner.SpawnedInstances[i].name.Contains(prefabName));

                // modify the name of the spawned object so that we can later identify if it was overwritten
                GameObject spawned = spawner.SpawnedInstances[i];
                spawned.name = "Spawned[" + i + "]";
                if (i == 0)
                {
                    firstSpawned = spawned;
                    Assert.IsTrue(firstSpawned != null);
                }
            }

            // spawn another object, this should be successful and should overwrite the first object
            bool overwriteRes = spawner.Spawn();

            // make sure that there are still only three objects
            Assert.AreEqual(3, spawner.SpawnedInstances.Length);
            Assert.IsTrue(spawner.MostRecentlySpawnedObject.name.Contains(prefabName));

            PostTestCleanUp(spawner);
        }

        /// <summary>
        /// Checks if a spawned object which is then destroyed can be overwritten without errors
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator Spawn_Limit1DestroyObject_NewSpawnReturnsTrue()
        {
            // create a spawner which allows overwriting and is limited to one object
            Spawner spawner = PreTestSetup("OverwriteSpawner1");

            Assert.AreEqual(0, spawner.SpawnedInstances.Length);
            // spawn an object, this should be successful
            bool res = spawner.Spawn();
            Assert.IsTrue(res);

            // destroy the spawned object
            GameObject.Destroy(spawner.SpawnedInstances[0]);
            // we need to wait for a frame so that the object gets cleaned up
            yield return null;
            // make sure that the destroyed object was removed from the list of spawned instances
            Assert.AreEqual(0, spawner.SpawnedInstances.Length);

            // spawn another object, this shhould be successful and should result in an error
            res = spawner.Spawn();
            Assert.IsTrue(res);

            PostTestCleanUp(spawner);
        }

        /// <summary>
        /// Checks if a spawned object which is then destroyed can be overwritten without errors
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator Spawn_Limit1DestroyObject_NewSpawnObjectCreated()
        {
            // create a spawner which allows overwriting and is limited to one object
            Spawner spawner = PreTestSetup("OverwriteSpawner1");

            Assert.AreEqual(0, spawner.SpawnedInstances.Length);
            // spawn an object, this should be successful
            bool res = spawner.Spawn();

            Assert.AreEqual(1, spawner.SpawnedInstances.Length);
            Assert.IsTrue(spawner.SpawnedInstances[0].name.Contains(prefabName));

            // destroy the spawned object
            GameObject.Destroy(spawner.SpawnedInstances[0]);
            // we need to wait for a frame so that the object gets cleaned up
            yield return null;
            // make sure that the destroyed object was removed from the list of spawned instances
            Assert.AreEqual(0, spawner.SpawnedInstances.Length);

            // spawn another object, this shhould be successful and should result in an error
            res = spawner.Spawn();

            Assert.AreEqual(1, spawner.SpawnedInstances.Length);
            Assert.IsTrue(spawner.SpawnedInstances[0] != null);
            Assert.IsTrue(spawner.SpawnedInstances[0].name.Contains(prefabName));

            PostTestCleanUp(spawner);
        }

        /// <summary>
        /// Checks if we cannot overwrite a spawned object if the spawner is set to "no overwrite" and limited to 1 instance
        /// </summary>
        [Test]
        public void Spawn_NoOverwriteLimit1Reached_ReturnsFalse()
        {
            // create a spawner which does not allow overwriting and is limited to 1 instance
            Spawner spawner = PreTestSetup("NoOverwriteSpawner1");

            Assert.AreEqual(0, spawner.SpawnedInstances.Length);
            // spawn an object
            bool res = spawner.Spawn();
            Assert.IsTrue(res);

            // keep track of the first spawned object
            GameObject firstSpawned = spawner.SpawnedInstances[0];
            firstSpawned.name = "First Spawned";

            // try to spawn another object, this should be aborted and we should still have the first object
            res = spawner.Spawn();
            Assert.IsFalse(res);

            PostTestCleanUp(spawner);
        }

        /// <summary>
        /// Checks if we cannot overwrite a spawned object if the spawner is set to "no overwrite" and limited to 1 instance
        /// </summary>
        [Test]
        public void Spawn_NoOverwriteLimit1Reached_ObjectNotReplaced()
        {
            // create a spawner which does not allow overwriting and is limited to 1 instance
            Spawner spawner = PreTestSetup("NoOverwriteSpawner1");

            Assert.AreEqual(0, spawner.SpawnedInstances.Length);
            // spawn an object
            bool res = spawner.Spawn();

            Assert.AreEqual(1, spawner.SpawnedInstances.Length);
            Assert.IsTrue(spawner.SpawnedInstances[0].name.Contains(prefabName));

            // keep track of the first spawned object
            GameObject firstSpawned = spawner.SpawnedInstances[0];
            firstSpawned.name = "First Spawned";

            // try to spawn another object, this should be aborted and we should still have the first object
            res = spawner.Spawn();

            Assert.AreEqual(1, spawner.SpawnedInstances.Length);
            Assert.AreEqual(firstSpawned, spawner.SpawnedInstances[0]);
            Assert.IsFalse(spawner.SpawnedInstances[0].name.Contains(prefabName));

            PostTestCleanUp(spawner);
        }

        /// <summary>
        /// Checks that a spawn operation returns false if the limit is reached and overwriting is disallowed
        /// </summary>
        [Test]
        public void Spawn_NoOverwriteLimit3Reached_ReturnsFalse()
        {
            // create a spawner that does not allow overwriting with a limit of 3
            Spawner spawner = PreTestSetup("NoOverwriteSpawner3");

            Assert.AreEqual(0, spawner.SpawnedInstances.Length);

            // spawn three objects to fill the object buffer
            for (int i = 0; i < 3; i++)
            {
                // spawn an object, spawning should have been successful
                bool res = spawner.Spawn();
                Assert.IsTrue(res);
            }

            // spawn another object, this should fail
            bool overwriteRes = spawner.Spawn();
            Assert.IsFalse(overwriteRes);

            PostTestCleanUp(spawner);
        }

        /// <summary>
        /// Checks that existing objects are not replaced on a "no-overwrite" spawner if the limit is reached
        /// </summary>
        [Test]
        public void Spawn_NoOverwriteLimit3Reached_ObjectNotReplaced()
        {
            // create a spawner that allows overwriting with a limit of 3
            Spawner spawner = PreTestSetup("NoOverwriteSpawner3");

            Assert.AreEqual(0, spawner.SpawnedInstances.Length);

            GameObject firstSpawned = null;

            // spawn three objects to fill the object buffer
            for (int i = 0; i < 3; i++)
            {
                // spawn an object, spawning should have been successful
                bool res = spawner.Spawn();

                Assert.AreEqual(i + 1, spawner.SpawnedInstances.Length);
                Assert.IsTrue(spawner.SpawnedInstances[i].name.Contains(prefabName));

                // modify the name of the spawned object so that we can later identify if it was overwritten
                GameObject spawned = spawner.SpawnedInstances[i];
                spawned.name = "Spawned[" + i + "]";
                if (i == 0)
                {
                    firstSpawned = spawned;
                    Assert.IsTrue(firstSpawned != null);
                }
            }

            // spawn another object, this should fail
            bool overwriteRes = spawner.Spawn();
            Assert.IsFalse(overwriteRes);

            // make sure that there are still only three objects
            Assert.AreEqual(3, spawner.SpawnedInstances.Length);
            Assert.IsTrue(spawner.MostRecentlySpawnedObject.name == "Spawned[2]");

            PostTestCleanUp(spawner);
        }

        /// <summary>
        /// Checks that a spawn operation returns true after the limit was reached but an instance was destroyed
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator Spawn_LimitReachedExternalDestroy_NewSpawnReturnsTrue()
        {
            // create a spawner which does not allow overwriting and is limited to 1 instance
            Spawner spawner = PreTestSetup("NoOverwriteSpawner1");

            Assert.AreEqual(0, spawner.SpawnedInstances.Length);
            // spawn an object
            bool res = spawner.Spawn();
            Assert.IsTrue(res);

            // keep track of the first spawned object
            GameObject firstSpawned = spawner.SpawnedInstances[0];
            GameObject.Destroy(firstSpawned);
            yield return null;

            // spawn another object, this should be successful since the first object is gone
            res = spawner.Spawn();
            Assert.IsTrue(res);

            PostTestCleanUp(spawner);
        }

        /// <summary>
        /// Checks that destroyed spawned instances are removed from the list of spawned instances
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator SpawnedInstances_ExternalDestroy_DestroyedObjectRemoved()
        {
            // create a spawner which does not allow overwriting and is limited to 1 instance
            Spawner spawner = PreTestSetup("NoOverwriteSpawner1");

            Assert.AreEqual(0, spawner.SpawnedInstances.Length);
            // spawn an object
            bool res = spawner.Spawn();

            // keep track of the first spawned object
            GameObject firstSpawned = spawner.SpawnedInstances[0];
            GameObject.Destroy(firstSpawned);
            yield return null;
            // make sure that the destroyed object was removed from the list of spawned instances
            Assert.AreEqual(0, spawner.SpawnedInstances.Length);

            PostTestCleanUp(spawner);
        }

        /// <summary>
        /// Checks that a new object can be created if the limit was reached but an instance was destroyed
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator Spawn_LimitReachedExternalDestroy_NewSpawnCreatesObject()
        {
            // create a spawner which does not allow overwriting and is limited to 1 instance
            Spawner spawner = PreTestSetup("NoOverwriteSpawner1");

            Assert.AreEqual(0, spawner.SpawnedInstances.Length);
            // spawn an object
            bool res = spawner.Spawn();

            // keep track of the first spawned object
            GameObject firstSpawned = spawner.SpawnedInstances[0];
            firstSpawned.name = "First Spawned";
            GameObject.Destroy(firstSpawned);
            yield return null;

            // spawn another object, this should be successful since the first object is gone
            res = spawner.Spawn();

            // check that we only have one object and that it is the new object
            Assert.AreEqual(1, spawner.SpawnedInstances.Length);
            Assert.IsTrue(spawner.SpawnedInstances[0].name.Contains(prefabName));

            PostTestCleanUp(spawner);
        }

        /// <summary>
        /// Checks if a spawner where DestroyWithSpawner is true also destroys all instances if it is destroyed
        /// If the spawner is destroyed, all spawned instances should also be destroyed
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator Destroy_DestroyWithSpawner_SpawnedInstancesDestroyed()
        {
            // create a spawner where the option "Destroy with Spawner" is enabled
            Spawner spawner = PreTestSetup("OverwriteSpawner1");

            Assert.AreEqual(0, spawner.SpawnedInstances.Length);
            // spawn an object
            bool res = spawner.Spawn();

            GameObject firstSpawned = spawner.SpawnedInstances[0];
            Assert.IsTrue(firstSpawned != null);

            // destroy the spanwer and check if the object was also destroyed
            GameObject.Destroy(spawner.gameObject);
            yield return null;
            Assert.IsTrue(firstSpawned == null);
        }
        
        /// <summary>
        /// Checks that the spawned instances can survive a spawner where DestroyWithSpawner is set to false
        /// If the spawner is destroyed, the instances should still exist
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator Destroy_NotDestroyWithSpawner_SpawnedInstancesStillExist()
        {
            // create a spawner where the option "Destroy with Spawner" is disabled
            Spawner spawner = PreTestSetup("NoDestroySpawner");

            Assert.AreEqual(0, spawner.SpawnedInstances.Length);
            // spawn an object
            bool res = spawner.Spawn();

            GameObject firstSpawned = spawner.SpawnedInstances[0];
            Assert.IsTrue(firstSpawned != null);

            // destroy the spawner and check that the object has not been destroyed
            GameObject.Destroy(spawner.gameObject);
            yield return null;
            Assert.IsTrue(firstSpawned != null);
        }
    }
}
