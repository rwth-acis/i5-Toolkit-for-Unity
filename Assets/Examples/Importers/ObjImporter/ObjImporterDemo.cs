using i5.Toolkit.ModelImporters;
using i5.Toolkit.ProceduralGeometry;
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            string[] lines = objData.Split('\n');
            GeometryConstructor gc = ObjImporter.ParseObjText(lines);
            meshFilter.sharedMesh = gc.ConstructMesh();
        }
        else if (Input.GetKeyDown(KeyCode.Delete))
        {
            Destroy(gameObject);
        }
    }
}
