using i5.Toolkit.Core.Spawners;
using UnityEngine;

namespace i5.Toolkit.Core.Examples.Spawners
{
	public class SpawnerMoveDemo : MonoBehaviour
    {
        [SerializeField] private Spawner spawner;

        // spawns a new object if the user presses F5. Then, it repositions the spawner so that the next object does not appear at the same location.
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                spawner.Spawn();

                // update spawner's position
                spawner.transform.position = new Vector3(
                    Random.Range(-2f, 2f),
                    spawner.transform.position.y,
                    spawner.transform.position.z);
            }
        }
    }
}