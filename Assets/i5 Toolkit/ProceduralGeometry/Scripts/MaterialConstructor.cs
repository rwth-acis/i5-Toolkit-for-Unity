using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialConstructor
{
    public Material ConstructMaterial()
    {
        return new Material(Shader.Find("Standard"));
    }
}
