using System;
using System.IO;
using UnityEngine;

namespace i5.Toolkit.Core.VersionTool
{
    [Serializable]
    public class VersionCache
    {
        public string appVersion;
        public Version wsaVersion;
        public int androidVersion;

        [SerializeField] private string strWsaVersion;

        private const string savePath = "Temp/VersionCache.tmp";

        public void Save()
        {
            strWsaVersion = wsaVersion.ToString();
            string json = JsonUtility.ToJson(this);
            File.WriteAllText(savePath, json);
        }

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

        public static void Remove()
        {
            File.Delete(savePath);
        }
    }
}