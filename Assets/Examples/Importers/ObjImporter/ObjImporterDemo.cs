using i5.Toolkit.Core.ModelImporters;
using i5.Toolkit.Core.ProceduralGeometry;
using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities;
using i5.Toolkit.Core.Utilities.ContentLoaders;
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
        public bool urlIsLocalFile = false;
        public string url;

        private async void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                ServiceManager.GetService<ObjImporter>().ExtendedLogging = extendedLogging;
                if (!urlIsLocalFile) 
                {
                    GameObject obj = await ServiceManager.GetService<ObjImporter>().ImportAsync(url);
                }
                else
                {
                    //Replace the default UnityWRequestLoader content loader with the FileSystemLoader content loader that can handle local files
                    ServiceManager.GetService<ObjImporter>().ContentLoader = new FileSystemLoader();
                    GameObject obj = await ServiceManager.GetService<ObjImporter>().ImportFromFileAsync(url);
                }
            }
        }
    }
}