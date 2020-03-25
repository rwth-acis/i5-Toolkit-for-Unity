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

            ObjImportOperation op = new ObjImportOperation("https://raw.githubusercontent.com/rwth-acis/Unity-Toolkit/features/objImporter/Assets/Examples/Importers/ObjImporter/Obj%20Models/Monkey_quads.obj",
                (res) =>
            {
                i5Debug.Log(res.result, this);
            });
            ServiceManager.GetService<ObjImporter>().Import(op);

            //string[] lines = objData.Split('\n');
            //ParseOperation op = new ParseOperation(lines, (res) =>
            //{
            //    meshFilter.sharedMesh = res.result.ConstructMesh();
            //});
            //ServiceManager.GetService<ObjParser>().AddOperation(op);
        }
    }
}
