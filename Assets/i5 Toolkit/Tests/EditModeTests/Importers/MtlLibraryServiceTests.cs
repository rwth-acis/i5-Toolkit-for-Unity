using i5.Toolkit.ModelImporters;
using i5.Toolkit.ProceduralGeometry;
using i5.Toolkit.ServiceCore;
using i5.Toolkit.TestUtilities;
using i5.Toolkit.Utilities;
using NUnit.Framework;
using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Tests.ModelImporters
{
    /// <summary>
    /// Tests for the MtlLibraryService class
    /// </summary>
    public class MtlLibraryServiceTests
    {
        /// <summary>
        /// The instance of the MtlLibraryService
        /// </summary>
        private MtlLibraryService mtlLibraryService;
        /// <summary>
        /// Content from a material library which is loaded in a one-time initialization
        /// </summary>
        private string content;

        /// <summary>
        /// Fake content loader that mimics a content load operation from the Web
        /// </summary>
        private FakeContentLoader fakeContentLoader;

        /// <summary>
        /// The name of the example library which is used in these tests
        /// </summary>
        private const string libraryName = "MatLib";

        /// <summary>
        /// One-time initialization that loads a material library and sets up the fake content loader
        /// </summary>
        [OneTimeSetUp]
        public void Initialize()
        {
            content = File.ReadAllText(PathUtils.GetPackagePath() + "Tests/EditModeTests/Importers/Data/MatLib.mtl");
            fakeContentLoader = new FakeContentLoader(content);
        }

        /// <summary>
        /// Called before every test; resets the scene and registers the service
        /// </summary>
        [SetUp]
        public void ResetScene()
        {
            EditorSceneManager.OpenScene(PathUtils.GetPackagePath() + "Tests/TestResources/SetupTestScene.unity");
            ServiceManager.RegisterService(new MtlLibraryService());
            mtlLibraryService = ServiceManager.GetService<MtlLibraryService>();
        }

        /// <summary>
        /// CHecks that LoadLibraryAsync() returns true if the library could be loaded
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator LoadLibraryAsync_NewLibrary_LibraryLoadedReturnsTrue()
        {
            mtlLibraryService.ContentLoader = fakeContentLoader;
            Task task = LoadLibrary();

            yield return AsyncTest.WaitForTask(task);

            bool loaded = mtlLibraryService.LibraryLoaded(libraryName);
            Assert.IsTrue(loaded, "The library should have been loaded but is not displayed as loaded");
        }

        /// <summary>
        /// Checks that LoadLibraryAsync() returns false if the library could not be loaded
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator LoadLibraryAsync_LoadFailed_LibraryLoadedReturnsFalse()
        {
            mtlLibraryService.ContentLoader = new FakeContentFailLoader();
            Task task = LoadLibrary();

            yield return AsyncTest.WaitForTask(task);

            LogAssert.Expect(LogType.Error, new Regex(@"\w*This is a simulated fail\w*"));

            bool loaded = mtlLibraryService.LibraryLoaded(libraryName);
            Assert.IsFalse(loaded, "The import should have aborted but apparently, the library is shown as imported");
        }

        /// <summary>
        /// Checks that LoadLibraryAsync() does not break if the same library is loaded multiple times
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Checks that GetMaterialConstructor returns null if the specified material library was not loaded
        /// </summary>
        [Test]
        public void GetMaterialConstructor_NonExistentMatLib_ReturnsNull()
        {
            mtlLibraryService.ContentLoader = fakeContentLoader;
            MaterialConstructor res = mtlLibraryService.GetMaterialConstructor("", "");
            Assert.IsNull(res);
        }

        /// <summary>
        /// Checks that GetMaterialConstructor returns null if the material does not exist in the specified library
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator GetMaterialConstructor_ExistentMatLibNonExistentMat_ReturnsNull()
        {
            mtlLibraryService.ContentLoader = fakeContentLoader;
            Task task = LoadLibrary();

            yield return AsyncTest.WaitForTask(task);

            MaterialConstructor res = mtlLibraryService.GetMaterialConstructor(libraryName, "");
            Assert.IsNull(res);
        }

        /// <summary>
        /// Checks that GetMaterialConstructor with an existing material in a loaded library does not return null
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator GetMaterialConstructor_ExistentMatLibExistentMat_ReturnsNotNull()
        {
            mtlLibraryService.ContentLoader = fakeContentLoader;
            Task task = LoadLibrary();

            yield return AsyncTest.WaitForTask(task);

            MaterialConstructor res = mtlLibraryService.GetMaterialConstructor(libraryName, "BlueMat");
            Assert.IsNotNull(res);
        }

        /// <summary>
        /// Checks that GetMaterialConstructor of an existing material returns a MaterialConstructor with the correct color
        /// The color is specified in the .mat file in the Data folder
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Checks that GetMaterialConstructor of an existing material returns a MaterialConstructor with the correct name
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Checks that comments in the mtl file are logged if ExtendedLogging is set to true
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator ExtendedLogging_Enabled_CommentsLogged()
        {
            mtlLibraryService.ContentLoader = fakeContentLoader;
            mtlLibraryService.ExtendedLogging = true;
            Task task = LoadLibrary();

            yield return AsyncTest.WaitForTask(task);

            LogAssert.Expect(LogType.Log, new Regex(@"\w*Comment found\w*"));
        }

        /// <summary>
        /// Checks that comments in the mtl fiel are not logged if ExtendedLogging is set to false
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator ExtendedLogging_Disabled_CommentsNotLogged()
        {
            mtlLibraryService.ContentLoader = fakeContentLoader;
            mtlLibraryService.ExtendedLogging = false;
            Task task = LoadLibrary();

            yield return AsyncTest.WaitForTask(task);

            LogAssert.NoUnexpectedReceived();
        }

        /// <summary>
        /// Loads an example library
        /// The given url needs to be valid but is chosen arbitrarily since the content is returned by the FakeContentLoader
        /// </summary>
        /// <returns></returns>
        private async Task LoadLibrary()
        {
            Uri testUri = new Uri("http://www.test.org/MatLib.mtl");
            await mtlLibraryService.LoadLibraryAsyc(testUri, libraryName);
        }
    }
}