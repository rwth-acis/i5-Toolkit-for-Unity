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
using i5.Toolkit.TestUtilities;
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

        [UnityTest]
        public IEnumerator LoadLibraryAsync_NewLibrary_LibraryLoadedReturnsTrue()
        {
            mtlLibraryService.ContentLoader = fakeContentLoader;
            Task task = LoadLibrary();

            yield return AsyncTest.WaitForTask(task);

            bool loaded = mtlLibraryService.LibraryLoaded(libraryName);
            Assert.IsTrue(loaded, "The library should have been loaded but is not displayed as loaded");
        }

        [UnityTest]
        public IEnumerator LoadLibraryAsync_LoadFailed_LibraryLoadedReturnsFalse()
        {
            mtlLibraryService.ContentLoader = fakeContentLoader;
            Task task = LoadLibrary();

            yield return AsyncTest.WaitForTask(task);

            bool loaded = mtlLibraryService.LibraryLoaded(libraryName);
            Assert.IsFalse(loaded, "The import should have aborted but apparently, the library is shown as imported");
        }

        [UnityTest]
        public IEnumerator LoadLibraryAsync_LoadLibraryMultipleTimes_NoErrorsAndLibraryLoadedTrue()
        {
            mtlLibraryService.ContentLoader = fakeContentLoader;
            Task task = LoadLibrary();

            yield return AsyncTest.WaitForTask(task);

            bool loaded = mtlLibraryService.LibraryLoaded(libraryName);
            Assert.IsTrue(loaded, "The library should have been loaded but is not displayed as loaded");

            Task task2 = LoadLibrary();

            yield return AsyncTest.WaitForTask(task2);

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

        [UnityTest]
        public IEnumerator GetMaterialConstructor_ExistentMatLibNonExistentMat_ReturnsNull()
        {
            mtlLibraryService.ContentLoader = fakeContentLoader;
            Task task = LoadLibrary();

            yield return AsyncTest.WaitForTask(task);

            MaterialConstructor res = mtlLibraryService.GetMaterialConstructor(libraryName, "");
            Assert.IsNull(res);
        }

        [UnityTest]
        public IEnumerator GetMaterialConstructor_ExistentMatLibExistentMat_ReturnsNotNull()
        {
            mtlLibraryService.ContentLoader = fakeContentLoader;
            Task task = LoadLibrary();

            yield return AsyncTest.WaitForTask(task);

            MaterialConstructor res = mtlLibraryService.GetMaterialConstructor(libraryName, "BlueMat");
            Assert.IsNotNull(res);
        }

        [UnityTest]
        public IEnumerator GetMaterialConstructor_ExistentMatLibExistentMat_MatConstrColorSet()
        {
            mtlLibraryService.ContentLoader = fakeContentLoader;
            Task task = LoadLibrary();

            yield return AsyncTest.WaitForTask(task);

            MaterialConstructor res = mtlLibraryService.GetMaterialConstructor(libraryName, "BlueMat");
            Assert.IsNotNull(res);
            Assert.AreEqual(new Color(0.185991f, 0.249956f, 0.800000f), res.Color);
        }

        [UnityTest]
        public IEnumerator GetMaterialConstructor_ExistentMatLibExistentMat_MatConstrNameSet()
        {
            mtlLibraryService.ContentLoader = fakeContentLoader;
            Task task = LoadLibrary();

            yield return AsyncTest.WaitForTask(task);

            MaterialConstructor res = mtlLibraryService.GetMaterialConstructor(libraryName, "BlueMat");
            Assert.IsNotNull(res);
            Assert.AreEqual("BlueMat", res.Name);
        }

        [UnityTest]
        public IEnumerator ExtendedLogging_Enabled_CommentsLogged()
        {
            mtlLibraryService.ContentLoader = fakeContentLoader;
            mtlLibraryService.ExtendedLogging = true;
            Task task = LoadLibrary();

            yield return AsyncTest.WaitForTask(task);

            LogAssert.Expect(LogType.Log, new Regex(@"\w*Comment found\w*"));
        }

        [UnityTest]
        public IEnumerator ExtendedLogging_Disabled_CommentsNotLogged()
        {
            mtlLibraryService.ContentLoader = fakeContentLoader;
            mtlLibraryService.ExtendedLogging = false;
            Task task = LoadLibrary();

            yield return AsyncTest.WaitForTask(task);

            LogAssert.NoUnexpectedReceived();
        }

        private async Task LoadLibrary()
        {
            Uri testUri = new Uri("http://www.test.org/MatLib.mtl");
            await mtlLibraryService.LoadLibraryAsyc(testUri, libraryName);
        }
    }
}