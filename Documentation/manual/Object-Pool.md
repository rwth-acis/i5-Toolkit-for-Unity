# Object Pool

![Object Pool](../resources/Logos/ObjectPool.svg)

## Purpose

**The problem**:
Usually, objects like GameObjects and meshes are instantiated when they are needed so that they can be used in the scene.
If the object is not required anymore, it is usually destroyed.
Such calls of Unity's `Instantiate` and `Destroy` have a performance overhead which manifests in frame drops.
Destroyed objects also need to be collected by the garbage collector which intercepts the program execution and also leads to poor performance.
This is especially true if many objects need to be created or destroyed at once.

**The solution: Object Pools**
A common technique to avoid such frame drops is to pool the objects.
This means that a number of objects are created in advance, e.g. at application startup.
At this point in time, a short loading time can be accepted since the application is most likely also loading other resources at that point.
After that, the application should only pull objects from the pool to use them.
Once the object is not required anymore, it is not destroyed but instead returned to the pool.
Hence, objects are recycled which can decrease the amount of times that the garbage collector runs.

## Usage

### Requesting Objects

You can use ObjectPools for any type of object.
To request an object, e.g. a GameObject from the pool, call the following:

```[C#]
GameObject fromPool = ObjectPool<GameObject>.RequestResource(() => {return new GameObject("Newly created");});
```

`RequestResource` works in the following way:
It tries to fetch an object from the pool and returns it.
If the pool is empty, the method falls back to the factory method which is provided as an argument.
In the example, the factory method is given as a lambda function.
In general, the factory method should create a new instance of the object and return it.

### Returning Objects

Once you do not need an object anymore, it must be returned to the pool so that other components can reuse it.
For instance, to return a GameObject `myGameObject`, call the following method:

```[C#]
ObjectPool<GameObject>.ReturnResource(myGameObject);
```

*Important*:
Before you return the object, you should reset its properties.
The state in which you return the object is the state in which will receive the object again from the pool.
No internal modification are performed by the pool.

Moreover, it is advisable to bring objects into a storage state.
For GameObjects, it makes sense to deactivate them so that they are not visible in the scene while they are unused.
This also means that you have to activate the GameObject again once you retrieve it.

### Clearing Pools

The content of object pools can be cleared using the `ClearPool` method.

### Working with Separate Sub-Pools for the Same Object Type

In some cases, it makes sense to leave objects initialized in a specific state.
For instance, it makes no sense to remove components from GameObjects which are already set up if the same piece of application logic retrieves the GameObject later on and re-adds the same components.
Therefore, you can register sub-pools.
The purpose of these sub-pools is that you can return objects in a specific state to this pool and can expect it to have this state once you request the object from this sub-pool again.
As the sub-pool is separate, other components still work on the main pool and will not receive these objects which are in a different state.

Registering these separate pools works in the following way:

```[C#]
int poolId = ObjectPool<GameObject>.CreateNewPool();
```

This method will return an id by which the sub-pool can be addressed.
By default, the main pool of an object type always has the id 0.

Once you have obtained the id of the sub-pool, you can use the `RequestResource` and `ReturnResource` methods and specify the id as the first argument.

```[C#]
GameObject fromSubPool = ObjectPool<GameObject>.RequestResource(poolId, () => {return new GameObject();});
ObjectPool<GameObject>.ReturnResource(poolId, fromSubPool);
```

You can also remove sub-pools by calling the `RemovePool` method.

## Test Scene
There is a test scenes which demonstrates the usage with meshes.
The test scene was used to make sure that the pool fixes the memory leak problem that meshes introduce.
If meshes are not destroyed implicitly, they are never collected by the garbage collector, meaning that creating new meshes will fill the memory over time.
The `ObjectPool<Mesh>` pool solves this problem because the meshes can be recycled.