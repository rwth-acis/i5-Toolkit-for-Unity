using i5.Toolkit.Core.ModelImporters;
using i5.Toolkit.Core.ProceduralGeometry;
using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.Examples.ObjImporterExample
{
    /// <summary>
    /// Demo client which allows the tester to import a obj file if F5 is pressed
    /// </summary>
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
}