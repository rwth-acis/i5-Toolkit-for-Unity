using System;
using System.IO;
using UnityEngine;

namespace i5.Toolkit.Core.VersionTool
{
    /// <summary>
    /// Cache for the version settings so that they can be restored after the build
    /// </summary>
    [Serializable]
    public class VersionCache
    {
        /// <summary>
        /// Version string which is used on many platforms, e.g. Standalone
        /// </summary>
        public string appVersion;
        /// <summary>
        /// Version for WSA apps
        /// </summary>
        public Version wsaVersion;
        /// <summary>
        /// Android version number
        /// </summary>
        public int androidVersion;

        // Unity cannot serialize the Version object,
        // so it is first converted to a string that is serialized
        [SerializeField] private string strWsaVersion;

        // path where the cache is stored
        private const string savePath = "Temp/VersionCache.tmp";

        /// <summary>
        /// Saves the cache as a temporary file in the project's Temp folder
        /// </summary>
        public void Save()
        {
            strWsaVersion = wsaVersion.ToString();
            string json = JsonUtility.ToJson(this);
            File.WriteAllText(savePath, json);
        }

        /// <summary>
        /// Loads the cache if it exists
        /// If no cache file exists, default values are loaded
        /// </summary>
        /// <returns>Returns the loaded version cache object</returns>
        public static VersionCache Load()
        {
            if (!File.Exists(savePath))
            {
                return new VersionCache();
            }
            string json = File.ReadAllText(savePath);
            VersionCache versionCache = JsonUtility.FromJson<VersionCache>(json);
            versionCache.wsaVersion = Version.Parse(versionCache.strWsaVersion);
            return versionCache;
        }

        /// <summary>
        /// Clears the cache by deleting the temporary file
        /// </summary>
        public static void Remove()
        {
            File.Delete(savePath);
        }
    }
}