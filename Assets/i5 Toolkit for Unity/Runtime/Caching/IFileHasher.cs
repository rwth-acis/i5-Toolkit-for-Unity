namespace i5.Toolkit.Core.Caching
{
    /// <summary>
    /// Contract for hashing files
    /// </summary>
    public interface IFileHasher
    {
        /// <summary>
        /// Calculates a hash for the given file using MD5
        /// </summary>
        /// <param name="filePath">The path to the file</param>
        /// <returns>Returns the MD5 hash of the given file</returns>
        string CalculateMD5Hash(string path);
    }
}
