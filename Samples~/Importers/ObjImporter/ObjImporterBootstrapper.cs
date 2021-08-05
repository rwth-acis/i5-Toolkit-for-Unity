using i5.Toolkit.Core.ModelImporters;
using i5.Toolkit.Core.ServiceCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.Examples.ObjImporterExample
{
    /// <summary>
    /// Bootstrapper which initializes the obj importer service
    /// </summary>
    public class ObjImporterBootstrapper : BaseServiceBootstrapper
    {
        /// <summary>
        /// Initializes the obj importer in the service manager
        /// </summary>
        protected override void RegisterServices()
        {
            ObjImporter importer = new ObjImporter();
            ServiceManager.RegisterService(importer);
        }

        protected override void UnRegisterServices()
        {
            ServiceManager.RemoveService<ObjImporter>();
        }
    }
}