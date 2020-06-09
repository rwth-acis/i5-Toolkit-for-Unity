using i5.Toolkit.ModelImporters;
using i5.Toolkit.ServiceCore;
using i5.Toolkit.TestUtilities;
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
    public class ObjImporterTests
    {
        private static string emptyObj, cubeObj, threeObj;
        private static string emptyMtl, cubeMtl, threeMtl;

        [OneTimeSetUp]
        public void LoadData()
        {
            cubeObj = File.ReadAllText("Assets/i5 Toolkit/Tests/EditModeTests/Importers/Data/Cube.obj");
            emptyObj = File.ReadAllText("Assets/i5 Toolkit/Tests/EditModeTests/Importers/Data/EmptyObj.obj");
            threeObj = File.ReadAllText("Assets/i5 Toolkit/Tests/EditModeTests/Importers/Data/ThreeObj.obj");
            cubeMtl = File.ReadAllText("Assets/i5 Toolkit/Tests/EditModeTests/Importers/Data/Cube.mtl");
            emptyMtl = File.ReadAllText("Assets/i5 Toolkit/Tests/EditModeTests/Importers/Data/EmptyObj.mtl");
            threeMtl = File.ReadAllText("Assets/i5 Toolkit/Tests/EditModeTests/Importers/Data/ThreeObj.mtl");
        }

        [SetUp]
        public void ResetScene()
        {
            EditorSceneManager.OpenScene("Assets/i5 Toolkit/Tests/TestResources/SetupTestScene.unity");
        }

        private ObjImporter SetUpObjImporter(string objContent, string mtlContent)
        {
            ObjImporter objImporter = new ObjImporter();
            objImporter.ContentLoader = new FakeContentLoader(objContent);
            ServiceManager.RegisterService(objImporter);
            ServiceManager.GetService<MtlLibraryService>().ContentLoader = new FakeContentLoader(mtlContent);
            return objImporter;
        }

        [Test]
        public void ContentLoader_Initialized_InitWithMRTKRestLoader()
        {
            ObjImporter objImporter = new ObjImporter();
            objImporter.Initialize(ServiceManager.Instance);
            Assert.NotNull(objImporter.ContentLoader);
            Assert.True(objImporter.ContentLoader.GetType() == typeof(MRTKRestLoader));
        }

        [Test]
        public void Initialize_Initialized_MtlLibraryServiceRegistered()
        {
            ObjImporter objImporter = new ObjImporter();
            objImporter.Initialize(ServiceManager.Instance);
            bool exists = ServiceManager.ServiceExists<MtlLibraryService>();
            Assert.True(exists);
        }

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

        [UnityTest]
        public IEnumerator ImportAsync_CubeObj_HasThreeChildren()
        {
            ObjImporter objImporter = SetUpObjImporter(threeObj, threeMtl);

            Task<GameObject> task = objImporter.ImportAsync("http://test.org/test.obj");
            yield return AsyncTest.WaitForTask(task);
            GameObject res = task.Result;

            Assert.AreEqual(3, res.transform.childCount);
        }

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