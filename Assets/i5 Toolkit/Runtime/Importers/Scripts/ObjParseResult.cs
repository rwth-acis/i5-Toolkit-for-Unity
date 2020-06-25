using i5.Toolkit.Core.ProceduralGeometry;

namespace i5.Toolkit.Core.ModelImporters
{
    /// <summary>
    /// Result of one sub-object of a parsing operation for a .obj file
    /// </summary>
    public class ObjParseResult
    {
        /// <summary>
        /// An object constructor which was for the sub-object
        /// </summary>
        public ObjectConstructor ObjectConstructor { get; private set; }

        /// <summary>
        /// Path to the material library that is used for the sub-object
        /// </summary>
        public string LibraryPath { get; set; }

        /// <summary>
        /// Name of the material which is used for the sub-object
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// Creates a new parse result
        /// </summary>
        public ObjParseResult()
        {
            ObjectConstructor = new ObjectConstructor();
        }
    }
}