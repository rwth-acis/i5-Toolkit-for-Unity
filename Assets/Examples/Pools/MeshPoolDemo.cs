using i5.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshPoolDemo : MonoBehaviour
{
    bool stored = false;
    Mesh mesh;

    private void Start()
    {
        mesh = new Mesh();
        mesh.name = "start";
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (stored)
            {
                Debug.Log("Get from pool");
                mesh = ObjectPool<Mesh>.RequestResource(() => { Mesh mesh = new Mesh(); mesh.name = "Created"; return mesh; });
                mesh.name = "Mesh " + Random.Range(1, 100);
                stored = false;
            }
            else
            {
                Debug.Log("Return to pool");
                ObjectPool<Mesh>.ReturnResource(mesh);
                stored = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log(mesh.name);
        }
    }
}
