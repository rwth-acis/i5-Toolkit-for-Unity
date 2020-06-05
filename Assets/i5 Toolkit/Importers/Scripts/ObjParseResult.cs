using i5.Toolkit.ProceduralGeometry;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjParseResult
{
    public List<ObjectConstructor> ConstructedObjects { get; private set; }

    public List<string> MtlLibs { get; }

    public ObjParseResult()
    {
        ConstructedObjects = new List<ObjectConstructor>();
        MtlLibs = new List<string>();
    }
}
