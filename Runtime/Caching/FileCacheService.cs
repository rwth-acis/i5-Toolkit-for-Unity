using i5.Toolkit.Core.Experimental.SystemAdapters;
using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities;
using i5.Toolkit.Core.Utilities.ContentLoaders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.Caching
{
    /// <summary>
    /// Service for caching files locally so that they do not need to be downloaded repeatedly
    /// </summary>
    public class FileCacheService : IService, IFileCache
    {
        /// <summary>
        /// Module that should be used for fetching the file's content
        /// </summary>
        public IContentLoader<string> ContentLoader { get; set; } = new UnityWebRequestLoader();

        /// <summary>
        /// Module that should be used for accessing files
        /// </summary>
        public IFileAccessor FileAccessor { get; set; } = new FileAccessorAdapter();

        /// <summary>
        /// Module should should be used for accessing directories
        /// </summary>
        public IDirectoryAccessor DirectoryAccessor { get; set; } = new DirectoryAccessorAdapter();

        /// <summary>
        /// Module for generating hash ids for files
        /// </summary>
        public IFileHasher FileHasher { get; set; } = new FileHasher();

        /// <summary>
        /// The name under which the cache registry is stored
        /// </summary>
        public const string persistentCacheFileName = "i5cache.json";

        /// <summary>
        /// If true, cache entries are stored persistently and read again on application startup
        /// </summary>
        public bool SessionPersistence { get; private set; }

        /// <summary>
        /// If true, cached files are hashed so that they cannot be switched out
        /// </summary>
        public bool UseSafeMode { get; private set; }

        /// <summary>
        /// Points to the folder where the cached files are stored and where the cache registry can be found
        /// </summary>
        public string CacheLocation { get; private set; }

        /// <summary>
        /// The amount of days for which a cached file is valid
        /// </summary>
        public float DaysValid { get; private set; }

        private Dictionary<string, CacheEntry> cacheContent = new Dictionary<string, CacheEntry>();

        /// <summary>
        /// Number of files in which are tracked in the cache
        /// </summary>
        public int FileCount
        {
            get
            {
                return cacheContent.Count;
            }
        }

        /// <summary>
        /// Creates a new file cache service
        /// </summary>
        /// <param name="sessionPersistence">If true, the tracked cache is stored and recovered in future application startups</param>
        /// <param name="useSafeMode">If true, files are hashed, meaning that cached content cannot be switched out by external influences</param>
        /// <param name="cacheLocationOverride">If set, the cache will be stored in the given place instead of the default path</param>
        /// <param name="daysValid">Number of days that an entry in the cache should stay valid before requiring a re-download</param>
        public FileCacheService(bool sessionPersistence = false, bool useSafeMode = true, string cacheLocationOverride = null, float daysValid = 365, IDirectoryAccessor directoryAccessor = null)
        {
            if (directoryAccessor != null)
            {
                DirectoryAccessor = directoryAccessor;
            }

            this.SessionPersistence = sessionPersistence;
            this.UseSafeMode = useSafeMode;
            if (!string.IsNullOrEmpty(cacheLocationOverride))
            {
                // first check if the cache location exists
                if (DirectoryAccessor.Exists(cacheLocationOverride))
                {
                    this.CacheLocation = cacheLocationOverride;
                }
                else
                {
                    i5Debug.LogError("Could not override cache location as the folder does not exist", this);
                }
            }

            // if the cache location is still uninitialized, fill it with the default cache path
            if (string.IsNullOrEmpty(this.CacheLocation))
            {
                this.CacheLocation = Application.temporaryCachePath;
            }
            this.DaysValid = Mathf.Max(1, daysValid);
        }

        /// <summary>
        /// Called when the service is registered at the service manager
        /// </summary>
        /// <param name="owner">The service manager which now owns the service</param>
        public void Initialize(IServiceManager owner)
        {
            if (SessionPersistence)
            {
                // try to load cachedFileLocation from file
                string pathToCache = Path.Combine(CacheLocation, persistentCacheFileName);
                if (FileAccessor.Exists(pathToCache))
                {
                    i5Debug.Log($"Loading cache from {pathToCache}", this);
                    string serializedFileInfo = FileAccessor.ReadAllText(pathToCache);
                    cacheContent = JsonDictionaryUtility.FromJson<string, CacheEntry>(serializedFileInfo);
                    // check loaded dictonary and remove entries for deleted files
                    var itemsToRemove = cacheContent.Where(fileInfo => !FileAccessor.Exists(fileInfo.Value.localFileName)).ToArray();
                    foreach (var item in itemsToRemove)
                    {
                        cacheContent.Remove(item.Key);
                    }

                    i5Debug.Log("The cache state from a previous session was restored successfully.", this);
                }
                else
                {
                    i5Debug.Log("No previous cache session detected. A new one is created.", this);
                }
            }
        }

        /// <summary>
        /// Called when the service is shut down
        /// </summary>
        public void Cleanup()
        {
            if (SessionPersistence)
            {
                string pathToCache = Path.Combine(CacheLocation, persistentCacheFileName);
                try
                {
                    FileAccessor.WriteAllText(pathToCache, JsonDictionaryUtility.ToJson(cacheContent));
                }
                catch
                {
                    i5Debug.LogError($"The current cache was not able to store its state to a persistent file under {pathToCache}.", this);
                }
            }
            else
            {
                // remove all cached objects
                foreach (KeyValuePair<string, CacheEntry> fileInfo in cacheContent)
                {
                    try
                    {
                        FileAccessor.Delete(fileInfo.Value.localFileName);
                    }
                    catch (Exception e)
                    {
                        i5Debug.LogWarning($"Could not delete cache file for {fileInfo.Value.localFileName}\n{e.ToString()}", this);
                    }
                }
            }
        }

        /// <summary>
        /// Caches a file and registers it in the cache.
        /// If the file is already cached, this method forces the download of the file again and updates the cache entry
        /// </summary>
        /// <param name="path">The path to the remote file resource, most likely an URL</param>
        /// <returns>Returns the path in local storage where the file was stored</returns>
        public async Task<string> AddOrUpdateInCacheAsync(string path)
        {
            string savePath = Path.Combine(CacheLocation, Path.GetFileNameWithoutExtension(path) + Path.GetExtension(path));
            // if the file already exists: create a new copy with an increased number; the old file stays as a backup
            int i = 2;
            while (FileAccessor.Exists(savePath))
            {
                savePath = Path.Combine(CacheLocation, Path.GetFileNameWithoutExtension(path) + i.ToString() + Path.GetExtension(path));
                i++;
            }

            WebResponse<string> fileRequestResponse = await ContentLoader.LoadAsync(path);
            if (fileRequestResponse.Successful)
            {
                FileAccessor.WriteAllText(savePath, fileRequestResponse.Content);
                //save in dictonary
                if (UseSafeMode)
                {
                    cacheContent[path] = new CacheEntry(savePath, FileHasher.CalculateMD5Hash(savePath), DateTime.Now);
                }
                else
                {
                    cacheContent[path] = new CacheEntry(savePath, "", DateTime.Now);
                }

                return savePath;
            }
            else
            {
                Debug.LogError("Could not retrieve file\n" + fileRequestResponse.ErrorMessage);
                return string.Empty;
            }
        }

        /// <summary>
        /// Checks whether the remote file at the given URL path is already cached
        /// </summary>
        /// <param name="path">The path to the remote file, probably an URL</param>
        /// <returns>Returns whether the file is already cached</returns>
        public bool IsFileInCache(string path)
        {
            return !string.IsNullOrEmpty(GetCachedFileLocation(path));
        }

        /// <summary>
        /// Returns the local file path where the file of the given URL path is cached
        /// If the file is not cached, it will return an empty string
        /// </summary>
        /// <param name="path">The path to the remote file, probably an URL</param>
        /// <returns>Returns the path in local storage to the cached version; if no version is cached, an empty string is returned</returns>
        public string GetCachedFileLocation(string path)
        {
            if (cacheContent.TryGetValue(path, out CacheEntry entry))
            {
                bool fileExists = FileAccessor.Exists(entry.localFileName);
                bool entryStillValid = entry.CacheDate >= DateTime.Now.AddDays(-1 * DaysValid);
                bool hashMatches = !UseSafeMode || (FileHasher.CalculateMD5Hash(entry.localFileName) == entry.fileHash);

                if (fileExists && entryStillValid && hashMatches)
                {
                    i5Debug.Log("Cache hit", this);
                    return entry.localFileName;
                }
            }

            return "";
        }
    }
}