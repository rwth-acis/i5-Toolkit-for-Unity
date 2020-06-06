using i5.Toolkit.ProceduralGeometry;
using NUnit.Framework;
using System.Text.RegularExpressions;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Tests.ProceduralGeometry
{
    public class MaterialConstructorTests
    {
        [SetUp]
        public void ResetScene()
        {
            EditorSceneManager.OpenScene("Assets/i5 Toolkit/Tests/TestResources/SetupTestScene.unity");
        }

        [Test]
        public void ConstructMaterial_DefaultSettings_GeneratesStandardMaterial()
        {
            MaterialConstructor materialConstructor = new MaterialConstructor();
            Material mat = materialConstructor.ConstructMaterial();
            Assert.IsNotNull(mat);
            Assert.AreEqual(Shader.Find("Standard"), mat.shader);
            Assert.AreEqual(materialConstructor.Name, mat.name);
        }

        [Test]
        public void ConstructMaterial_NameSet_MaterialHasName()
        {
            MaterialConstructor materialConstructor = new MaterialConstructor();
            string materialName = "My Material " + Random.Range(0, 10000);
            materialConstructor.Name = materialName;
            Material mat = materialConstructor.ConstructMaterial();
            Assert.IsNotNull(mat);
            Assert.AreEqual(Shader.Find("Standard"), mat.shader);
            Assert.AreEqual(materialName, mat.name);
        }

        [Test]
        public void ConstructMaterial_ColorSet_MaterialHasColor()
        {
            MaterialConstructor materialConstructor = new MaterialConstructor();
            Color color = Random.ColorHSV();
            materialConstructor.Color = color;
            Material mat = materialConstructor.ConstructMaterial();
            Assert.IsNotNull(mat);
            Assert.AreEqual(Shader.Find("Standard"), mat.shader);
            Assert.AreEqual(color, mat.color);
        }

        [Test]
        public void ConstructMaterial_ShaderSet_MaterialHasShader()
        {
            MaterialConstructor materialConstructor = new MaterialConstructor();
            string shaderName = "Unlit/Color";
            materialConstructor.ShaderName = shaderName;
            Material mat = materialConstructor.ConstructMaterial();
            Assert.IsNotNull(mat);
            Assert.AreEqual(Shader.Find(shaderName), mat.shader);
        }

        [Test]
        public void ConstructMaterial_TexturesNotFetched_GivesWarning()
        {
            MaterialConstructor materialConstructor = new MaterialConstructor();
            materialConstructor.SetTexture("_MainTex", new TextureConstructor(""));
            Material mat = materialConstructor.ConstructMaterial();
            LogAssert.Expect(LogType.Warning, new Regex(@"\w*Constructed material which has unfetched textures.\w*"));
        }

        [Test]
        public async void FetchDependencies_NoTexturesProvided_NoError()
        {
            MaterialConstructor materialConstructor = new MaterialConstructor();
            await materialConstructor.FetchDependencies();
        }
    }
}