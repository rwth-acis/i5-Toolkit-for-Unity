using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace i5.Toolkit.Core.TestHelpers
{
    public static class PlayModeTestUtilities
    {
        public static void LoadEmptyTestScene()
        {
            CheckPlayMode();
            Scene playScene = SceneManager.CreateScene("PlayTest Scene");
            SceneManager.SetActiveScene(playScene);
        }

        public static void UnloadTestScene()
        {
            CheckPlayMode();
            SceneManager.UnloadSceneAsync("PlayTest Scene");
        }

        private static void CheckPlayMode()
        {
            Assert.IsTrue(Application.isPlaying, "Play Mode Test Utilities can only be used in PlayMode");
        }
    }
}