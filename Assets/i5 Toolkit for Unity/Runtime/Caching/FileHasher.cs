using i5.Toolkit.Core.Experimental.SystemAdapters;
using System;
using System.Security.Cryptography;

namespace i5.Toolkit.Core.Caching
{
    /// <summary>
    /// Generates hash checksums for files
    /// </summary>
    public class FileHasher : IFileHasher
    {
        public IFileAccessor FileAccessor { get; set; } = new FileAccessorAdapter();

        /// <summary>
        /// Calculates a hash for the given file using MD5
        /// </summary>
        /// <param name="filePath">The path to the file</param>
        /// <returns>Returns the MD5 hash of the given file</returns>
        public string CalculateMD5Hash(string filePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = FileAccessor.OpenRead(filePath))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }
}
