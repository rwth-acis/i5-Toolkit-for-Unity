using i5.Toolkit.ProceduralGeometry;
using i5.Toolkit.TestUtilities;
using i5.Toolkit.Utilities;
using NUnit.Framework;
using System.Text.RegularExpressions;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Tests.ProceduralGeometry
{
    /// <summary>
    /// Tests for the ObjectConstructor class
    /// </summary>
    public class ObjectConstructorTests
    {
        /// <summary>
        /// Sets up the scene before every test
        /// </summary>
        [SetUp]
        public void ResetScene()
        {
            EditModeTestUtilities.ResetScene();
        }

        /// <summary>
        /// Checks that an empty GeometryConstructor produces an empty GameObject and logs a warning
        /// </summary>
        [Test]
        public void ConstructObject_EmptyGeometry_CreatesEmptyGOWithWarning()
        {
            ObjectConstructor objConstructor = new ObjectConstructor();
            GameObject result = objConstructor.ConstructObject();

            AssertEmptyGameObjectCreated(result, "New GameObject");
        }

        /// <summary>
        /// Checks that a GeometryConstructor that is set to null produces an empty GameObject and logs a warning
        /// </summary>
        [Test]
        public void ConstructObject_GeometryConstructorNull_CreatesEmptyGOWithWarning()
        {
            ObjectConstructor objConstructor = new ObjectConstructor();
            objConstructor.GeometryConstructor = null;
            GameObject result = objConstructor.ConstructObject();

            AssertEmptyGameObjectCreated(result, "New GameObject");
        }

        /// <summary>
        /// Reusable function that checks if a given GameObject was created and that a warning is logged that the object is empty
        /// </summary>
        /// <param name="result">The produced GameObject</param>
        /// <param name="name">The expected name of the GameObject</param>
        private void AssertEmptyGameObjectCreated(GameObject result, string name)
        {
            LogAssert.Expect(LogType.Warning, new Regex(@"\w*Created object with empty geometry."
                + @"This might not be intended since you can just use Instantiate oder the ObjectPool.\w*"));
            Assert.IsNotNull(result);
            Assert.AreEqual("New GameObject", result.name);

            MeshFilter meshFilter = result.GetComponent<MeshFilter>();
            Assert.IsNull(meshFilter);
            MeshRenderer meshRenderer = result.GetComponent<MeshRenderer>();
            Assert.IsNull(meshRenderer);
        }

        /// <summary>
        /// Checks if the constructed object uses the default material if the material constructor is set to null
        /// </summary>
        [Test]
        public void ConstructObject_WithGeometryNullMaterial_GOWithMeshDefaultMat()
        {
            ObjectConstructor objConstructor = new ObjectConstructor();
            GeometryConstructor geometryConstructor = CreateSimpleGeometry();
            objConstructor.GeometryConstructor = geometryConstructor;
            objConstructor.MaterialConstructor = null;
            GameObject result = objConstructor.ConstructObject();

            AssertGameObjectWithGeometry(result, geometryConstructor, out MeshRenderer meshRenderer);
        }

        /// <summary>
        /// Checks if the default material settings create a GameObject with a default material
        /// </summary>
        [Test]
        public void ConstructObject_WithGeometryDefaultMaterial_GOWithMeshDefaultMat()
        {
            ObjectConstructor objConstructor = new ObjectConstructor();
            GeometryConstructor geometryConstructor = CreateSimpleGeometry();
            objConstructor.GeometryConstructor = geometryConstructor;
            GameObject result = objConstructor.ConstructObject();

            AssertGameObjectWithGeometry(result, geometryConstructor, out MeshRenderer meshRenderer);
        }

        /// <summary>
        /// Checks if the settings of the material constructor are applied to the generated GameObject
        /// </summary>
        [Test]
        public void ConstructObject_MaterialConstructorGiven_AssignedMaterial()
        {
            ObjectConstructor objConstructor = new ObjectConstructor();
            GeometryConstructor geometryConstructor = CreateSimpleGeometry();
            MaterialConstructor materialConstructor = new MaterialConstructor();
            materialConstructor.Color = Color.red;
            materialConstructor.Name = "RedMat";
            objConstructor.GeometryConstructor = geometryConstructor;
            objConstructor.MaterialConstructor = materialConstructor;
            GameObject result = objConstructor.ConstructObject();

            AssertGameObjectWithGeometry(result, geometryConstructor, out MeshRenderer meshRenderer);

            Assert.AreEqual(Color.red, meshRenderer.sharedMaterial.color);
            Assert.AreEqual(materialConstructor.Name, meshRenderer.sharedMaterial.name);
        }

        /// <summary>
        /// Reusable function that checks if the given GameObject has the correct geometry and shader
        /// </summary>
        /// <param name="result">The created GameObject that should be checked</param>
        /// <param name="geometryConstructor">The geometry constructor which was used to create the object's mesh</param>
        /// <param name="meshRenderer">The mesh renderer that is retrieved from the GameObject result</param>
        private void AssertGameObjectWithGeometry(GameObject result, GeometryConstructor geometryConstructor,
            out MeshRenderer meshRenderer)
        {
            Assert.IsNotNull(result);
            Assert.AreEqual(geometryConstructor.Name, result.name);

            MeshFilter meshFilter = result.GetComponent<MeshFilter>();
            Assert.IsNotNull(meshFilter);
            meshRenderer = result.GetComponent<MeshRenderer>();
            Assert.IsNotNull(meshRenderer);

            Assert.AreEqual(Shader.Find("Standard"), meshRenderer.sharedMaterial.shader);
        }

        /// <summary>
        /// Creates a GeometryConstructor with a simple plane geometry
        /// </summary>
        /// <returns>A GeometryConstructor with a single quad</returns>
        private GeometryConstructor CreateSimpleGeometry()
        {
            GeometryConstructor gc = new GeometryConstructor();
            int v1 = gc.AddVertex(new Vector3(0, 0, 0));
            int v2 = gc.AddVertex(new Vector3(0, 1, 0));
            int v3 = gc.AddVertex(new Vector3(1, 0, 0));
            int v4 = gc.AddVertex(new Vector3(1, 1, 0));
            gc.AddQuad(v1, v3, v4, v2);
            gc.Name = "Simple Geometry" + Random.Range(0, 10000);
            return gc;
        }
    }
}
