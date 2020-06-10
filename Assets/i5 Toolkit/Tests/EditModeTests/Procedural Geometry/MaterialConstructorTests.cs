using i5.Toolkit.ProceduralGeometry;
using i5.Toolkit.TestUtilities;
using NUnit.Framework;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Tests.ProceduralGeometry
{
    public class MaterialConstructorTests
    {
        private FakeTextureConstructor fakeTextureConstructor;
        private FakeTextureConstructorFail fakeTextureConstructorFail;

        [SetUp]
        public void ResetScene()
        {
            EditorSceneManager.OpenScene("Assets/i5 Toolkit/Tests/TestResources/SetupTestScene.unity");
            fakeTextureConstructor = new FakeTextureConstructor();
            fakeTextureConstructorFail = new FakeTextureConstructorFail();
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
            materialConstructor.SetTexture("_MainTex", fakeTextureConstructor);
            Material mat = materialConstructor.ConstructMaterial();
            LogAssert.Expect(LogType.Warning, new Regex(@"\w*Constructed material which has unfetched textures.\w*"));
        }

        [UnityTest]
        public IEnumerator FetchDependencies_NoTexturesProvided_ReturnsTrue()
        {
            MaterialConstructor materialConstructor = new MaterialConstructor();
            Task<bool> task = materialConstructor.FetchDependencies();

            yield return AsyncTest.WaitForTask(task);

            bool success = task.Result;

            Assert.True(success);
        }

        [UnityTest]
        public IEnumerator FetchDependencies_TextureFetchSuccess_ReturnsTrue()
        {
            MaterialConstructor materialConstructor = new MaterialConstructor();
            materialConstructor.SetTexture("tex", fakeTextureConstructor);
            Task<bool> task = materialConstructor.FetchDependencies();

            yield return AsyncTest.WaitForTask(task);

            bool success = task.Result;

            Assert.True(success);
        }

        [UnityTest]
        public IEnumerator FetchDependencies_TextureFetchFail_ReturnsFalse()
        {
            MaterialConstructor materialConstructor = new MaterialConstructor();
            materialConstructor.SetTexture("tex", fakeTextureConstructorFail);
            Task<bool> task = materialConstructor.FetchDependencies();

            yield return AsyncTest.WaitForTask(task);

            bool success = task.Result;

            Assert.False(success);
        }

        [UnityTest]
        public IEnumerator ConstructMaterial_FetchedTexture_TextureSetInMaterial()
        {
            MaterialConstructor materialConstructor = new MaterialConstructor();
            materialConstructor.SetTexture("_MainTex", fakeTextureConstructor);
            Task<bool> task = materialConstructor.FetchDependencies();

            yield return AsyncTest.WaitForTask(task);
            bool success = task.Result;

            Assert.True(success);

            Task<Texture2D> textureTask = fakeTextureConstructor.FetchTextureAsync();

            yield return AsyncTest.WaitForTask(textureTask);
            Texture2D expectedTexture = textureTask.Result;

            Material mat = materialConstructor.ConstructMaterial();
            Assert.NotNull(mat.mainTexture);
            Assert.AreEqual(expectedTexture.imageContentsHash, mat.mainTexture.imageContentsHash);
        }
    }
}