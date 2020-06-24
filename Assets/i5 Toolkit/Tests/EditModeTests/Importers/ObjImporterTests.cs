using i5.Toolkit.ModelImporters;
using i5.Toolkit.ServiceCore;
using i5.Toolkit.TestUtilities;
using i5.Toolkit.Utilities;
using i5.Toolkit.Utilities.ContentLoaders;
using NUnit.Framework;
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

        /// <summary>
        /// Called one time to load the contents of the .obj and .mtl files
        /// </summary>
        [OneTimeSetUp]
        public void LoadData()
        {
            cubeObj = File.ReadAllText(PathUtils.GetPackagePath() + "Tests/EditModeTests/Importers/Data/Cube.obj");
            emptyObj = File.ReadAllText(PathUtils.GetPackagePath() + "Tests/EditModeTests/Importers/Data/EmptyObj.obj");
            threeObj = File.ReadAllText(PathUtils.GetPackagePath() + "Tests/EditModeTests/Importers/Data/ThreeObj.obj");
            cubeMtl = File.ReadAllText(PathUtils.GetPackagePath() + "Tests/EditModeTests/Importers/Data/Cube.mtl");
            emptyMtl = File.ReadAllText(PathUtils.GetPackagePath() + "Tests/EditModeTests/Importers/Data/EmptyObj.mtl");
            threeMtl = File.ReadAllText(PathUtils.GetPackagePath() + "Tests/EditModeTests/Importers/Data/ThreeObj.mtl");
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
            objImporter.ContentLoader = new FakeContentLoader(objContent);
            ServiceManager.RegisterService(objImporter);
            ServiceManager.GetService<MtlLibraryService>().ContentLoader = new FakeContentLoader(mtlContent);
            return objImporter;
        }

        /// <summary>
        /// Checks that the ContentLoader is initialized with the UnityWebRequestLoader by default
        /// </summary>
        [Test]
        public void ContentLoader_Initialized_InitWithUnityWebRequestLoader()
        {
            ObjImporter objImporter = new ObjImporter();
            objImporter.Initialize(ServiceManager.Instance);
            Assert.NotNull(objImporter.ContentLoader);
            Assert.True(objImporter.ContentLoader.GetType() == typeof(UnityWebRequestLoader));
        }

        /// <summary>
        /// Checks that the service initialization automatically registers an instance of the MtlLibraryService
        /// </summary>
        [Test]
        public void Initialize_Initialized_MtlLibraryServiceRegistered()
        {
            ObjImporter objImporter = new ObjImporter();
            objImporter.Initialize(ServiceManager.Instance);
            bool exists = ServiceManager.ServiceExists<MtlLibraryService>();
            Assert.True(exists);
        }

        /// <summary>
        /// Checks that hte service cleanup automatically unregisters the MtlLibraryService
        /// </summary>
        [Test]
        public void Cleaup_AfterCleanup_MtlLibraryServiceUnregistered()
        {
            ObjImporter objImporter = new ObjImporter();
            objImporter.Initialize(ServiceManager.Instance);
            bool exists = ServiceManager.ServiceExists<MtlLibraryService>();
            Assert.True(exists);
            objImporter.Cleanup();
            exists = ServiceManager.ServiceExists<MtlLibraryService>();
            Assert.False(exists);
        }

        /// <summary>
        /// Checks that ImportAsync returns null if the web request failed
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator ImportAsync_WebRequestFailed_ReturnNull()
        {
            ObjImporter objImporter = new ObjImporter();
            objImporter.ContentLoader = new FakeContentFailLoader();
            ServiceManager.RegisterService(objImporter);
            Task<GameObject> task = objImporter.ImportAsync("http://test.org/test.obj");

            yield return AsyncTest.WaitForTask(task);

            GameObject res = task.Result;

            LogAssert.Expect(LogType.Error, new Regex(@"\w*Error fetching obj. No object imported\w*"));
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

            Task<GameObject> task = objImporter.ImportAsync("http://test.org/test.obj");

            yield return AsyncTest.WaitForTask(task);
            GameObject res = task.Result;

            LogAssert.Expect(LogType.Warning, new Regex(@"\w*There is an object without parsed vertices\w*"));
            LogAssert.Expect(LogType.Error, new Regex(@"\w*No objects could be constructed.\w*"));

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
            ServiceManager.GetService<MtlLibraryService>().ContentLoader = new FakeContentFailLoader();

            Task<GameObject> task = objImporter.ImportAsync("http://test.org/test.obj");
            yield return AsyncTest.WaitForTask(task);
            GameObject res = task.Result;

            LogAssert.Expect(LogType.Error, new Regex(@"\w*This is a simulated fail\w*"));
            LogAssert.Expect(LogType.Error, new Regex(@"\w*Could not load .mtl file\w*"));

            Assert.NotNull(res);
            Assert.AreEqual(1, res.transform.childCount);
            MeshRenderer mr = res.transform.GetChild(0).GetComponent<MeshRenderer>();
            Assert.AreEqual("New Material", mr.sharedMaterial.name);
        }
    }
}