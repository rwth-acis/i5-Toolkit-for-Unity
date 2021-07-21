# Object Pool

![Object Pool](../resources/Logos/ObjectPool.svg)

## Purpose

**The problem**:
Usually, objects like [GameObjects](xref:UnityEngine.GameObject) and [Meshes](xref:UnityEngine.Mesh) are instantiated when they are needed so that they can be used in the scene.
If the object is not required anymore, it is usually destroyed.
Such calls of Unity's <xref:UnityEngine.Object#Instantiate> and <xref:UnityEngine.Object#Destroy> have a performance overhead which manifests in frame drops.
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

<xref:i5.Toolkit.Core.Utilities.ObjectPool`1.RequestResource*> works in the following way:
It tries to fetch an object from the pool and returns it.
If the pool is empty, the method falls back to the factory method which is provided as an argument.
In the example, the factory method is given as a lambda function.
In general, the factory method should create a new instance of the object and return it.

### Returning Objects

Once you do not need an object anymore, it must be returned to the pool so that other components can reuse it.
For instance, to return a GameObject `myGameObject`, call the following method:

```[C#]
ObjectPool<GameObject>.ReleaseResource(myGameObject);
```

*Important*:
Before you return the object using <xref:i5.Toolkit.Core.Utilities.ObjectPool`1.ReleaseResource*>, you should reset its properties.
The state in which you return the object is the state in which will receive the object again from the pool.
No internal modification is performed by the pool.

Moreover, it is advisable to bring objects into a storage state.
For GameObjects, it makes sense to deactivate them so that they are not visible in the scene while they are unused.
This also means that you have to activate the GameObject again once you retrieve it.

### Clearing Pools

The content of object pools can be cleared using the <xref:i5.Toolkit.Core.Utilities.ObjectPool`1.ClearPool*> method.
You can specify a method that takes the object as input and will clean them up.
For instance, you can specify <xref:UnityEngine.Object#Destroy> to destroy GameObjects in the pool.

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

The method <xref:i5.Toolkit.Core.Utilities.ObjectPool`1.CreateNewPool> will return an id by which the sub-pool can be addressed.
By default, the main pool of an object type always has the id 0.

Once you have obtained the id of the sub-pool, you can use the <xref:i5.Toolkit.Core.Utilities.ObjectPool`1.RequestResource*> and <xref:i5.Toolkit.Core.Utilities.ObjectPool`1.ReleaseResource*> methods and specify the id as the first argument.

```[C#]
GameObject fromSubPool = ObjectPool<GameObject>.RequestResource(poolId, () => {return new GameObject();});
ObjectPool<GameObject>.ReleaseResource(poolId, fromSubPool);
```

You can also remove sub-pools by calling the <xref:i5.Toolkit.Core.Utilities.ObjectPool`1.RemovePool*> method.
Similar to the <xref:i5.Toolkit.Core.Utilities.ObjectPool`1.ClearPool*> method, you can specify a function to destroy the objects that are currently in the pool.

### Administering the Pool/ In Which State Should Objects Enter the Pool?

How you or organize the pool(s) is up to you.
All objects in the same pool should be interchangeable since you will get back an arbitrary one.
You should also be clear about the condition in which you expect an object to be when it is dispensed from the pool.
The object is not modified within the pool.
So, before putting objects into the pool, reset them into a state in which they can immediately be used again when they are retrieved.
Moreover, the creation method of the <xref:i5.Toolkit.Core.Utilities.ObjectPool`1.RequestResource*> method must also provide new objects in the same state as recycled objects.

Imagine the following situation:
You have an application where you create and destroy a large amount of cube and sphere GameObjects.
The cubes are static and the spheres are affected by gravity using a Rigidboy component.
There are two ways how you could use pools to administer these two types of GameObjects.

1. Create two separate GameObject-pools.
   One of them only gives back the cubes and the other one only contains spheres.
   Here, the assumption is that pool 1 only contains the cubes and pool 2 only contains spheres that are already equipped with a Rigidbody component.
   The creation method that is supplied with <xref:i5.Toolkit.Core.Utilities.ObjectPool`1.RequestResource*> of pool 1 creates static cubes and the creation method for <xref:i5.Toolkit.Core.Utilities.ObjectPool`1.RequestResource*> of pool 2 creates spheres and initializes them with a Rigidbody.
   When returning the GameObjects to the pools, cubes only go into the first pool and spheres are only inserted into the second pool.
   Before returning a GameObject to the pool, check that it has a default scale, rotation and remove all components that were not on it when you retrieved it out of the pool.
   Your code should also check that the spheres still have a Rigidbody when they are inserted into the pool.

2. The second option is to use one pool for both GameObjects.
   Here, the cube and sphere GameObjects are reduced to a common denominator that is stored in the pool.
   Both the cubes and spheres have a Renderer component to display them in the scene.
   Hence, this means that you only put empty GameObjects into the pool that have a Renderer component on them.
   However, before inserting the GameObject, you should reset the renderer's mesh so that it does not render the cube or sphere shape anymore.
   Moreover, you should remove the Rigidbody component from the spheres.
   Similarly, the creation method in `RequestResource` returns an empty GameObject with a Renderer component on it.
   When retrieving a GameObject from the pool, you now need to do some setup work.
   For the cubes, add the cube mesh to the Renderer to define the shape.
   For the sphere, add a sphere mesh and the Rigidbody component to the retrieved GameObject.

Both options have their pros and cons.
The separated pools are easier to administer and it is clearer which kind of content a pool contains.
However, objects in a pool are very specific and potentially not be reused often in the program.
For one common pool, there is a higher reusability as the stored object is more general.
However, more cleanup and setup work is required when returning objects into the pool and retrieving them again.

> The object is not modified within the pool.
> In particular, GameObjects that are stored within the pool and should not appear in the scene, need to be deactivated *before* adding them to the pool.
> They must then be activated again *after* retrieving them from the pool.

> In your project, you should document how you use the pools.
> This means you should write down whether you use multiple different pools and in which state you assume objects to be in each pool.

## Test Scene
There is a test scene which demonstrates the usage with meshes.
The test scene was used to make sure that the pool fixes the memory leak problem that meshes introduce.
If meshes are not destroyed implicitly, they are never collected by the garbage collector, meaning that creating new meshes will fill the memory over time.
The `ObjectPool<Mesh>` pool solves this problem because the meshes can be recycled.