using UnityEngine;
using UnityEngine.SceneManagement;

namespace i5.Toolkit.Core.Utilities.UnityAdapters
{
    public interface ISceneManager
    {
        public Scene GetSceneByBuildIndex(int index);

        public AsyncOperation LoadSceneAsync(int sceneBuildIndex);

        public AsyncOperation LoadSceneAsync(int sceneBuildIndex, LoadSceneMode mode);

        public AsyncOperation UnloadSceneAsync(Scene scene);

    }
}