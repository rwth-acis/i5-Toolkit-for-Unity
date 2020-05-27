using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MtlParseResult
{
    public Material material;
    public Dictionary<string, string> textureMaps;

    public MtlParseResult()
    {
        textureMaps = new Dictionary<string, string>();
    }

    public MtlParseResult(Material material) : this()
    {
        this.material = material;
    }
}
