using i5.Toolkit.ModelImporters;
using i5.Toolkit.ServiceCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjImporterBootstrapper : MonoBehaviour, IServiceManagerBootstrapper
{
    public void InitializeServiceManager()
    {
        ObjImporter2 importer = new ObjImporter2();
        ServiceManager.RegisterService(importer);
    }
}
