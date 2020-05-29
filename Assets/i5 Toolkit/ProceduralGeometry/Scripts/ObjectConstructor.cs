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

        public void ConstructObject()
        {
            GameObject go = ObjectPool<GameObject>.RequestResource(() => { return new GameObject("Object Constructor Result"); });

            go.name = GeometryConstructor.Name;

            MeshFilter meshFilter = ComponentUtilities.GetOrAddComponent<MeshFilter>(go);
            MeshRenderer meshRenderer = ComponentUtilities.GetOrAddComponent<MeshRenderer>(go);

            meshRenderer.material = Material.ConstructMaterial();
            Mesh mesh = GeometryConstructor.ConstructMesh();
            meshFilter.sharedMesh = mesh;
        }
    }
}