using System.IO;

namespace i5.Toolkit.Core.Experimental.SystemAdapters
{
    public class FileAccessorAdapter : IFileAccessor
    {
        public void Delete(string path)
        {
            File.Delete(path);
        }

        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        public FileStream OpenRead(string path)
        {
            return File.OpenRead(path);
        }

        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }

        public void WriteAllText(string path, string contents)
        {
            File.WriteAllText(path, contents);
        }
    }
}
