using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ExternalNugetCSPostProcessor : AssetPostprocessor
{
#if EXTERNAL_NUGET

    public override int GetPostprocessOrder()
    {
        return 20;
    }

    public static void OnGeneratedCSProjectFiles()
    {

    }

#endif
}