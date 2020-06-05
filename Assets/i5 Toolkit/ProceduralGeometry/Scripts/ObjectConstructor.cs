using i5.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.ProceduralGeometry
{
    public class ObjectConstructor
    {
        public GeometryConstructor GeometryConstructor { get; set; }
        
        public MaterialConstructor Material { get; set; }

        public ObjectConstructor()
        {
            GeometryConstructor = new GeometryConstructor();
            Material = new MaterialConstructor();
        }

        public ObjectConstructor(GeometryConstructor geometryConstructor)
        {
            GeometryConstructor = geometryConstructor;
        }

        public ObjectConstructor(GeometryConstructor geometryConstructor, MaterialConstructor material) : this(geometryConstructor)
        {
            Material = material;
        }

        public GameObject ConstructObject(Transform parent = null)
        {
            GameObject gameObject = ObjectPool<GameObject>.RequestResource(() => { return new GameObject("Object Constructor Result"); });

            gameObject.name = GeometryConstructor.Name;
            if (parent != null)
            {
                gameObject.transform.parent = parent;
            }

            MeshFilter meshFilter = ComponentUtilities.GetOrAddComponent<MeshFilter>(gameObject);
            MeshRenderer meshRenderer = ComponentUtilities.GetOrAddComponent<MeshRenderer>(gameObject);

            if (Material != null)
            {
                meshRenderer.material = Material.ConstructMaterial();
            }
            else
            {
                meshRenderer.material = new Material(Shader.Find("Standard"));
            }
            Mesh mesh = GeometryConstructor.ConstructMesh();
            meshFilter.sharedMesh = mesh;

            return gameObject;
        }
    }
}