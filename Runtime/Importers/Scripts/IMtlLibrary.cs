using i5.Toolkit.Core.ProceduralGeometry;
using i5.Toolkit.Core.Utilities.ContentLoaders;
using System;
using System.Threading.Tasks;

namespace i5.Toolkit.Core.ModelImporters
{
    public interface IMtlLibrary
    {
        bool ExtendedLogging { get; set; }

        IContentLoader<string> ContentLoader { get; set; }

        MaterialConstructor GetMaterialConstructor(string materialLibrary, string materialName);

        bool LibraryLoaded(string name);

        Task<bool> LoadLibraryAsyc(string absolutePath, string libraryName);
    }
}