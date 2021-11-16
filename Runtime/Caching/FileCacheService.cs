using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities;
using i5.Toolkit.Core.Utilities.ContentLoaders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.Caching
{
    /// <summary>
    /// Service for caching files locally so that they do not need to be downloaded repeatedly
    /// </summary>
    public class FileCacheService : IService
    {
        /// <summary>
        /// Module that should be used for fetching the file's content
        /// </summary>
        public IContentLoader<string> ContentLoader { get; set; }

        private const string persistentCacheFileName = "i5cache.json";

        private bool sessionPersistence;
        private bool useSafeMode;
        private string cacheLocation;
        private double daysValid;

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
        public FileCacheService(bool sessionPersistence = false, bool useSafeMode = true, string cacheLocationOverride = null, double daysValid = 365)
        {
            // initialize the content loader if it is not set up
            if (ContentLoader == null)
            {
                ContentLoader = new UnityWebRequestLoader();
            }
            this.sessionPersistence = sessionPersistence;
            this.useSafeMode = useSafeMode;
            if (!string.IsNullOrEmpty(cacheLocationOverride))
            {
                // first check if the cache location exists
                if (Directory.Exists(cacheLocationOverride))
                {
                    this.cacheLocation = cacheLocationOverride;
                }
                else
                {
                    i5Debug.LogError("Could not override cache location as the folder does not exist", this);
                }
            }

            // if the cache location is still uninitialized, fill it with the default cache path
            if (string.IsNullOrEmpty(this.cacheLocation))
            {
                this.cacheLocation = Application.temporaryCachePath;
            }
            this.daysValid = daysValid;
        }

        /// <summary>
        /// Called when the service is registered at the service manager
        /// </summary>
        /// <param name="owner">The service manager which now owns the service</param>
        public void Initialize(IServiceManager owner)
        {
            if (sessionPersistence)
            {
                // try to load cachedFileLocation from file
                string pathToCache = Path.Combine(cacheLocation, persistentCacheFileName);
                if (File.Exists(pathToCache))
                {
                    i5Debug.Log($"Loading cache from {pathToCache}", this);
                    string serializedFileInfo = File.ReadAllText(pathToCache);
                    cacheContent = JsonDictionaryUtility.FromJson<string, CacheEntry>(serializedFileInfo);
                    // check loaded dictonary and remove entries for deleted files
                    var itemsToRemove = cacheContent.Where(fileInfo => !File.Exists(fileInfo.Value.localFileName)).ToArray();
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
            if (sessionPersistence)
            {
                string pathToCache = Path.Combine(cacheLocation, persistentCacheFileName);
                try
                {
                    File.WriteAllText(pathToCache, JsonDictionaryUtility.ToJson(cacheContent));
                }
                catch
                {
                    i5Debug.Log($"The current cache was not able to store its state to a persistent file under {pathToCache}.", this);
                }
            }
            else
            {
                // remove all cached objects
                foreach (KeyValuePair<string, CacheEntry> fileInfo in cacheContent)
                {
                    try
                    {
                        File.Delete(fileInfo.Value.localFileName);
                    }
                    catch (Exception e)
                    {
                        i5Debug.LogWarning($"Could not delete cache file for {fileInfo.Value.localFileName}\n{e.ToString()}", this);
                    }
                }
            }
        }

        public async Task<string> AddOrUpdateInCache(string path)
        {
            string savePath = Path.Combine(cacheLocation, Path.GetFileNameWithoutExtension(path) + Path.GetExtension(path));
            int i = 2;
            while (File.Exists(savePath))
            {
                savePath = Path.Combine(cacheLocation, Path.GetFileNameWithoutExtension(path) + i.ToString() + Path.GetExtension(path));
                i++;
            }

            WebResponse<string> fileRequestResponse = await ContentLoader.LoadAsync(path);
            if (fileRequestResponse.Successful)
            {
                File.WriteAllText(savePath, fileRequestResponse.Content);
                //save in dictonary
                if (useSafeMode)
                {
                    cacheContent[path] = new CacheEntry(savePath, CalculateMD5Hash(savePath), DateTime.Now);
                }
                else
                {
                    cacheContent[path] = new CacheEntry(savePath, "", DateTime.Now);
                }

                return savePath;
            }
            else
            {
                return string.Empty;
            }
        }

        public bool IsFileInCache(string path)
        {
            if (cacheContent.TryGetValue(path, out CacheEntry entry))
            {
                if (useSafeMode && File.Exists(entry.localFileName) && (CalculateMD5Hash(entry.localFileName) == entry.fileHash) && (entry.CacheDate >= DateTime.Now.AddDays(-1 * daysValid)))
                {
                    return true;
                }

                if (!useSafeMode && File.Exists(entry.localFileName) && (entry.CacheDate >= DateTime.Now.AddDays(-1 * daysValid)))
                {
                    return File.Exists(entry.localFileName);
                }
            }
            return false;
        }

        public string GetCachedFileLocation(string path)
        {
            CacheEntry entry;
            if (cacheContent.TryGetValue(path, out entry))
            {
                if (useSafeMode && File.Exists(entry.localFileName) && (CalculateMD5Hash(entry.localFileName) == entry.fileHash) && (entry.CacheDate >= DateTime.Now.AddDays(-1 * daysValid)))
                {
                    i5Debug.Log("Cache hit", this);
                    return entry.localFileName;
                }
                if (!useSafeMode && File.Exists(entry.localFileName) && (entry.CacheDate >= DateTime.Now.AddDays(-1 * daysValid)))
                {
                    i5Debug.Log("Cache hit", this);
                    return entry.localFileName;
                }
                return "";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Calculates a hash for the given file
        /// </summary>
        /// <param name="filePath">The path to the file</param>
        /// <returns>Returns the MD5 hash of the given file</returns>
        private string CalculateMD5Hash(string filePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }
}