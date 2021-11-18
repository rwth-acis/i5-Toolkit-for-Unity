using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities;
using i5.Toolkit.Core.Utilities.ContentLoaders;
using System.Threading.Tasks;

namespace i5.Toolkit.Core.Caching
{
    /// <summary>
    /// A content loader that integrates the cache functionality
    /// Any file which is loaded via this content loader is stored in the cache and taken from there in future requests
    /// </summary>
    public class CacheAwareContentLoader : IContentLoader<string>
    {
        /// <summary>
        /// The content loader which should be used for the actual access to the content
        /// By default, it is initialized with the UnityWebRequestLoader
        /// </summary>
        public IContentLoader<string> InternalContentLoader { get; set; } = new UnityWebRequestLoader();

        /// <summary>
        /// The cache where the downloaded content is stored
        /// </summary>
        public IFileCache Cache { get; set; }

        /// <summary>
        /// Creates a new cache aware content loader
        /// </summary>
        /// <param name="cache">A reference to the cache that should be used. If it is not set, it will be pulled via the service manager</param>
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
            //Check if the cache is set
            if (Cache != null || ServiceManager.ServiceExists<FileCacheService>())
            {
                if (Cache == null)
                {
                    // if no fixed cache is set then load one from the ServiceManager
                    Cache = ServiceManager.GetService<FileCacheService>();
                }
                // determine the cache path
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
                // no cache found: just use the internal content loader and ignore all cache functionality with a warning
                i5Debug.LogWarning("Using the cache aware content loader but no cache is set up. " +
                    "Hence, it will not use the cache functionality and just work like a normal content loader. " +
                    "To fix this, either provide a cache to the service or register the FileCacheService at the service manager.", this);
                response = await InternalContentLoader.LoadAsync(uri);
            }
            return response;
        }
    }
}