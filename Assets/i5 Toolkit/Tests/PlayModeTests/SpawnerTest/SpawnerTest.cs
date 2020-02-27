using System.Collections;
using System.Collections.Generic;
using i5.Toolkit.Spawners;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class SpawnerTest
    {
        private Spawner spawnerOverwriteLimited;

        private const string testBasePath = "Assets/i5 Toolkit/Tests/PlayModeTests/SpawnerTest";
        private const string prefabName = "SpawnedObject";

        [SetUp]
        public void Setup()
        {
            GameObject prefabSpanwerOverwriteLimited = AssetDatabase.LoadAssetAtPath<GameObject>(testBasePath + "/Spawner.prefab");
            spawnerOverwriteLimited = GameObject.Instantiate(prefabSpanwerOverwriteLimited).GetComponent<Spawner>();
        }

        [Test]
        public void TestSingleSpawn()
        {
            Assert.AreEqual(0, spawnerOverwriteLimited.SpawnedInstances.Length);
            bool res = spawnerOverwriteLimited.Spawn();
            Assert.IsTrue(res);

            Assert.AreEqual(1, spawnerOverwriteLimited.SpawnedInstances.Length);
            Assert.IsTrue(spawnerOverwriteLimited.SpawnedInstances[0].name.Contains(prefabName));
        }

        [TearDown]
        public void Teardown()
        {
            GameObject.Destroy(spawnerOverwriteLimited.gameObject);
        }
    }
}
