using System.Threading.Tasks;

namespace i5.Toolkit.Core.Caching
{
    public interface IFileCache
    {
        bool SessionPersistence { get; }
        bool UseSafeMode { get; }
        string CacheLocation { get; }
        float DaysValid { get; }
        int FileCount { get; }

        Task<string> AddOrUpdateInCacheAsync(string path);
        bool IsFileInCache(string path);
        string GetCachedFileLocation(string path);
    }
}
