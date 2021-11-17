using System.IO;

namespace i5.Toolkit.Core.Experimental.SystemAdapters
{
    public class DirectoryAccessorAdapter : IDirectoryAccessor
    {
        public bool Exists(string path)
        {
            return Directory.Exists(path);
        }
    }
}
