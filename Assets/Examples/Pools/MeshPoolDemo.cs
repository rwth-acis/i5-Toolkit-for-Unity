using i5.Toolkit.Core.Utilities;
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
        if (Input.GetKeyDown(KeyCode.F5))
        {
            if (stored)
            {
                i5Debug.Log("Get from pool", this);
                mesh = ObjectPool<Mesh>.RequestResource(() => { Mesh mesh = new Mesh(); mesh.name = "Created"; return mesh; });
                mesh.name = "Mesh " + Random.Range(1, 100);
                stored = false;
            }
            else
            {
                i5Debug.Log("Return to pool", this);
                ObjectPool<Mesh>.ReturnResource(mesh);
                stored = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.F6))
        {
            i5Debug.Log(mesh.name, this);
        }
    }
}
