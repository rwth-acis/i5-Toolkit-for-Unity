using i5.Toolkit.Core.ModelImporters;
using i5.Toolkit.Core.ServiceCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjImporterBootstrapper : MonoBehaviour, IServiceManagerBootstrapper
{
    public void InitializeServiceManager()
    {
        ObjImporter importer = new ObjImporter();
        ServiceManager.RegisterService(importer);
    }
}
