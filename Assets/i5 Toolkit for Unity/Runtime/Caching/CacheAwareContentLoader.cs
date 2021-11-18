using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities;
using i5.Toolkit.Core.Utilities.ContentLoaders;
using System.Threading.Tasks;

namespace i5.Toolkit.Core.Caching
{
    public class CacheAwareContentLoader : IContentLoader<string>
    {
        public IContentLoader<string> InternalContentLoader { get; set; } = new UnityWebRequestLoader();

        public IFileCache Cache { get; set; }

        public CacheAwareContentLoader(IFileCache cache = null)
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
            WebResponse<string> response;
            //Check for cached files
            if (Cache != null || ServiceManager.ServiceExists<FileCacheService>())
            {
                if (Cache == null)
                {
                    // if no fixed cache is set then load one from the ServiceManager
                    Cache = ServiceManager.GetService<FileCacheService>();
                }
                string cachePath;
                if (Cache.IsFileInCache(uri))
                {
                    //use existing cache entry
                    cachePath = Cache.GetCachedFileLocation(uri);
                }
                else
                {
                    // create new entry in cache
                    cachePath = await Cache.AddOrUpdateInCacheAsync(uri);

                    //in case of an empty string the cache was not able to cache the file correctly - therefore the fallback is to not use the cache for this file
                    if (string.IsNullOrEmpty(cachePath))
                    {
                        cachePath = uri;
                    }
                }
                response = await InternalContentLoader.LoadAsync(cachePath);
            }
            else
            {
                response = await InternalContentLoader.LoadAsync(uri);
            }
            return response;
        }
    }
}