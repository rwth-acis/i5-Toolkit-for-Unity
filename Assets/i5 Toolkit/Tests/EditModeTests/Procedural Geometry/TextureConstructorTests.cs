using System.Collections;
using System.Collections.Generic;
using i5.Toolkit.ProceduralGeometry;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Tests.ProceduralGeometry
{
    public class TextureConstructorTests
    {
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
            string loadPath = "loadPath";
            TextureConstructor textureConstructor = new TextureConstructor(loadPath);
            Assert.AreEqual(loadPath, textureConstructor.LoadPath);
        }
    }
}
