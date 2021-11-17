using i5.Toolkit.Core.Experimental.SystemAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace i5.Toolkit.Core.Caching
{
    public class FileHasher : IFileHasher
    {
        public IFileAccessor FileAccessor { get; set; } = new FileAccessorAdapter();

        /// <summary>
        /// Calculates a hash for the given file
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
