using i5.Toolkit.ProceduralGeometry;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjParseResult
{
    public ObjectConstructor ObjectConstructor { get; private set; }

    public string LibraryPath { get; set; }

    public string MaterialName { get; set; }

    public ObjParseResult()
    {
        ObjectConstructor = new ObjectConstructor();
    }
}
