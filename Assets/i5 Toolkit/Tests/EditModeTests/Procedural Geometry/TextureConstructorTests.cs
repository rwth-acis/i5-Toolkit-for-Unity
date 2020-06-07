using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using i5.Toolkit.ProceduralGeometry;
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

        [Test]
        public async void FetchTextureAsync_WebRequestSuccessful_ReturnsTexture()
        {
            TextureConstructor textureConstructor = new TextureConstructor(loadPath);
            textureConstructor.TextureLoader = new FakeTextureLoader();
            Texture2D res = await textureConstructor.FetchTextureAsync();
            Assert.NotNull(res);
            Assert.AreEqual(2, res.width);
            Assert.AreEqual(2, res.height);
        }

        [Test]
        public async void FetchTextureAsync_WebRequestFailed_ReturnsNull()
        {
            TextureConstructor textureConstructor = new TextureConstructor(loadPath);
            textureConstructor.TextureLoader = new FakeTextureFailLoader();
            Texture2D res = await textureConstructor.FetchTextureAsync();
            Assert.Null(res);
        }
    }

    class FakeTextureLoader : ITextureLoader
    {
        public Task<WebResponse<Texture2D>> LoadTextureAsync(string uri)
        {
            Texture2D tex = new Texture2D(2, 2);
            WebResponse<Texture2D> resp = new WebResponse<Texture2D>(tex, new byte[0], 200);
            return new Task<WebResponse<Texture2D>>(() => { return resp; });
        }
    }

    class FakeTextureFailLoader : ITextureLoader
    {
        public Task<WebResponse<Texture2D>> LoadTextureAsync(string uri)
        {
            WebResponse<Texture2D> resp = new WebResponse<Texture2D>("This is a simulated fail", 404);
            return new Task<WebResponse<Texture2D>>(() => { return resp; });
        }
    }
}
