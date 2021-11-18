using System;
using UnityEngine;

namespace i5.Toolkit.Core.Caching
{
    /// <summary>
    /// One data entry in the cache
    /// </summary>
    [Serializable]
    public class CacheEntry
    {
        /// <summary>
        /// Name of the file
        /// </summary>
        public string localFileName;

        /// <summary>
        /// Hash checksum of the file
        /// </summary>
        public string fileHash;

        // internal representation of the date as a string for json serialization
        [SerializeField] private string cacheDate;

        /// <summary>
        /// Date of the entry
        /// </summary>
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

        /// <summary>
        /// Creates a new cache entry
        /// </summary>
        /// <param name="localFileName">The name of the file</param>
        /// <param name="fileHash">The hash of the file</param>
        /// <param name="cacheDate">The date of the cache entry</param>
        public CacheEntry(string localFileName, string fileHash, DateTime cacheDate)
        {
            this.localFileName = localFileName;
            this.fileHash = fileHash;
            this.CacheDate = cacheDate;
        }
    }
}