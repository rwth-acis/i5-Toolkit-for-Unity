using UnityEngine;
using UnityEngine.SceneManagement;

namespace i5.Toolkit.Core.Experimental.UnityAdapters
{
    public interface ISceneManager
    {
        Scene GetSceneByBuildIndex(int index);

        AsyncOperation LoadSceneAsync(int sceneBuildIndex);

        AsyncOperation LoadSceneAsync(int sceneBuildIndex, LoadSceneMode mode);

        AsyncOperation UnloadSceneAsync(Scene scene);

    }
}