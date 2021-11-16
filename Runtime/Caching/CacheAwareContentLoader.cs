using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities;
using i5.Toolkit.Core.Utilities.ContentLoaders;
using System.Threading.Tasks;

namespace i5.Toolkit.Core.Caching
{
    public class CacheAwareContentLoader : IContentLoader<string>
    {
        public IContentLoader<string> InternContentLoader { get; set; }

        public FileCache Cache { get; set; }

        public CacheAwareContentLoader(FileCache cache)
        {
            this.Cache = cache;
        }

        /// <summary>
        /// Load the file that is specified by the uri. Uses the chache when possible.
        /// </summary>
        /// <param name="uri">Path to the file that should be loaded.</param>
        /// <returns>Returns the loaded file, either from cache or the given uri</returns>
        public async Task<WebResponse<string>> LoadAsync(string uri)
        {
            // initialize the content loader
            if (InternContentLoader == null)
            {
                InternContentLoader = new UnityWebRequestLoader();
            }

            WebResponse<string> response;
            //Check for cached files
            if (Cache != null || ServiceManager.ServiceExists<FileCache>())
            {
                FileCache objCache = Cache;
                if (objCache == null)
                {
                    //when no fixed cache is set then load one from the ServiceManager
                    objCache = ServiceManager.GetService<FileCache>();
                }
                string cachePath;
                if (objCache.IsFileInCache(uri))
                {
                    //use existing cache entry
                    cachePath = objCache.GetCachedFileLocation(uri);
                }
                else
                {
                    // create new entry in cache
                    cachePath = await objCache.AddOrUpdateInCache(uri);

                    //in case of an empty string the cache was not able to cache the file correctly - therefore the fallback is to not use the cache for this file
                    if (string.IsNullOrEmpty(cachePath))
                    {
                        cachePath = uri;
                    }
                }
                response = await InternContentLoader.LoadAsync(cachePath);
            }
            else
            {
                response = await InternContentLoader.LoadAsync(uri);
            }
            return response;
        }
    }
}