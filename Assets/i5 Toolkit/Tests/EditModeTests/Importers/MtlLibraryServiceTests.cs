using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using i5.Toolkit.ModelImporters;
using i5.Toolkit.ServiceCore;
using i5.Toolkit.Utilities.ContentLoaders;
using Microsoft.MixedReality.Toolkit.Utilities;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Tests.ModelImporters 
{
    public class MtlLibraryServiceTests
    {
        MtlLibraryService mtlLibraryService;

        [SetUp]
        public void ResetScene()
        {
            EditorSceneManager.OpenScene("Assets/i5 Toolkit/Tests/TestResources/SetupTestScene.unity");
            ServiceManager.RegisterService(new MtlLibraryService());
            mtlLibraryService = ServiceManager.GetService<MtlLibraryService>();
            mtlLibraryService.ContentLoader = new FakeContentLoader();
        }

        [Test]
        public async void LoadLibraryAsync_NewLibrary_LibraryLoadedReturnsTrue()
        {
            Uri testUri = new Uri("http://www.test.org/MatLib.mtl");
            string libraryName = "MatLib";
            await mtlLibraryService.LoadLibraryAsyc(testUri, libraryName);

            bool loaded = mtlLibraryService.LibraryLoaded(libraryName);
            Assert.IsTrue(loaded, "The library should have been loaded but is not displayed as loaded");
        }

        [Test]
        public async void LoadLibraryAsync_LoadFailed_LibraryLoadedReturnsFalse()
        {
            mtlLibraryService.ContentLoader = new FakeContentFailLoader();
            Uri testUri = new Uri("http://www.test.org/MatLib.mtl");
            string libraryName = "MatLib";
            await mtlLibraryService.LoadLibraryAsyc(testUri, libraryName);

            bool loaded = mtlLibraryService.LibraryLoaded(libraryName);
            Assert.IsFalse(loaded, "The import should have aborted but apparently, the library is shown as imported");
        }

        [Test]
        public async void LoadLibraryAsync_LoadLibraryMultipleTimes_NoErrorsAndLibraryLoadedTrue()
        {
            Uri testUri = new Uri("http://www.test.org/MatLib.mtl");
            string libraryName = "MatLib";
            await mtlLibraryService.LoadLibraryAsyc(testUri, libraryName);

            bool loaded = mtlLibraryService.LibraryLoaded(libraryName);
            Assert.IsTrue(loaded, "The library should have been loaded but is not displayed as loaded");

            await mtlLibraryService.LoadLibraryAsyc(testUri, libraryName);
            LogAssert.Expect(LogType.Warning, new Regex(@"\w*was already loaded\w*"));
            loaded = mtlLibraryService.LibraryLoaded(libraryName);
            Assert.IsTrue(loaded, "Loading the same library multiple times makes the library show up as unloaded");
        }
    }

    class FakeContentLoader : IContentLoader
    {
        public Task<Response> LoadAsync(string uri)
        {
            string content = File.ReadAllText("Assets/i5 Toolkit/Tests/EditModeTests/Importers/Data/MatLib.mtl");
            Response resp = new Response(true, content, new byte[0], 200);
            return new Task<Response>(() => { return resp; });
        }
    }

    class FakeContentFailLoader : IContentLoader
    {
        public Task<Response> LoadAsync(string uri)
        {
            Response resp = new Response(false, "This is a simulated fail", new byte[0], 404);
            return new Task<Response>(() => { return resp; });
        }
    }
}