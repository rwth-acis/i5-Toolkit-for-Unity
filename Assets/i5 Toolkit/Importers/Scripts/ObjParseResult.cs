using i5.Toolkit.ProceduralGeometry;

namespace i5.Toolkit.ModelImporters
{
    public class ObjParseResult
    {
        public ObjectConstructor ObjectConstructor { get; private set; }

        public string LibraryPath { get; set; }

        public string MaterialName { get; set; }

        public ObjParseResult()
        {
            ObjectConstructor = new ObjectConstructor();
        }
    }
}