using System;
using UnityEngine;

namespace i5.Toolkit.Core.Caching
{
    [Serializable]
    public class CacheEntry
    {
        public string localFileName;
        public string fileHash;

        [SerializeField] private string cacheDate;


        public DateTime CacheDate
        {
            get
            {
                return DateTime.Parse(cacheDate);
            }
            set
            {
                cacheDate = value.ToString();
            }
        }

        public CacheEntry(string localFileName, string fileHash, DateTime cacheDate)
        {
            this.localFileName = localFileName;
            this.fileHash = fileHash;
            this.CacheDate = cacheDate;
        }
    }
}