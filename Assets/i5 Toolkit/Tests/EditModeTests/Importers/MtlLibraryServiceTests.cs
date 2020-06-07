using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using i5.Toolkit.ModelImporters;
using i5.Toolkit.ProceduralGeometry;
using i5.Toolkit.ServiceCore;
using i5.Toolkit.Utilities;
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
        private MtlLibraryService mtlLibraryService;
        private string content;

        private FakeContentLoader fakeContentLoader;

        private const string libraryName = "MatLib";

        [OneTimeSetUp]
        public void Initialize()
        {
            content = File.ReadAllText("Assets/i5 Toolkit/Tests/EditModeTests/Importers/Data/MatLib.mtl");
            fakeContentLoader = new FakeContentLoader(content);
        }

        [SetUp]
        public void ResetScene()
        {
            EditorSceneManager.OpenScene("Assets/i5 Toolkit/Tests/TestResources/SetupTestScene.unity");
            ServiceManager.RegisterService(new MtlLibraryService());
            mtlLibraryService = ServiceManager.GetService<MtlLibraryService>();
        }

        [Test]
        public async void LoadLibraryAsync_NewLibrary_LibraryLoadedReturnsTrue()
        {
            mtlLibraryService.ContentLoader = fakeContentLoader;
            await LoadLibrary();

            bool loaded = mtlLibraryService.LibraryLoaded(libraryName);
            Assert.IsTrue(loaded, "The library should have been loaded but is not displayed as loaded");
        }

        [Test]
        public async void LoadLibraryAsync_LoadFailed_LibraryLoadedReturnsFalse()
        {
            mtlLibraryService.ContentLoader = fakeContentLoader;
            await LoadLibrary();

            bool loaded = mtlLibraryService.LibraryLoaded(libraryName);
            Assert.IsFalse(loaded, "The import should have aborted but apparently, the library is shown as imported");
        }

        [Test]
        public async void LoadLibraryAsync_LoadLibraryMultipleTimes_NoErrorsAndLibraryLoadedTrue()
        {
            mtlLibraryService.ContentLoader = fakeContentLoader;
            await LoadLibrary();

            bool loaded = mtlLibraryService.LibraryLoaded(libraryName);
            Assert.IsTrue(loaded, "The library should have been loaded but is not displayed as loaded");

            await LoadLibrary();
            LogAssert.Expect(LogType.Warning, new Regex(@"\w*was already loaded\w*"));
            loaded = mtlLibraryService.LibraryLoaded(libraryName);
            Assert.IsTrue(loaded, "Loading the same library multiple times makes the library show up as unloaded");
        }

        [Test]
        public void GetMaterialConstructor_NonExistentMatLib_ReturnsNull()
        {
            mtlLibraryService.ContentLoader = fakeContentLoader;
            MaterialConstructor res = mtlLibraryService.GetMaterialConstructor("", "");
            Assert.IsNull(res);
        }

        [Test]
        public async void GetMaterialConstructor_ExistentMatLibNonExistentMat_ReturnsNull()
        {
            mtlLibraryService.ContentLoader = fakeContentLoader;
            await LoadLibrary();
            MaterialConstructor res = mtlLibraryService.GetMaterialConstructor(libraryName, "");
            Assert.IsNull(res);
        }

        [Test]
        public async void GetMaterialConstructor_ExistentMatLibExistentMat_ReturnsNotNull()
        {
            mtlLibraryService.ContentLoader = fakeContentLoader;
            await LoadLibrary();
            MaterialConstructor res = mtlLibraryService.GetMaterialConstructor(libraryName, "BlueMat");
            Assert.IsNotNull(res);
        }

        [Test]
        public async void GetMaterialConstructor_ExistentMatLibExistentMat_MatConstrColorSet()
        {
            mtlLibraryService.ContentLoader = fakeContentLoader;
            await LoadLibrary();
            MaterialConstructor res = mtlLibraryService.GetMaterialConstructor(libraryName, "BlueMat");
            Assert.IsNotNull(res);
            Assert.AreEqual(new Color(0.185991f, 0.249956f, 0.800000f), res.Color);
        }

        [Test]
        public async void GetMaterialConstructor_ExistentMatLibExistentMat_MatConstrNameSet()
        {
            mtlLibraryService.ContentLoader = fakeContentLoader;
            await LoadLibrary();
            MaterialConstructor res = mtlLibraryService.GetMaterialConstructor(libraryName, "BlueMat");
            Assert.IsNotNull(res);
            Assert.AreEqual("BlueMat", res.Name);
        }

        [Test]
        public async void ExtendedLogging_Enabled_CommentsLogged()
        {
            mtlLibraryService.ContentLoader = fakeContentLoader;
            await LoadLibrary();
            LogAssert.Expect(LogType.Log, new Regex(@"\w*Comment found\w*"));
        }

        private async Task LoadLibrary()
        {
            Uri testUri = new Uri("http://www.test.org/MatLib.mtl");
            await mtlLibraryService.LoadLibraryAsyc(testUri, libraryName);
        }
    }

    class FakeContentLoader : IContentLoader
    {
        private string content;
        public FakeContentLoader(string content)
        {
            this.content = content;
        }

        public Task<WebResponse<string>> LoadAsync(string uri)
        {
            WebResponse<string> resp = new WebResponse<string>(content, new byte[0], 200);
            return new Task<WebResponse<string>>(() => { return resp; });
        }
    }

    class FakeContentFailLoader : IContentLoader
    {
        public Task<WebResponse<string>> LoadAsync(string uri)
        {
            WebResponse<string> resp = new WebResponse<string>("This is a simulated fail", 404);
            return new Task<WebResponse<string>>(() => { return resp; });
        }
    }
}