using i5.Toolkit.ProceduralGeometry;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjParseResult : MonoBehaviour
{
    public List<GeometryConstructor> ConstructedGeometries { get; private set; }

    public List<string> MtlLibs { get; }

    public ObjParseResult()
    {
        ConstructedGeometries = new List<GeometryConstructor>();
        MtlLibs = new List<string>();
    }
}
