using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i5.Toolkit.Core.Caching
{
    public interface IFileHasher
    {
        /// <summary>
        /// Calculates a hash for the given file
        /// </summary>
        /// <param name="filePath">The path to the file</param>
        /// <returns>Returns the MD5 hash of the given file</returns>
        string CalculateMD5Hash(string path);
    }
}
