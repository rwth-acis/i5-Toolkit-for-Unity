using i5.Toolkit.Core.Spawners;
using UnityEngine;

namespace i5.Toolkit.Core.Examples.Spawners
{

    public class SpawnerDemo : MonoBehaviour
    {
        public Spawner spawner;

        private int spawnCount = 0;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                spawner.Spawn();
                spawner.MostRecentlySpawnedObject.transform.position = new Vector3(0, spawnCount, 0);
                spawnCount++;
            }
        }
    }
}