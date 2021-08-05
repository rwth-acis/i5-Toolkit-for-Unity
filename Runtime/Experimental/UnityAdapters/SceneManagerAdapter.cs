using UnityEngine;
using UnityEngine.SceneManagement;

namespace i5.Toolkit.Core.Experimental.UnityAdapters
{
    public class SceneManagerAdapter : ISceneManager
    {
        public Scene GetSceneByBuildIndex(int index)
        {
            return SceneManager.GetSceneByBuildIndex(index);
        }

        public AsyncOperation LoadSceneAsync(int sceneBuildIndex)
        {
            return SceneManager.LoadSceneAsync(sceneBuildIndex);
        }

        public AsyncOperation LoadSceneAsync(int sceneBuildIndex, LoadSceneMode mode)
        {
            return SceneManager.LoadSceneAsync(sceneBuildIndex, mode);
        }

        public AsyncOperation UnloadSceneAsync(Scene scene)
        {
            return SceneManager.UnloadSceneAsync(scene);
        }
    }
}