using i5.Toolkit.ModelImporters;
using i5.Toolkit.ProceduralGeometry;
using i5.Toolkit.ServiceCore;
using i5.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ObjImporterDemo : MonoBehaviour
{
    [Multiline]
    public string objData;

    private MeshFilter meshFilter;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            if (meshFilter.sharedMesh != null)
            {
                ObjectPool<Mesh>.ReturnResource(meshFilter.sharedMesh);
            }

            string[] lines = objData.Split('\n');
            ImportOperation op = new ImportOperation(lines, (res) =>
            {
                meshFilter.sharedMesh = res.result.ConstructMesh();
            });
            ServiceManager.GetService<ObjImporter>().AddOperation(op);
        }
    }
}
