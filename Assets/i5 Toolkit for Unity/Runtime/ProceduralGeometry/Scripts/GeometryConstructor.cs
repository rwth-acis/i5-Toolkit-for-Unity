using i5.Toolkit.Core.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.ProceduralGeometry
{
    /// <summary>
    /// Utility class which guides the construction of meshes
    /// Provides helper methods to register vertices and create faces of different shapes
    /// </summary>
    public class GeometryConstructor
    {
        /// <summary>
        /// The vertices of the geometry object
        /// </summary>
        public List<Vector3> Vertices { get; private set; }
        /// <summary>
        /// Manually set normals
        /// </summary>
        public List<Vector3> Normals { get; private set; }
        /// <summary>
        /// Manually set UV coords
        /// </summary>
        public List<Vector2> UVCoords { get; private set; }
        /// <summary>
        /// The triangles/faces of the geometry object
        /// </summary>
        public List<int> Triangles { get; private set; }

        /// <summary>
        /// The name of the produced mesh
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Creates the geometry constructor to buid the mesh data
        /// You can only add geometry, not remove it
        /// </summary>
        public GeometryConstructor()
        {
            Vertices = new List<Vector3>();
            Normals = new List<Vector3>();
            UVCoords = new List<Vector2>();
            Triangles = new List<int>();
            Name = "New Mesh";
        }

        /// <summary>
        /// Adds a disconnected, unnamed vertex to the geometry
        /// </summary>
        /// <param name="coordinates">The coordinates of the vertex</param>
        /// <returns>The index of the created vertex</returns>
        public int AddVertex(Vector3 coordinates)
        {
            Vertices.Add(coordinates);
            return Vertices.Count - 1;
        }

        /// <summary>
        /// Adds a disconnected vertex to the geometry with a given normal vector
        /// The normal vector is only used if a normal vector is supplied for every vertex
        /// </summary>
        /// <param name="coordinates">The coordinates of the vertex</param>
        /// <param name="normalVector">The normal vector which should be used</param>
        /// <returns>The index of the created vertex</returns>
        public int AddVertex(Vector3 coordinates, Vector3 normalVector)
        {
            Normals.Add(normalVector);
            return AddVertex(coordinates);
        }

        /// <summary>
        /// Adds a disconnected vertex to the geometry with the given coordinates, UV coordinates and normal vector
        /// </summary>
        /// <param name="coordinates">The 3D coordinates of the vertex</param>
        /// <param name="uvCoordinates">The texture UV coordinates of the vertex</param>
        /// <param name="normalVector">The normal vector of the vertex</param>
        /// <returns>Returns the index of the created vertex</returns>
        public int AddVertex(Vector3 coordinates, Vector2 uvCoordinates, Vector3 normalVector)
        {
            UVCoords.Add(uvCoordinates);
            return AddVertex(coordinates, normalVector);
        }

        /// <summary>
        /// Adds a disconnected vertex to the geometry with the given coordinates and UV coordinates
        /// </summary>
        /// <param name="coordinates">The 3D coordinates of the vertex</param>
        /// <param name="uvCoordinates">The texture UV coordinates of the vertex</param>
        /// <returns>Returns the index of the created vertex</returns>
        public int AddVertex(Vector3 coordinates, Vector2 uvCoordinates)
        {
            UVCoords.Add(uvCoordinates);
            return AddVertex(coordinates);
        }

        /// <summary>
        /// Adds a triangle to the geometry
        /// List the three vertices in clockwise order as seen from the outside
        /// The indices must exist in the geometry, i.e. they first need to be added using AddVertex()
        /// </summary>
        /// <param name="v0">Index of vertex 1</param>
        /// <param name="v1">Index of vertex 2</param>
        /// <param name="v2">Index of vertex 3</param>
        /// <param name="flipNormal">If set to true, the triangle will face the other way</param>
        public void AddTriangle(int v0, int v1, int v2, bool flipNormal = false)
        {
            if (CheckVertexIndex(v0) && CheckVertexIndex(v1) && CheckVertexIndex(v2))
            {
                if (flipNormal)
                {
                    Triangles.Add(v0);
                    Triangles.Add(v2);
                    Triangles.Add(v1);
                }
                else
                {
                    Triangles.Add(v0);
                    Triangles.Add(v1);
                    Triangles.Add(v2);
                }
            }
        }

        /// <summary>
        /// Adds a quad to the geometry (by adding two triangles)
        /// List the four vertices in clockwise order as seen from the outside
        /// The diagonal will be created between the first and third vertex
        /// The indices must exist in the geometry, i.e. they first need to be added using AddVertex()
        /// </summary>
        /// <param name="v0">Index of vertex 1</param>
        /// <param name="v1">Index of vertex 2</param>
        /// <param name="v2">Index of vertex 3</param>
        /// <param name="v3">Index of vertex 4</param>
        /// /// <param name="flipNormals">If set to true, the quad will face the other way</param>
        public void AddQuad(int v0, int v1, int v2, int v3, bool flipNormals = false)
        {
            if (CheckVertexIndex(v0) && CheckVertexIndex(v1) && CheckVertexIndex(v2) && CheckVertexIndex(v3))
            {
                // add two triangles: top right triangle and bottom left triangle
                AddTriangle(v0, v1, v2, flipNormals);
                AddTriangle(v0, v2, v3, flipNormals);
            }
        }

        /// <summary>
        /// Adds a fan of triangles to the geometry
        /// List the otherVertices clockwise
        /// The indices must exist in the geometry, i.e. they first need to be added using AddVertex()
        /// </summary>
        /// <param name="poleVertex">The pole vertex which is connected to all other vertices of the fan</param>
        /// <param name="otherVertices">The vertices which span the fan</param>
        /// <param name="flipNormals">If set to true, the triangle fan will face the other way</param>
        public void AddTriangleFan(int poleVertex, int[] otherVertices, bool flipNormals = false)
        {
            if (CheckVertexIndex(poleVertex))
            {
                // check if all vertices are valid
                for (int i = 0; i < otherVertices.Length; i++)
                {
                    if (!CheckVertexIndex(otherVertices[i]))
                    {
                        return;
                    }
                }

                for (int i = 0; i < otherVertices.Length - 1; i++)
                {
                    AddTriangle(poleVertex, otherVertices[i], otherVertices[i + 1], flipNormals);
                }
            }
        }

        /// <summary>
        /// Builds a mesh from the constructed geometry data
        /// </summary>
        /// <returns>The constructed mesh which is described by these geometry data</returns>
        public Mesh ConstructMesh()
        {
            Mesh mesh = ObjectPool<Mesh>.RequestResource(() => { return new Mesh(); });
            if (Vertices.Count > 65535)
            {
                mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            }
            else
            {
                mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt16;
            }
            mesh.name = Name;
            mesh.SetVertices(Vertices);
            mesh.SetTriangles(Triangles, 0);
            // assign the normals: use the ones supplied if there is one for every vertex or recalculate them otherwise
            if (Vertices.Count == Normals.Count)
            {
                mesh.SetNormals(Normals);
            }
            else
            {
                // if some normals were added: create warning so that developer is not confused that recalculated normals are used instead
                if (Normals.Count > 0)
                {
                    i5Debug.LogWarning("Some normals were supplied but there are vertices without normals." +
                        " The mesh will use calculated normals instead." +
                        " To avoid this, supply normal vectors for every vertex that is added to the geometry constructor.", this);
                }
                mesh.RecalculateNormals();
            }
            // assign the UV coordinates: use the ones supplied if there is one for every vertex or use none otherwise
            if (Vertices.Count == UVCoords.Count)
            {
                mesh.SetUVs(0, UVCoords);
            }
            else
            {
                // if some UV coordinates were added: create a warning that they are not used
                if (UVCoords.Count > 0)
                {
                    i5Debug.LogWarning("Some UV coordinates were set but there are vertices without UV coordinates." +
                        "Therefore, no UV coordinates will be used.", this);
                }
            }
            return mesh;
        }

        /// <summary>
        /// Checks if the given vertex index is in the bounds of the vertex array
        /// </summary>
        /// <param name="vertexIndex">The index of the vertex to check</param>
        /// <returns>True if the referenced vertex exists, false otherwise</returns>
        private bool CheckVertexIndex(int vertexIndex)
        {
            if (vertexIndex < 0 || vertexIndex >= Vertices.Count)
            {
                i5Debug.LogError("Geometry Construction Error: Referenced index is out of mesh vertices bounds: " + vertexIndex, this);
                return false;
            }
            return true;
        }
    }
}