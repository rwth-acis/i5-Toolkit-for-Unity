﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class PersistenceScene
{
    private static Scene persistentScene;

    /// <summary>
    /// Gets or creates the persistent scene if it does not exist
    /// In the editor, this will always return the active scene since you should not create new scenes
    /// in the editor
    /// </summary>
    /// <returns>Returns the persistent scene</returns>
    public static Scene GetPersistentScene()
    {
#if UNITY_EDITOR
        // in the editor we should not create new scenes
        return SceneManager.GetActiveScene();
#endif
#if !UNITY_EDITOR
        // in play mode we can create 
        if (persistentScene == null)
        {
            persistentScene = SceneManager.CreateScene("i5 Persistent Scene");
        }
        return persistentScene;
#endif
    }

    /// <summary>
    /// Moves the provided GameObject to the persistent scene
    /// If this is executed in an editor build, this has no effect
    /// </summary>
    /// <param name="gameObject">GameObject which should be made persistent</param>
    public static void MarkPersistent(GameObject gameObject)
    {
        Scene targetScene = GetPersistentScene();
        SceneManager.MoveGameObjectToScene(gameObject, targetScene);
    }

    /// <summary>
    /// Makes an object not persist anymore by moving it to the active scene
    /// </summary>
    /// <param name="gameObject">The GameObject which should not be persistent anymore</param>
    public static void UnmarkPersistent(GameObject gameObject)
    {
        Scene targetScene = SceneManager.GetActiveScene();
        SceneManager.MoveGameObjectToScene(gameObject, targetScene);
    }
}