# Persistent Scene

## Use Case

When switching a scene in Unity using the replace method, the GameObjects from one scene are unloaded and the content of the next scene is loaded.
There are use cases where some GameObjects in Unity should persist across scene changes, e.g. if they are global manager objects or also the player object.
For these, Unity provides the `DontDestroyOnLoad` method.
However, DontDestroyOnLoad is discouraged as it does not give much control over the object.
For instance, it is not possible to undo this persitent status again.

A better solution than DontDestroyOnLoad is to manually create a persistent scene that exists for the entire execution time of the application.
The i5 Toolkit provides a possible implementation for this.
GameObjects can be marked as persistent so that they are not unloaded in scene changes.
Instead, the GameObjects are transfered to a "i5 Persistent Scene".

## Usage

To make an object persist between scene changes, use the function <xref:i5.Toolkit.Core.Utilities.PersistenceScene.MarkPersistent*>:

```[C#]
PersistenceScene.MarkPersistent(gameObject);
```

If you are using the Persistence Scene solution, make sure that you load new scenes in the additive mode and not the single mode:
Do not use the single mode, as this will unload the persistent scene.

```[C#]
SceneManager.LoadScene("NewScene", LoadSceneMode.Additive);
```

If the GameObject should not be persistent anymore, you can revert its status using the command <xref:i5.Toolkit.Core.Utilities.PersistenceScene.UnmarkPersistent*>.
Note that this will move the GameObject into the currently active scene.
This is not guaranteed to be the same scene as the one it was originally in.

```[C#]
PersistenceScene.UnmarkPersistent(gameObject);
```

## Functionality

When using the <xref:i5.Toolkit.Core.Utilities.PersistenceScene>, it will create a new scene "i5 Persistent Scene" and load it in an additive mode.
Objects which are marked as persistent are transferred into this scene.
As long as scripts do not unload the persistent scene, GameObjects in it will persist across scene changes.

> When talking about persistent objects, this means making GameObjects persistent within one application session with regard to scene changes.
> This feature does not create persistence in-between sessions like a save-load solution.