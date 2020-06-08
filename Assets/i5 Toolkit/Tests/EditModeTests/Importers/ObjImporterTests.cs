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

        private IEnumerator WaitForTask(Task task)
        {
            while (!task.IsCompleted) { yield return null; }
            if (task.IsFaulted) { throw task.Exception; }
            yield return null;
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

        //[Test]
        //public async void ImportAsync_WebRequestFailed_ReturnNull()
        //{
        //    ObjImporter objImporter = new ObjImporter();
        //    objImporter.ContentLoader = new FakeContentFailLoader();
        //    ServiceManager.RegisterService(objImporter);
        //    GameObject res = await objImporter.ImportAsync("http://test.org/test.obj");
        //    LogAssert.Expect(LogType.Error, new Regex(@"\w*Error fetching obj. No object imported\w*"));
        //    Assert.Null(res);
        //}

        //[Test]      
        //public async void ImportAsync_WebRequestSuccess_SetName()
        //{
        //    ObjImporter objImporter = new ObjImporter();
        //    objImporter.ContentLoader = new FakeContentLoader(cubeObj);
        //    ServiceManager.RegisterService(objImporter);
        //    GameObject res = await objImporter.ImportAsync("http://test.org/test.obj");
        //    Assert.NotNull(res);
        //    Assert.AreEqual("test", res.name);
        //}

        //[Test]
        //public async void ImportAsync_EmptyObj_ReturnGameObject()
        //{
        //    ObjImporter objImporter = new ObjImporter();
        //    objImporter.ContentLoader = new FakeContentLoader(emptyObj);
        //    ServiceManager.RegisterService(objImporter);
        //    GameObject res = await objImporter.ImportAsync("http://test.org/test.obj");
        //    Assert.NotNull(res);
        //    Assert.AreEqual(0, res.transform.childCount);
        //}

        [UnityTest]
        public IEnumerator ImportAsync_CubeObj_HasChild()
        {
            ObjImporter objImporter = new ObjImporter();
            objImporter.ContentLoader = new FakeContentLoader(cubeObj);
            ServiceManager.RegisterService(objImporter);
            ServiceManager.GetService<MtlLibraryService>().ContentLoader = new FakeContentLoader(cubeMtl);

            Task<GameObject> testTask = objImporter.ImportAsync("http://test.org/test.obj");

            yield return AsyncTest.WaitForTask(testTask);

            GameObject res = testTask.Result;

            Assert.NotNull(res);
            Assert.AreEqual(1, res.transform.childCount);
        }

        //[Test]
        //public async void ImportAsync_CubeObj_ChildHasComponents()
        //{
        //    ObjImporter objImporter = new ObjImporter();
        //    objImporter.ContentLoader = new FakeContentLoader(cubeObj);
        //    ServiceManager.RegisterService(objImporter);
        //    GameObject res = await objImporter.ImportAsync("http://test.org/test.obj");
        //    MeshFilter mf = res.transform.GetChild(0).GetComponent<MeshFilter>();
        //    Assert.NotNull(mf);
        //    MeshRenderer mr = res.transform.GetChild(0).GetComponent<MeshRenderer>();
        //    Assert.NotNull(mr);
        //}

        //[Test]
        //public async void ImportAsync_CubeObj_ChildHasCorrectMesh()
        //{
        //    ObjImporter objImporter = new ObjImporter();
        //    objImporter.ContentLoader = new FakeContentLoader(cubeObj);
        //    ServiceManager.RegisterService(objImporter);
        //    GameObject res = await objImporter.ImportAsync("http://test.org/test.obj");
        //    MeshFilter mf = res.transform.GetChild(0).GetComponent<MeshFilter>();
        //    Assert.AreEqual(8, mf.sharedMesh.vertices.Length);
        //    Assert.AreEqual(12, mf.sharedMesh.triangles.Length);
        //}

        //[Test]
        //public async void ImportAsync_CubeObj_ChildHasCorrectMaterial()
        //{
        //    ObjImporter objImporter = new ObjImporter();
        //    objImporter.ContentLoader = new FakeContentLoader(cubeObj);
        //    ServiceManager.RegisterService(objImporter);
        //    GameObject res = await objImporter.ImportAsync("http://test.org/test.obj");
        //    MeshRenderer mr = res.transform.GetChild(0).GetComponent<MeshRenderer>();
        //    Assert.AreEqual("TestMaterial", mr.material.name);
        //}

        //[Test]
        //public async void ImportAsync_CubeObj_HasThreeChildren()
        //{
        //    ObjImporter objImporter = new ObjImporter();
        //    objImporter.ContentLoader = new FakeContentLoader(cubeObj);
        //    ServiceManager.RegisterService(objImporter);
        //    GameObject res = await objImporter.ImportAsync("http://test.org/test.obj");
        //    Assert.AreEqual(3, res.transform.childCount);
        //}
    }
}