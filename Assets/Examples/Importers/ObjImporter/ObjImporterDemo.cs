using i5.Toolkit.Core.ModelImporters;
using i5.Toolkit.Core.ProceduralGeometry;
using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjImporterDemo : MonoBehaviour
{
    public bool extendedLogging = true;
    public string url;

    private async void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            ServiceManager.GetService<ObjImporter>().ExtendedLogging = extendedLogging;
            GameObject obj = await ServiceManager.GetService<ObjImporter>().ImportAsync(url);
        }
    }
}
