using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities.ContentLoaders;
using i5.Toolkit.Core.Utilities;
using System.Threading.Tasks;

namespace i5.Toolkit.Core.ModelImporters
{
    public class ObjCache : IService
    {
        private string cacheLocation;

        private Dictionary<string, string> cachedFileLocation = new Dictionary<string, string>();

        public void Initialize(IServiceManager owner)
        {
            cacheLocation = Application.temporaryCachePath;
        }

        public void Cleanup()
        {
            //remove all cached objects
            foreach(KeyValuePair<string, string> fileInfo in cachedFileLocation)
            {
                try
                {
                    File.Delete(fileInfo.Value);
                }
                catch
                {

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
                cachedFileLocation[path] = savePath;
                return savePath;
            }
            else
            {
                return "";
            }
        }
        
        public bool isFileInCache(string path)
        {
            string location = "";
            if (cachedFileLocation.TryGetValue(path, out location))
            {
                return File.Exists(location);
            }
            else
            {
                return false;
            }
        }

        public string getCachedFileLocation(string path)
        {
            string location = "";
            if (cachedFileLocation.TryGetValue(path, out location))
            {
                if (File.Exists(location))
                {
                    return location;
                }
                return "";
            }
            else
            {
                return "";
            }
        }
    }
}