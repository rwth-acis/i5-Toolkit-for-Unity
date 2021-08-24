using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CacheEntry
{
    public string localFileName { get; set; }
    public string fileHash { get; set; }
    public DateTime cacheDate { get; set; }

    public CacheEntry()
    {

    }

    public CacheEntry(string localFileName, string fileHash, DateTime cacheDate)
    {
        this.localFileName = localFileName;
        this.fileHash = fileHash;
        this.cacheDate = cacheDate;
    }
}
