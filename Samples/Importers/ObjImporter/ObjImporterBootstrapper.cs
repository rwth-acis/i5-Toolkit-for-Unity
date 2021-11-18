using i5.Toolkit.Core.Caching;
using i5.Toolkit.Core.ModelImporters;
using i5.Toolkit.Core.ServiceCore;
using UnityEngine;

namespace i5.Toolkit.Core.Examples.ObjImporterExample
{
    /// <summary>
    /// Bootstrapper which initializes the obj importer service
    /// </summary>
    public class ObjImporterBootstrapper : BaseServiceBootstrapper
    {
        [SerializeField]
        private bool useCache = false;


        /// <summary>
        /// Initializes the obj importer in the service manager
        /// </summary>
        protected override void RegisterServices()
        {
            ObjImporter importer = new ObjImporter();
            ServiceManager.RegisterService(importer);

            // in case you want to cache downloaded models:
            // create a file cache service and register at the service manager
            // after that: set the obj importer's content loader to a cache aware content loader
            // it will make use of the cache whenever it is possible
            if (useCache)
            {
                // you can change the file cache's settings in the constructor of the FileCacheService
                FileCacheService fileCache = new FileCacheService();
                ServiceManager.RegisterService(fileCache);
                importer.ContentLoader = new CacheAwareContentLoader();
            }
        }

        protected override void UnRegisterServices()
        {
            ServiceManager.RemoveService<ObjImporter>();
            // also remove the file cache service since we do not need it anymore
            if (ServiceManager.ServiceExists<FileCacheService>())
            {
                ServiceManager.RemoveService<FileCacheService>();
            }
        }
    }
}