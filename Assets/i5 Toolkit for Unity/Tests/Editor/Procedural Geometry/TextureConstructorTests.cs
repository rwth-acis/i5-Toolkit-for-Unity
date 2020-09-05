using i5.Toolkit.Core.Editor.TestHelpers;
using i5.Toolkit.Core.ProceduralGeometry;
using i5.Toolkit.Core.TestUtilities;
using NUnit.Framework;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Core.Tests.ProceduralGeometry
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
            EditModeTestUtilities.ResetScene();
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
            Texture2D expected = new Texture2D(2, 2);
            textureConstructor.TextureLoader = FakeContentLoaderFactory.CreateFakeLoader(expected);

            Task<Texture2D> task = textureConstructor.FetchTextureAsync();
            yield return AsyncTest.WaitForTask(task);
            Texture2D res = task.Result;

            Assert.NotNull(res);
            Assert.AreEqual(expected.imageContentsHash, res.imageContentsHash);
        }

        /// <summary>
        /// Checks that the texture constructor returns null if the web request failed
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator FetchTextureAsync_WebRequestFailed_ReturnsNull()
        {
            TextureConstructor textureConstructor = new TextureConstructor(loadPath);
            textureConstructor.TextureLoader = FakeContentLoaderFactory.CreateFakeFailLoader<Texture2D>();

            LogAssert.Expect(LogType.Error, new Regex(@"\w*This is a simulated fail\w*"));

            Task<Texture2D> task = textureConstructor.FetchTextureAsync();
            yield return AsyncTest.WaitForTask(task);
            Texture2D res = task.Result;

            Assert.Null(res);
        }
    }
}
