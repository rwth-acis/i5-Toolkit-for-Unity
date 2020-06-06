using i5.Toolkit.Utilities;
using UnityEngine;

namespace i5.Toolkit.ProceduralGeometry
{
    public class ObjectConstructor
    {
        public GeometryConstructor GeometryConstructor { get; set; }
        
        public MaterialConstructor MaterialConstructor { get; set; }

        public ObjectConstructor()
        {
            GeometryConstructor = new GeometryConstructor();
            MaterialConstructor = new MaterialConstructor();
        }

        public ObjectConstructor(GeometryConstructor geometryConstructor)
        {
            GeometryConstructor = geometryConstructor;
        }

        public ObjectConstructor(GeometryConstructor geometryConstructor, MaterialConstructor material) : this(geometryConstructor)
        {
            MaterialConstructor = material;
        }

        public GameObject ConstructObject(Transform parent = null)
        {
            GameObject gameObject = ObjectPool<GameObject>.RequestResource(() => { return new GameObject("Object Constructor Result"); });

            if (parent != null)
            {
                gameObject.transform.parent = parent;
            }

            if (GeometryConstructor == null || GeometryConstructor.Vertices.Count == 0)
            {
                Debug.LogWarning("Created object with empty geometry."
                + "This might not be intended since you can just use Instantiate oder the ObjectPool.");
                gameObject.name = "New GameObject";
            }
            else
            {
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