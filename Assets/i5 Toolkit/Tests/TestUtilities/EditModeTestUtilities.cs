using i5.Toolkit.Utilities;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace i5.Toolkit.TestUtilities
{
    public static class EditModeTestUtilities
    {
        public static void ResetScene()
        {
            Assert.IsTrue(Application.isEditor, "This scene reset only works in edit mode tests");
            EditorSceneManager.OpenScene(PathUtils.GetPackagePath() + "Tests/TestResources/SetupTestScene.unity");
        }
    }
}