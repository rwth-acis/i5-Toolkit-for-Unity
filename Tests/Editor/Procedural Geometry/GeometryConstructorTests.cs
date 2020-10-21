using i5.Toolkit.Core.Editor.TestHelpers;
using i5.Toolkit.Core.ProceduralGeometry;
using NUnit.Framework;
using System;
using UnityEngine;

namespace i5.Toolkit.Core.Tests.ProceduralGeometry
{
    /// <summary>
    /// Tests for the GeometryConstructor class
    /// </summary>
    public class GeometryConstructorTests
    {
        [SetUp]
        public void ResetScene()
        {
            EditModeTestUtilities.ResetScene();
        }

        /// <summary>
        /// Checks that an empty mesh is generated if no input is provided to the geometry constructor
        /// </summary>
        [Test]
        public void ConstructMesh_NoInput_GeneratesEmptyMesh()
        {
            GeometryConstructor gc = new GeometryConstructor();
            Mesh mesh = gc.ConstructMesh();
            Assert.IsNotNull(mesh);
            Assert.IsEmpty(mesh.vertices);
            Assert.IsEmpty(mesh.triangles);
        }

        /// <summary>
        /// Checks that an added vertex exists in the generated mesh
        /// </summary>
        [Test]
        public void AddVertex_OneVertexAdded_VertexStored()
        {
            GeometryConstructor gc = new GeometryConstructor();
            int index = gc.AddVertex(Vector3.one);
            Assert.AreEqual(gc.Vertices.Count, 1);
            Assert.AreEqual(gc.Vertices[0], Vector3.one);
            Assert.AreEqual(index, 0);
        }

        /// <summary>
        /// Checks that an added triangle exists in the generated mesh
        /// </summary>
        [Test]
        public void AddTriangle_TriangleAdded_TriangleInMesh()
        {
            GeometryConstructor gc = new GeometryConstructor();
            Vector3[] vertices = new Vector3[]
            {
                Vector3.zero,
                new Vector3(0, 1,0),
                new Vector3(1, 0, 0)
            };
            int[] expectedTriangles = new int[] { 0, 1, 2 };

            int v0 = gc.AddVertex(vertices[0]);
            int v1 = gc.AddVertex(vertices[1]);
            int v2 = gc.AddVertex(vertices[2]);
            gc.AddTriangle(v0, v1, v2);
            Mesh mesh = gc.ConstructMesh();
            // check if the vertex indices are correct
            Assert.AreEqual(v0, expectedTriangles[0]);
            Assert.AreEqual(v1, expectedTriangles[1]);
            Assert.AreEqual(v2, expectedTriangles[2]);
            // check the mesh
            Assert.IsNotNull(mesh);
            Assert.AreEqual(mesh.vertices.Length, 3);
            Assert.AreEqual(mesh.triangles.Length, 3);
            Assert.AreEqual(mesh.vertices, vertices);
            Assert.AreEqual(mesh.triangles, expectedTriangles);
        }

        /// <summary>
        /// Checks that an added quad exists in the generated mesh
        /// </summary>
        [Test]
        public void AddQuad_QuadAdded_QuadInMesh()
        {
            GeometryConstructor gc = new GeometryConstructor();
            Vector3[] vertices = new Vector3[]
            {
                new Vector3(0, 0, 0),
                new Vector3(0, 1, 0),
                new Vector3(1, 1, 0),
                new Vector3(1, 0, 0)
            };
            int[] expectedTriangles = new int[] { 0, 1, 2, 0, 2, 3 };

            // perform test action
            int v0 = gc.AddVertex(vertices[0]);
            int v1 = gc.AddVertex(vertices[1]);
            int v2 = gc.AddVertex(vertices[2]);
            int v3 = gc.AddVertex(vertices[3]);
            gc.AddQuad(v0, v1, v2, v3);
            Mesh mesh = gc.ConstructMesh();

            // check
            Assert.AreEqual(v0, 0);
            Assert.AreEqual(v1, 1);
            Assert.AreEqual(v2, 2);
            Assert.AreEqual(v3, 3);
            Assert.AreEqual(gc.Vertices.ToArray(), vertices);
            Assert.AreEqual(gc.Triangles.ToArray(), expectedTriangles);
            Assert.AreEqual(mesh.vertices, vertices);
            Assert.AreEqual(mesh.triangles, expectedTriangles);
        }

        /// <summary>
        /// Checks that an added triangle fan exists in the generated mesh
        /// </summary>
        [Test]
        public void AddTriangleFan_FanAdded_FanInMesh()
        {
            GeometryConstructor gc = new GeometryConstructor();
            Vector3[] vertices = new Vector3[]
            {
                Vector3.zero,
                new Vector3(-1, -1, 0),
                new Vector3(-1, 1, 0),
                new Vector3(1 , 1, 0),
                new Vector3(1, -1, 0)
            };
            int[] indices = new int[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                indices[i] = gc.AddVertex(vertices[i]);
            }
            int[] outerVertexIndices = new int[indices.Length - 1];
            Array.Copy(indices, 1, outerVertexIndices, 0, indices.Length - 1);
            gc.AddTriangleFan(indices[0], outerVertexIndices);
            Mesh mesh = gc.ConstructMesh();

            // check
            Assert.AreEqual(gc.Vertices.ToArray(), vertices);
            int[] expectedTriangles = new int[]
            {
                0, 1, 2,
                0, 2, 3,
                0, 3, 4,
            };
            Assert.AreEqual(gc.Triangles.ToArray(), expectedTriangles);
            Assert.AreEqual(mesh.vertices, vertices);
            Assert.AreEqual(mesh.triangles, expectedTriangles);
        }
    }
}
