using FakeItEasy;
using i5.Toolkit.Core.Editor.TestHelpers;
using i5.Toolkit.Core.ProceduralGeometry;
using i5.Toolkit.Core.TestHelpers;
using NUnit.Framework;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Core.Tests.ProceduralGeometry
{
    /// <summary>
    /// Tests for the MaterialConstructor class
    /// </summary>
    public class MaterialConstructorTests
    {
        /// <summary>
        /// Loads the test scene before each test and resets the texture constructors
        /// </summary>
        [SetUp]
        public void ResetScene()
        {
            EditModeTestUtilities.ResetScene();
        }

        /// <summary>
        /// Checks that the material constructor with default settings creates a standard material
        /// </summary>
        [Test]
        public void ConstructMaterial_DefaultSettings_GeneratesStandardMaterial()
        {
            MaterialConstructor materialConstructor = new MaterialConstructor();
            Material mat = materialConstructor.ConstructMaterial();
            Assert.IsNotNull(mat);
            Assert.AreEqual(Shader.Find("Standard"), mat.shader);
            Assert.AreEqual(materialConstructor.Name, mat.name);
        }

        /// <summary>
        /// Checks that the given name of the material constructor is set in the generated material
        /// </summary>
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

        /// <summary>
        /// Checks that the given color of the material constructor is set in the generated material
        /// </summary>
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

        /// <summary>
        /// Checks that the given shader of the material constructor is set in the generated material
        /// </summary>
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

        /// <summary>
        /// Checks that a warning is logged if a material is constructed without fetching specified textures
        /// </summary>
        [Test]
        public void ConstructMaterial_TexturesNotFetched_GivesWarning()
        {
            MaterialConstructor materialConstructor = new MaterialConstructor();
            materialConstructor.SetTexture("_MainTex", A.Fake<ITextureConstructor>());
            Material mat = materialConstructor.ConstructMaterial();
            LogAssert.Expect(LogType.Warning, new Regex(@"\w*Constructed material which has unfetched textures.\w*"));
        }

        /// <summary>
        /// Checks that FetchDependencies() returns true if there is nothing to fetch
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator FetchDependencies_NoTexturesProvided_ReturnsTrue()
        {
            MaterialConstructor materialConstructor = new MaterialConstructor();
            Task<bool> task = materialConstructor.FetchDependencies();

            yield return AsyncTest.WaitForTask(task);

            bool success = task.Result;

            Assert.True(success);
        }

        /// <summary>
        /// Checks that FetchDependencies() returns ture if the textures could be fetched
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator FetchDependencies_TextureFetchSuccess_ReturnsTrue()
        {
            MaterialConstructor materialConstructor = new MaterialConstructor();
            ITextureConstructor fakeTextureConstructor = A.Fake<ITextureConstructor>();
            A.CallTo(() => fakeTextureConstructor.FetchTextureAsync()).Returns(Task.FromResult(new Texture2D(2, 2)));

            materialConstructor.SetTexture("tex", fakeTextureConstructor);
            Task<bool> task = materialConstructor.FetchDependencies();

            yield return AsyncTest.WaitForTask(task);

            bool success = task.Result;

            Assert.True(success);
        }

        /// <summary>
        /// Checks that FetchDependencies() returns false if the textures could not be fetched
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator FetchDependencies_TextureFetchFail_ReturnsFalse()
        {
            MaterialConstructor materialConstructor = new MaterialConstructor();
            ITextureConstructor fakeTextureConstructorFail = A.Fake<ITextureConstructor>();
            A.CallTo(() => fakeTextureConstructorFail.FetchTextureAsync()).Returns(Task.FromResult<Texture2D>(null));
            materialConstructor.SetTexture("tex", fakeTextureConstructorFail);

            Task<bool> task = materialConstructor.FetchDependencies();

            yield return AsyncTest.WaitForTask(task);

            bool success = task.Result;

            Assert.False(success);
        }

        /// <summary>
        /// Checks that fetched textures are set in the generated material
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator ConstructMaterial_FetchedTexture_TextureSetInMaterial()
        {
            MaterialConstructor materialConstructor = new MaterialConstructor();
            Texture2D expectedTexture = new Texture2D(2, 2);
            ITextureConstructor fakeTextureConstructor = A.Fake<ITextureConstructor>();
            A.CallTo(() => fakeTextureConstructor.FetchTextureAsync()).Returns(Task.FromResult(expectedTexture));
            materialConstructor.SetTexture("_MainTex", fakeTextureConstructor);
            Task<bool> task = materialConstructor.FetchDependencies();

            yield return AsyncTest.WaitForTask(task);
            bool success = task.Result;

            Assert.True(success);

            Material mat = materialConstructor.ConstructMaterial();
            Assert.NotNull(mat.mainTexture);
            Assert.AreEqual(expectedTexture.imageContentsHash, mat.mainTexture.imageContentsHash);
        }
    }
}