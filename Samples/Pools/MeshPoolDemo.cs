using i5.Toolkit.Core.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.Examples.MeshPoolExample
{
    /// <summary>
    /// Example that demonstrates that the Mesh pool fixes the memory leak caused by meshes
    /// The tester can press F5 to create a mesh or return it to the mesh pool so that it is reused
    /// If F6 is pressed, the name of the used mesh is logged
    /// Open the performance profile and look at the memory statistics to see if a memory leak exists
    /// </summary>
    public class MeshPoolDemo : MonoBehaviour
    {
        private bool stored = false;
        private Mesh mesh;

        /// <summary>
        /// Iniitalizes the mesh
        /// </summary>
        private void Start()
        {
            mesh = new Mesh();
            mesh.name = "start";
        }
        

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                // if the mesh is stored in the pool, get it
                if (stored)
                {
                    i5Debug.Log("Get from pool", this);
                    mesh = ObjectPool<Mesh>.RequestResource(() => { Mesh mesh = new Mesh(); mesh.name = "Created"; return mesh; });
                    mesh.name = "Mesh " + Random.Range(1, 100);
                    stored = false;
                }
                // if the mesh is not stored, return it to the pool
                else
                {
                    i5Debug.Log("Return to pool", this);
                    ObjectPool<Mesh>.ReleaseResource(mesh);
                    stored = true;
                }
            }
            else if (Input.GetKeyDown(KeyCode.F6))
            {
                i5Debug.Log(mesh.name, this);
            }
        }
    }
}