using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialConstructor
{
    public string ShaderName { get; set; }

    public Color Color { get; set; } = Color.white;

    private Dictionary<string, float> floatValues;

    public MaterialConstructor() : this("Standard")
    {
    }

    public MaterialConstructor(string shaderName)
    {
        ShaderName = shaderName;
        floatValues = new Dictionary<string, float>();
    }

    public void SetFloat(string name, float value)
    {
        if (floatValues.ContainsKey(name))
        {
            floatValues[name] = value;
        }
        else
        {
            floatValues.Add(name, value);
        }
    }

    public Material ConstructMaterial()
    {
        Material mat = new Material(Shader.Find(ShaderName));
        mat.color = Color;
        foreach(KeyValuePair<string, float> setCommand in floatValues)
        {
            mat.SetFloat(setCommand.Key, setCommand.Value);
        }
        return mat;
    }
}
