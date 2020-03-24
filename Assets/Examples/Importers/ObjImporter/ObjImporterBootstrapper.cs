using i5.Toolkit.ModelImporters;
using i5.Toolkit.ServiceCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjImporterBootstrapper : MonoBehaviour
{
    private void Start()
    {
        ObjImporter objImporter = new ObjImporter();
        ServiceManager.RegisterService(objImporter);
    }
}
