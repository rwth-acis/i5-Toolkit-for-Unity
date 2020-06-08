using System.Collections;
using System.Collections.Generic;
using i5.Toolkit.ModelImporters;
using i5.Toolkit.ServiceCore;
using i5.Toolkit.Utilities.ContentLoaders;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Tests.ModelImporters
{
    public class ObjImporterTests
    {
        [SetUp]
        public void ResetScene()
        {
            EditorSceneManager.OpenScene("Assets/i5 Toolkit/Tests/TestResources/SetupTestScene.unity");
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
    }
}
