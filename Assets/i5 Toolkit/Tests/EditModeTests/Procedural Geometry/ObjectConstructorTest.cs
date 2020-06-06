using System.Collections;
using System.Collections.Generic;
using i5.Toolkit.ProceduralGeometry;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ObjectConstructorTest
    {
        [SetUp]
        public void ResetScene()
        {
            EditorSceneManager.OpenScene("Assets/i5 Toolkit/Tests/TestResources/SetupTestScene.unity");
        }

        /// <summary>
        /// Test that should not produce 
        /// </summary>
        [Test]
        public void CreateObjWithoutGeometry()
        {
            ObjectConstructor objConstructor = new ObjectConstructor();
            GameObject result = objConstructor.ConstructObject();
            LogAssert.Expect(LogType.Warning, "Created object with empty geometry."
                + "This might not be intended since you can just use Instantiate oder the ObjectPool.");
            Assert.IsNotNull(result);
            Assert.AreEqual("New GameObject", result.name);

            MeshFilter meshFilter = result.GetComponent<MeshFilter>();
            Assert.IsNull(meshFilter);
            MeshRenderer meshRenderer = result.GetComponent<MeshRenderer>();
            Assert.IsNull(meshRenderer);
        }

        [Test]
        public void CreateObjWithNullGeometry()
        {
            ObjectConstructor objConstructor = new ObjectConstructor();
            objConstructor.GeometryConstructor = null;
            GameObject result = objConstructor.ConstructObject();
            LogAssert.Expect(LogType.Warning, "Created object with empty geometry."
                + "This might not be intended since you can just use Instantiate oder the ObjectPool.");
            Assert.IsNotNull(result);
            Assert.AreEqual("New GameObject", result.name);

            MeshFilter meshFilter = result.GetComponent<MeshFilter>();
            Assert.IsNull(meshFilter);
            MeshRenderer meshRenderer = result.GetComponent<MeshRenderer>();
            Assert.IsNull(meshRenderer);
        }

        [Test]
        public void CreateObj_WithGeometry_NullMaterial()
        {
            ObjectConstructor objConstructor = new ObjectConstructor();
            GeometryConstructor geometryConstructor = CreateSimpleGeometry();
            objConstructor.GeometryConstructor = geometryConstructor;
            objConstructor.MaterialConstructor = null;
            GameObject result = objConstructor.ConstructObject();

            Assert.IsNotNull(result);
            Assert.AreEqual(geometryConstructor.Name, result.name);

            MeshFilter meshFilter = result.GetComponent<MeshFilter>();
            Assert.IsNotNull(meshFilter);
            MeshRenderer meshRenderer = result.GetComponent<MeshRenderer>();
            Assert.IsNotNull(meshRenderer);

            Assert.AreEqual(Shader.Find("Standard"), meshRenderer.sharedMaterial.shader);
        }

        [Test]
        public void CreateObj_WithGeometry_DefaultMaterial()
        {
            ObjectConstructor objConstructor = new ObjectConstructor();
            GeometryConstructor geometryConstructor = CreateSimpleGeometry();
            objConstructor.GeometryConstructor = geometryConstructor;
            GameObject result = objConstructor.ConstructObject();

            Assert.IsNotNull(result);
            Assert.AreEqual(geometryConstructor.Name, result.name);

            MeshFilter meshFilter = result.GetComponent<MeshFilter>();
            Assert.IsNotNull(meshFilter);
            MeshRenderer meshRenderer = result.GetComponent<MeshRenderer>();
            Assert.IsNotNull(meshRenderer);

            Assert.AreEqual(Shader.Find("Standard"), meshRenderer.sharedMaterial.shader);
        }

        [Test]
        public void CreateObj_WithGeometry_AssignedMaterial()
        {
            ObjectConstructor objConstructor = new ObjectConstructor();
            GeometryConstructor geometryConstructor = CreateSimpleGeometry();
            MaterialConstructor materialConstructor = new MaterialConstructor();
            materialConstructor.Color = Color.red;
            materialConstructor.Name = "RedMat";
            objConstructor.GeometryConstructor = geometryConstructor;
            objConstructor.MaterialConstructor = materialConstructor;
            GameObject result = objConstructor.ConstructObject();

            Assert.IsNotNull(result);
            Assert.AreEqual(geometryConstructor.Name, result.name);

            MeshFilter meshFilter = result.GetComponent<MeshFilter>();
            Assert.IsNotNull(meshFilter);
            MeshRenderer meshRenderer = result.GetComponent<MeshRenderer>();
            Assert.IsNotNull(meshRenderer);

            Assert.AreEqual(Shader.Find("Standard"), meshRenderer.sharedMaterial.shader);
            Assert.AreEqual(Color.red, meshRenderer.sharedMaterial.color);
            Assert.AreEqual(materialConstructor.Name, meshRenderer.sharedMaterial.name);
        }

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
