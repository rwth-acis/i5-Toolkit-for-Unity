using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities.ContentLoaders;
using i5.Toolkit.Core.Utilities;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System;
using System.Linq;

namespace i5.Toolkit.Core.ModelImporters
{
    public class FileCache : IService
    {
        private const string persistentFileName = "i5cache.json";

        private bool sessionPersistence;
        private bool useSaveMode;
        private string cacheLocation;
        private double daysValid;

        private Dictionary<string, CacheEntry> cachedFileLocation = new Dictionary<string, CacheEntry>();

        public FileCache(bool sessionPersistence = false, bool useSaveMode = true, string cacheLocationOverride=null, double daysValid=365)
        {
            this.sessionPersistence = sessionPersistence;
            this.useSaveMode = useSaveMode;
            if(cacheLocationOverride != null && Directory.Exists(cacheLocationOverride))
            {
                this.cacheLocation = cacheLocationOverride;
            }
            else
            {
                this.cacheLocation = Application.temporaryCachePath;
            }
            this.daysValid = daysValid;
        }

        public void Initialize(IServiceManager owner)
        {
            if (sessionPersistence)
            {
                //try to load cachedFileLocation from file
                if(File.Exists(string.Format("{0}/{1}", cacheLocation, persistentFileName)))
                {
                    i5Debug.Log(string.Format("{0}/{1}", cacheLocation, persistentFileName), this);
                    string serializedFileInfo = File.ReadAllText(string.Format("{0}/{1}", cacheLocation, persistentFileName));
                    cachedFileLocation = JsonConvert.DeserializeObject<Dictionary<string, CacheEntry>>(serializedFileInfo);
                    //check loaded dictonary
                    var itemsToRemove = cachedFileLocation.Where(fileInfo => !File.Exists(fileInfo.Value.localFileName)).ToArray();
                    foreach (var item in itemsToRemove)
                        cachedFileLocation.Remove(item.Key);
                    
                    i5Debug.Log("The cache state from a previous session was restored successfully.", this);
                }
                else
                {
                    i5Debug.Log("No previous cache session detected. A new one is created.", this);
                }
            }
            else
            {
                i5Debug.Log("The cache failed to restore the state of a previous session.", this);
            }
        }

        public void Cleanup()
        {
            if (sessionPersistence)
            {
                try
                {
                    File.WriteAllText(string.Format("{0}/{1}", cacheLocation, persistentFileName), JsonConvert.SerializeObject(cachedFileLocation));
                }
                catch
                {
                    i5Debug.Log("The current cach was not able to store its state to a persistent file.", this);
                }
            }
            else
            {
                //remove all cached objects
                foreach (KeyValuePair<string, CacheEntry> fileInfo in cachedFileLocation)
                {
                    try
                    {
                        File.Delete(fileInfo.Value.localFileName);
                    }
                    catch
                    {

                    }
                }
            }
        }

        public async Task<string> addOrUpdateInCache(string path)
        {
            string savePath = string.Format("{0}/{1}{2}", cacheLocation, Path.GetFileNameWithoutExtension(path), Path.GetExtension(path));
            int i = 2;
            while (File.Exists(savePath))
            {
                savePath = string.Format("{0}/{1}{2}", cacheLocation, Path.GetFileNameWithoutExtension(path) + i.ToString(), Path.GetExtension(path));
                i++;
            }

            UnityWebRequestLoader ContentLoader = new UnityWebRequestLoader();
            WebResponse<string> fileRequestResponse = await ContentLoader.LoadAsync(path);
            if (fileRequestResponse.Successful)
            {
                File.WriteAllText(savePath, fileRequestResponse.Content);
                //save in dictonary
                if (useSaveMode)
                {
                    cachedFileLocation[path] = new CacheEntry(savePath, calculateMD5Hash(savePath), DateTime.Now);
                }
                else
                {
                    cachedFileLocation[path] = new CacheEntry(savePath, "", DateTime.Now);
                }
                
                return savePath;
            }
            else
            {
                return "";
            }
        }

        public bool isFileInCache(string path)
        {
            CacheEntry entry;
            if (cachedFileLocation.TryGetValue(path, out entry))
            {
                if (useSaveMode && File.Exists(entry.localFileName) && (calculateMD5Hash(entry.localFileName) == entry.fileHash) && (entry.cacheDate >= DateTime.Now.AddDays(-1*daysValid)))
                {
                    return true;
                }
                
                if(!useSaveMode && File.Exists(entry.localFileName) && (entry.cacheDate >= DateTime.Now.AddDays(-1*daysValid)))
                {
                    return File.Exists(entry.localFileName);
                }
            }
            return false;
        }

        public string getCachedFileLocation(string path)
        {
            CacheEntry entry;
            if (cachedFileLocation.TryGetValue(path, out entry))
            {
                if (useSaveMode && File.Exists(entry.localFileName) && (calculateMD5Hash(entry.localFileName) == entry.fileHash) && (entry.cacheDate >= DateTime.Now.AddDays(-1*daysValid)))
                {
                    i5Debug.Log("Cache hit", this);
                    return entry.localFileName;
                }
                if (!useSaveMode && File.Exists(entry.localFileName) && (entry.cacheDate >= DateTime.Now.AddDays(-1*daysValid)))
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
        
        private string calculateMD5Hash(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        public int filesInCache()
        {
            return cachedFileLocation.Count;
        }
    }
}