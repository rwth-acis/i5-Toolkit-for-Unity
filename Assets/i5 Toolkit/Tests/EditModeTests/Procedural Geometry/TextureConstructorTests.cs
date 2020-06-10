using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using i5.Toolkit.ProceduralGeometry;
using i5.Toolkit.TestUtilities;
using i5.Toolkit.Utilities;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Tests.ProceduralGeometry
{
    public class TextureConstructorTests
    {
        private const string loadPath = "loadPath";

        [SetUp]
        public void ResetScene()
        {
            EditorSceneManager.OpenScene("Assets/i5 Toolkit/Tests/TestResources/SetupTestScene.unity");
        }

        [Test]
        public void TextureConstructor_ConstructorCalled_TextureLoaderInitialized()
        {
            TextureConstructor textureConstructor = new TextureConstructor("");
            Assert.IsNotNull(textureConstructor.TextureLoader);
        }

        [Test]
        public void TextureConstructor_ConstructorCalled_LoadPathInitialized()
        {
            TextureConstructor textureConstructor = new TextureConstructor(loadPath);
            Assert.AreEqual(loadPath, textureConstructor.LoadPath);
        }

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
