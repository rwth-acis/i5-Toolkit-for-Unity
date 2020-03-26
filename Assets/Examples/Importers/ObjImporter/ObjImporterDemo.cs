using i5.Toolkit.ModelImporters;
using i5.Toolkit.ProceduralGeometry;
using i5.Toolkit.ServiceCore;
using i5.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjImporterDemo : MonoBehaviour
{
    public string url;

    private async void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            GameObject obj = await ServiceManager.GetService<ObjImporter>().ImportAsync(url);
        }
    }
}
