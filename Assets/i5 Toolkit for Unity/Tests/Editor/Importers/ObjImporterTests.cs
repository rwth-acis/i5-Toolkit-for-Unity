using FakeItEasy;
using i5.Toolkit.Core.Editor.TestHelpers;
using i5.Toolkit.Core.ModelImporters;
using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.TestHelpers;
using i5.Toolkit.Core.TestUtilities;
using i5.Toolkit.Core.Utilities;
using i5.Toolkit.Core.Utilities.ContentLoaders;
using NUnit.Framework;
using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TestTools;
using i5.Toolkit.Core.Caching;

namespace i5.Toolkit.Core.Tests.ModelImporters
{
    /// <summary>
    /// Tests for the ObjImporter class
    /// </summary>
    public class ObjImporterTests
    {
        /// <summary>
        /// Contents of the .obj files
        /// There is a file with no object, one object and three objects
        /// </summary>
        private static string emptyObj, cubeObj, threeObj;
        /// <summary>
        /// Contents of the .mtl files
        /// There is a file with no material, one material and three materials
        /// </summary>
        private static string emptyMtl, cubeMtl, threeMtl;

        private static string onlineObjPath = "https://people.sc.fsu.edu/~jburkardt/data/obj/airboat.obj";

        /// <summary>
        /// Called one time to load the contents of the .obj and .mtl files
        /// </summary>
        [OneTimeSetUp]
        public void LoadData()
        {
            string basePath = "Tests/Editor/Importers/Data/";
            cubeObj = File.ReadAllText(PackagePathUtils.GetPackagePath() + basePath + "Cube.obj");
            emptyObj = File.ReadAllText(PackagePathUtils.GetPackagePath() +  basePath + "EmptyObj.obj");
            threeObj = File.ReadAllText(PackagePathUtils.GetPackagePath() + basePath + "ThreeObj.obj");
            cubeMtl = File.ReadAllText(PackagePathUtils.GetPackagePath() + basePath + "Cube.mtl");
            emptyMtl = File.ReadAllText(PackagePathUtils.GetPackagePath() + basePath + "EmptyObj.mtl");
            threeMtl = File.ReadAllText(PackagePathUtils.GetPackagePath() + basePath + "ThreeObj.mtl");
        }

        /// <summary>
        /// Resets the scene to the standard test scene before executed each test
        /// </summary>
        [SetUp]
        public void ResetScene()
        {
            EditModeTestUtilities.ResetScene();
        }

        /// <summary>
        /// Reusable function to set up the ObjImporter service and to register it at the service manager
        /// </summary>
        /// <param name="objContent">The .obj that the fake loader of the obj importer should load</param>
        /// <param name="mtlContent">The .mtl that the fake loader of the obj importer should load</param>
        /// <returns></returns>
        private ObjImporter SetUpObjImporter(string objContent, string mtlContent)
        {
            ObjImporter objImporter = new ObjImporter();
            IServiceManager serviceManager = A.Fake<IServiceManager>();
            objImporter.Initialize(serviceManager);
            objImporter.ContentLoader = FakeContentLoaderFactory.CreateFakeLoader(objContent);
            objImporter.MtlLibrary.ContentLoader = FakeContentLoaderFactory.CreateFakeLoader(mtlContent);
            return objImporter;
        }

        /// <summary>
        /// Reusable function to set up the ObjImporter service that uses a Cache and to register it at the service manager
        /// </summary>
        /// <returns></returns>
        private Tuple<ObjImporter, FileCache> SetUpObjImporterWithCache()
        {
            IServiceManager serviceManager = A.Fake<IServiceManager>();
            FileCache fCache = new FileCache();
            fCache.Initialize(serviceManager);
            ObjImporter objImporter = new ObjImporter();
            objImporter.activateChache();
            objImporter.Initialize(serviceManager);
            return new Tuple<ObjImporter, FileCache>(objImporter, fCache);
        }

        /// <summary>
        /// Checks that the ContentLoader is initialized with the UnityWebRequestLoader by default
        /// </summary>
        [Test]
        public void ContentLoader_Initialized_InitWithUnityWebRequestLoader()
        {
            ObjImporter objImporter = new ObjImporter();
            IServiceManager serviceManager = A.Fake<IServiceManager>();
            objImporter.Initialize(serviceManager);
            Assert.NotNull(objImporter.ContentLoader);
            Assert.True(objImporter.ContentLoader.GetType() == typeof(UnityWebRequestLoader));
        }

        /// <summary>
        /// Checks that the service initialization automatically initializes the MtlLibrary
        /// </summary>
        [Test]
        public void Initialize_Initialized_MtlLibrarySetUp()
        {
            ObjImporter objImporter = new ObjImporter();
            IServiceManager serviceManager = A.Fake<IServiceManager>();
            objImporter.Initialize(serviceManager);
            Assert.NotNull(objImporter.MtlLibrary);
        }

        /// <summary>
        /// Checks that hte obj importer creates a new pool
        /// </summary>
        [Test]
        public void Initialize_CreatesNewPool()
        {
            ObjImporter objImporter = new ObjImporter();
            IServiceManager serviceManager = A.Fake<IServiceManager>();
            int poolCount = ObjectPool<GameObject>.CountPools();
            objImporter.Initialize(serviceManager);
            Assert.AreEqual(poolCount + 1, ObjectPool<GameObject>.CountPools());
        }

        [Test]
        public void Cleanup_RemovesPool()
        {
            ObjImporter objImporter = new ObjImporter();
            IServiceManager serviceManager = A.Fake<IServiceManager>();
            int poolCount = ObjectPool<GameObject>.CountPools();
            objImporter.Initialize(serviceManager);
            Assert.AreEqual(poolCount + 1, ObjectPool<GameObject>.CountPools());

            objImporter.Cleanup();
            Assert.AreEqual(poolCount, ObjectPool<GameObject>.CountPools());
        }

        /// <summary>
        /// Checks that the cacheaware content loader is used as the content loader when caching is enabled.
        /// </summary>
        [Test]
        public void Cachawarecontent_When_Setup_With_Cache()
        {
            Tuple<ObjImporter, FileCache> setup = SetUpObjImporterWithCache();
            ObjImporter objImporter = setup.Item1;
            FileCache fileCache = setup.Item2;

            Assert.IsTrue(objImporter.ContentLoader.GetType() == typeof(CachAwareContentLoader));
        }

        /// <summary>
        /// Checks that ImportAsync returns null if the web request failed
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator ImportAsync_WebRequestFailed_ReturnNull()
        {
            ObjImporter objImporter = new ObjImporter();
            IServiceManager serviceManager = A.Fake<IServiceManager>();
            objImporter.Initialize(serviceManager);
            objImporter.ContentLoader = FakeContentLoaderFactory.CreateFakeFailLoader<string>();

            LogAssert.Expect(LogType.Error, new Regex(@"\w*Error fetching obj. No object imported\w*"));

            Task<GameObject> task = objImporter.ImportAsync("http://test.org/test.obj");

            yield return AsyncTest.WaitForTask(task);

            GameObject res = task.Result;

            Assert.Null(res);
        }

        /// <summary>
        /// Checks that ImportAsync sets the name of the GameObject correctly based on the name of the .obj file
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator ImportAsync_WebRequestSuccess_SetName()
        {
            ObjImporter objImporter = SetUpObjImporter(cubeObj, cubeMtl);

            Task<GameObject> task = objImporter.ImportAsync("http://test.org/test.obj");
            yield return AsyncTest.WaitForTask(task);
            GameObject res = task.Result;

            Assert.NotNull(res);
            Assert.AreEqual("test", res.name);
        }

        /// <summary>
        /// Checks that a GameObject is returned if an empty .obj is loaded
        /// and that the corresponding warnings and errors are given
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator ImportAsync_EmptyObj_ReturnGameObject()
        {
            ObjImporter objImporter = SetUpObjImporter(emptyObj, emptyMtl);

            LogAssert.Expect(LogType.Warning, new Regex(@"\w*There is an object without parsed vertices\w*"));
            LogAssert.Expect(LogType.Error, new Regex(@"\w*No objects could be constructed.\w*"));

            Task<GameObject> task = objImporter.ImportAsync("http://test.org/test.obj");

            yield return AsyncTest.WaitForTask(task);
            GameObject res = task.Result;

            Assert.NotNull(res);
            Assert.AreEqual(0, res.transform.childCount);
        }

        /// <summary>
        /// Checks that a child object was created if the cube .obj file is imported
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator ImportAsync_CubeObj_HasChild()
        {
            ObjImporter objImporter = SetUpObjImporter(cubeObj, cubeMtl);

            Task<GameObject> testTask = objImporter.ImportAsync("http://test.org/test.obj");

            yield return AsyncTest.WaitForTask(testTask);

            GameObject res = testTask.Result;

            Assert.NotNull(res);
            Assert.AreEqual(1, res.transform.childCount);
        }

        /// <summary>
        /// Checks that ImportAsync generates a child object with MeshFilter and MeshRenderer components
        /// when importing the cube .obj file
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator ImportAsync_CubeObj_ChildHasComponents()
        {
            ObjImporter objImporter = SetUpObjImporter(cubeObj, cubeMtl);

            Task<GameObject> task = objImporter.ImportAsync("http://test.org/test.obj");

            yield return AsyncTest.WaitForTask(task);

            GameObject res = task.Result;

            MeshFilter mf = res.transform.GetChild(0).GetComponent<MeshFilter>();
            Assert.NotNull(mf);
            MeshRenderer mr = res.transform.GetChild(0).GetComponent<MeshRenderer>();
            Assert.NotNull(mr);
        }

        /// <summary>
        /// Checks that ImportAsync assigns the correct mesh to the created child GameObject
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator ImportAsync_CubeObj_ChildHasCorrectMesh()
        {
            ObjImporter objImporter = SetUpObjImporter(cubeObj, cubeMtl);

            Task<GameObject> task = objImporter.ImportAsync("http://test.org/test.obj");

            yield return AsyncTest.WaitForTask(task);

            GameObject res = task.Result;

            MeshFilter mf = res.transform.GetChild(0).GetComponent<MeshFilter>();
            Assert.AreEqual(24, mf.sharedMesh.vertices.Length); // 8 * 3 = 24 (every vertex belongs to three triangles)
            Assert.AreEqual(36, mf.sharedMesh.triangles.Length); // 12 *3 = 36
        }

        /// <summary>
        /// Checks that ImportAsync assigns the correct material to the generated child GameObject
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator ImportAsync_CubeObj_ChildHasCorrectMaterial()
        {
            ObjImporter objImporter = SetUpObjImporter(cubeObj, cubeMtl);

            Task<GameObject> task = objImporter.ImportAsync("http://test.org/test.obj");
            yield return AsyncTest.WaitForTask(task);
            GameObject res = task.Result;

            MeshRenderer mr = res.transform.GetChild(0).GetComponent<MeshRenderer>();
            Assert.AreEqual("TestMaterial", mr.sharedMaterial.name);
        }

        /// <summary>
        /// Checks that ImportAsync creates three child objects for the three objects in ThreeObj.obj
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator ImportAsync_ThreeObj_HasThreeChildren()
        {
            ObjImporter objImporter = SetUpObjImporter(threeObj, threeMtl);

            Task<GameObject> task = objImporter.ImportAsync("http://test.org/test.obj");
            yield return AsyncTest.WaitForTask(task);
            GameObject res = task.Result;

            Assert.AreEqual(3, res.transform.childCount);
        }

        /// <summary>
        /// Checks that ImportAsync creates an object wtih a default material if the .obj file could be loaded
        /// but the .mtl file could not be loaded
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator ImportAsync_ObjFetchSuccessMtlFetchFail_CreateObjectWithDefaultMat()
        {
            ObjImporter objImporter = SetUpObjImporter(cubeObj, "");
            objImporter.MtlLibrary.ContentLoader = FakeContentLoaderFactory.CreateFakeFailLoader<string>();

            LogAssert.Expect(LogType.Error, new Regex(@"\w*This is a simulated fail\w*"));
            LogAssert.Expect(LogType.Error, new Regex(@"\w*Could not load .mtl file\w*"));

            Task<GameObject> task = objImporter.ImportAsync("http://test.org/test.obj");
            yield return AsyncTest.WaitForTask(task);
            GameObject res = task.Result;

            Assert.NotNull(res);
            Assert.AreEqual(1, res.transform.childCount);
            MeshRenderer mr = res.transform.GetChild(0).GetComponent<MeshRenderer>();
            Assert.AreEqual("New Material", mr.sharedMaterial.name);
        }

        /// <summary>
        /// Checks that gameobject can be loaded from the web when the cache is activated.
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator ImportAsync_Loading_Web_With_Cache_Enabled()
        {
            ObjImporter objImporter = SetUpObjImporterWithCache().Item1;
            
            Task<GameObject> task = objImporter.ImportAsync(onlineObjPath);
            yield return AsyncTest.WaitForTask(task);
            GameObject res = task.Result;

            Assert.NotNull(res);
        }

        /// <summary>
        /// Importing objects asynchronous is checked to use the cache.
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator ImportAsync_Cached_When_Loading_Twice()
        {
            Tuple<ObjImporter, FileCache> setup = SetUpObjImporterWithCache();
            ObjImporter objImporter = setup.Item1;
            FileCache fileCache = setup.Item2;

            Task<GameObject> task = objImporter.ImportAsync(onlineObjPath);
            yield return AsyncTest.WaitForTask(task);

            string cachResult = fileCache.getCachedFileLocation(onlineObjPath);
            
            //LogAssert.Expect(LogType.Log, "Cache hit");
            Assert.IsNotEmpty(cachResult);
        }
    }
}