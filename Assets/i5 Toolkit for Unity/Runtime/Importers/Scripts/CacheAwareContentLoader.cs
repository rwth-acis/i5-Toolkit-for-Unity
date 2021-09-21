using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using i5.Toolkit.Core.Utilities.ContentLoaders;
using System.Threading.Tasks;
using i5.Toolkit.Core.Utilities;
using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.ModelImporters;

namespace i5.Toolkit.Core.ModelImporters
{
    public class CachAwareContentLoader : IContentLoader<string>
    {
        private IContentLoader<string> internContentLoader { get; set; }

        public async Task<WebResponse<string>> LoadAsync(string uri)
        {
            // initialize the content loader
            if (internContentLoader == null)
            {
                internContentLoader = new UnityWebRequestLoader();
            }
            WebResponse<string> response;
            //Check for cached files
            if (ServiceManager.ServiceExists<FileCache>())
            {
                FileCache objCache = ServiceManager.GetService<FileCache>();
                string cachePath;
                if (objCache.isFileInCache(uri))
                {
                    //use existing cache entry
                    cachePath = objCache.getCachedFileLocation(uri);
                }
                else
                {
                    // create new entry in cache
                    cachePath = await objCache.addOrUpdateInCache(uri);

                    //incase of an empty string the cache was not able to cache the file correctly - therefore the fallback is to not use the cache for this file
                    if (cachePath == "")
                    {
                        cachePath = uri;
                    }
                }
                response = await internContentLoader.LoadAsync(cachePath);
            }
            else
            {
                response = await internContentLoader.LoadAsync(uri);
            }
            return response;
        }
    }
}