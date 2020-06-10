using i5.Toolkit.ProceduralGeometry;
using i5.Toolkit.TestUtilities;
using i5.Toolkit.Utilities;
using NUnit.Framework;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Tests.ProceduralGeometry
{
    /// <summary>
    /// Tests for the TextureConstructor class
    /// </summary>
    public class TextureConstructorTests
    {
        /// <summary>
        /// Fake load path for the TextureLoader
        /// </summary>
        private const string loadPath = "loadPath";

        /// <summary>
        /// Loads the SetupTestScene before every test
        /// </summary>
        [SetUp]
        public void ResetScene()
        {
            EditorSceneManager.OpenScene("Assets/i5 Toolkit/Tests/TestResources/SetupTestScene.unity");
        }

        /// <summary>
        /// Checks that the TextureLoader is always initialized with a default object
        /// </summary>
        [Test]
        public void TextureConstructor_ConstructorCalled_TextureLoaderInitialized()
        {
            TextureConstructor textureConstructor = new TextureConstructor("");
            Assert.IsNotNull(textureConstructor.TextureLoader);
        }

        /// <summary>
        /// Checks that the LoadPath is initialized with the value specified in the constructor
        /// </summary>
        [Test]
        public void TextureConstructor_ConstructorCalled_LoadPathInitialized()
        {
            TextureConstructor textureConstructor = new TextureConstructor(loadPath);
            Assert.AreEqual(loadPath, textureConstructor.LoadPath);
        }

        /// <summary>
        /// Checks that the texture constructor returns the correct texture
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator FetchTextureAsync_WebRequestSuccessful_ReturnsTexture()
        {
            TextureConstructor textureConstructor = new TextureConstructor(loadPath);
            FakeTextureLoader fakeTextureLoader = new FakeTextureLoader();
            textureConstructor.TextureLoader = fakeTextureLoader;
            Task<Texture2D> task = textureConstructor.FetchTextureAsync();

            yield return AsyncTest.WaitForTask(task);
            Texture2D res = task.Result;

            Task<WebResponse<Texture2D>> taskExpectedRes = fakeTextureLoader.LoadTextureAsync("");
            yield return AsyncTest.WaitForTask(taskExpectedRes);
            WebResponse<Texture2D> expectedResp = taskExpectedRes.Result;

            Assert.NotNull(res);
            Assert.AreEqual(expectedResp.Content.imageContentsHash, res.imageContentsHash);
        }

        /// <summary>
        /// Checks that the texture constructor returns null if the web request failed
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator FetchTextureAsync_WebRequestFailed_ReturnsNull()
        {
            TextureConstructor textureConstructor = new TextureConstructor(loadPath);
            textureConstructor.TextureLoader = new FakeTextureFailLoader();
            Task<Texture2D> task = textureConstructor.FetchTextureAsync();

            yield return AsyncTest.WaitForTask(task);
            Texture2D res = task.Result;

            LogAssert.Expect(LogType.Error, new Regex(@"\w*This is a simulated fail\w*"));
            Assert.Null(res);
        }
    }
}
