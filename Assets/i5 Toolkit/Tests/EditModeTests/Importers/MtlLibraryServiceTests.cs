using System.Collections;
using System.Collections.Generic;
using i5.Toolkit.ModelImporters;
using i5.Toolkit.ServiceCore;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Tests.ModelImporters 
{
    public class MtlLibraryServiceTests
    {
        [SetUp]
        public void ResetScene()
        {
            EditorSceneManager.OpenScene("Assets/i5 Toolkit/Tests/TestResources/SetupTestScene.unity");
            ServiceManager.RegisterService(new MtlLibraryService());
        }
    }
}