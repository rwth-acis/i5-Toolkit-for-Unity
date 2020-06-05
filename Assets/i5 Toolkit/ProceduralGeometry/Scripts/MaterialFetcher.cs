using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialFetcher
{
    public string MaterialLibrary { get; set; }

    public string MaterialName { get; set; }

    public MaterialFetcher() : this("", "")
    {
    }

    public MaterialFetcher(string materialLibrary, string materialName)
    {
        MaterialLibrary = materialLibrary;
        MaterialName = materialName;
    }
}
