using i5.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class TextureConstructor
{
    public string LoadPath { get; set; }

    public TextureConstructor(string loadPath)
    {
        LoadPath = loadPath;
    }

    public Texture2D ConstructTexture()
    {
        return new Texture2D(2, 2);
    }
}
