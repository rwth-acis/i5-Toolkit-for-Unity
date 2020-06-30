using i5.Toolkit.Core.Utilities;
using UnityEngine;

namespace i5.Toolkit.Core.ProceduralGeometry
{
    /// <summary>
    /// Class for constructing objects on demand
    /// </summary>
    public class ObjectConstructor
    {
        /// <summary>
        /// Constructor which defines the object's geometry
        /// </summary>
        public GeometryConstructor GeometryConstructor { get; set; }
        
        /// <summary>
        /// Constructor which defines the object's material
        /// </summary>
        public MaterialConstructor MaterialConstructor { get; set; }

        /// <summary>
        /// Creates the object constructor with empty geometry and material constructors
        /// </summary>
        public ObjectConstructor()
        {
            GeometryConstructor = new GeometryConstructor();
            MaterialConstructor = new MaterialConstructor();
        }

        /// <summary>
        /// Creates the object constructor with the given geometry constructor
        /// </summary>
        /// <param name="geometryConstructor">Geometry constructor to initialize the object</param>
        public ObjectConstructor(GeometryConstructor geometryConstructor)
        {
            GeometryConstructor = geometryConstructor;
            MaterialConstructor = new MaterialConstructor();
        }

        /// <summary>
        /// Creates the object constructor with the given geometry and material constructors
        /// </summary>
        /// <param name="geometryConstructor">Geometry constructor to initialize the object</param>
        /// <param name="material">Material constructor to initialize the object</param>
        public ObjectConstructor(GeometryConstructor geometryConstructor, MaterialConstructor material) : this(geometryConstructor)
        {
            MaterialConstructor = material;
        }

        /// <summary>
        /// Constructs a GameObject and populates it with the mesh of the geometry constructor and the material of the material constructor
        /// </summary>
        /// <param name="parent">Optional; Parents the GameObject to the specified transform</param>
        /// <returns>Returns the created GameObject</returns>
        public GameObject ConstructObject(Transform parent = null)
        {
            // get a GameObject from the pool
            GameObject gameObject = ObjectPool<GameObject>.RequestResource(() => { return new GameObject("Object Constructor Result"); });

            if (parent != null)
            {
                gameObject.transform.parent = parent;
            }

            // we can create objects without geometry but this is unnecessarily complex so we inform the dev about this
            if (GeometryConstructor == null || GeometryConstructor.Vertices.Count == 0)
            {
                Debug.LogWarning("Created object with empty geometry."
                + "This might not be intended since you can just use Instantiate oder the ObjectPool.");
                gameObject.name = "New GameObject";
            }
            else
            {
                // set up the GameObject
                gameObject.name = GeometryConstructor.Name;

                MeshFilter meshFilter = ComponentUtilities.GetOrAddComponent<MeshFilter>(gameObject);
                MeshRenderer meshRenderer = ComponentUtilities.GetOrAddComponent<MeshRenderer>(gameObject);

                if (MaterialConstructor != null)
                {
                    meshRenderer.material = MaterialConstructor.ConstructMaterial();
                }
                else
                {
                    meshRenderer.material = new Material(Shader.Find("Standard"));
                }
                Mesh mesh = GeometryConstructor.ConstructMesh();
                meshFilter.sharedMesh = mesh;
            }

            return gameObject;
        }
    }
}