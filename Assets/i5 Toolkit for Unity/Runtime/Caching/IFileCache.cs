using System.Threading.Tasks;

namespace i5.Toolkit.Core.Caching
{
    /// <summary>
    /// A file cache for storing local copies for remote files
    /// </summary>
    public interface IFileCache
    {
        /// <summary>
        /// If true, cached files should be stored between app sessions
        /// </summary>
        bool SessionPersistence { get; }
        /// <summary>
        /// If true, cached files are protected against switching them
        /// </summary>
        bool UseSafeMode { get; }
        /// <summary>
        /// States where the cache is stored on the file system
        /// </summary>
        string CacheLocation { get; }
        /// <summary>
        /// States how long entries should stay valid before they need to be re-downloaded
        /// </summary>
        float DaysValid { get; }
        /// <summary>
        /// The number of files which are currently stored in the cache
        /// </summary>
        int FileCount { get; }

        /// <summary>
        /// Adds a new entry to the cache or forces an update to an existing entry
        /// </summary>
        /// <param name="path">The path to the remote file, probably an URL</param>
        /// <returns>Returns the file path of the cached file on the local file system</returns>
        Task<string> AddOrUpdateInCacheAsync(string path);

        /// <summary>
        /// Checks whether the file at the given URL path was already cached
        /// </summary>
        /// <param name="path">The path to the remote file, probably an URL</param>
        /// <returns>Returns true if the file is cached, otherwise false</returns>
        bool IsFileInCache(string path);

        /// <summary>
        /// Looks up the path to the cached file, given a path to the remote 
        /// </summary>
        /// <param name="path">The path to the remote file, probably an URL</param>
        /// <returns>Retruns the path to the local cached file or an empty string if the file is not cached</returns>
        string GetCachedFileLocation(string path);
    }
}
