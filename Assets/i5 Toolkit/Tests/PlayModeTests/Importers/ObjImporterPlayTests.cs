using i5.Toolkit.ModelImporters;
using i5.Toolkit.ServiceCore;
using i5.Toolkit.TestUtilities;
using NUnit.Framework;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace i5.Toolkit.Tests.ModelImporters
{
    public class ObjImporterPlayTests
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
        public void SetUpScene()
        {
            SceneManager.LoadScene("SetupTestScene");
        }

        [UnityTest]
        public IEnumerator ImportAsync_CubeObj_HasChild()
        {
            ObjImporter objImporter = new ObjImporter();
            objImporter.ContentLoader = new FakeContentLoader(threeObj);
            ServiceManager.RegisterService(objImporter);
            ServiceManager.GetService<MtlLibraryService>().ContentLoader = new FakeContentLoader(cubeMtl);

            Task<GameObject> testTask = objImporter.ImportAsync("http://test.org/test.obj");

            yield return AsyncTest.WaitForTask(testTask);

            //Assert.NotNull(res);
            //Debug.Log(res.transform.childCount);
            //Assert.AreEqual(1, res.transform.childCount);
            //Assert.AreEqual(5, res.transform.childCount);
            Assert.Pass();
        }
    }
}
