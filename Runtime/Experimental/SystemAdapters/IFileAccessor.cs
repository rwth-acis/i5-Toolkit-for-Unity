using System.IO;

namespace i5.Toolkit.Core.Experimental.SystemAdapters
{
    public interface IFileAccessor
    {
        bool Exists(string path);

        string ReadAllText(string path);

        void WriteAllText(string path, string contents);

        void Delete(string path);

        FileStream OpenRead(string path);
    }
}
