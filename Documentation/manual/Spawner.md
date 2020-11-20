# Spawner

![Spawner](../resources/Logos/Spawner.svg)

The spawner is a configurable object that allows you to create copies of a pre-defined prefab in the scene.

## Setup

The spawner can be set up in Unity's inspector with the following properties:

**Prefab**: Add a prefab that the spawner should create in the scene.

**Spawn on Awake**: If this option is checked, a copy of the prefab is immediately created once the application starts.

**Destroy With Spawner**: If this option is checked and you destroy the spawner component, it will also destroy all created copies in the scene.

**Max Number of Instances**: You can limit the amount of copies which are created in the scene, e.g. for performance.
If you set this value to 0, no limit is set.

**Overwrite Existing Instances**: If you check this option and you have set a limit for the copies, creating new copies will overwrite old ones.
The overwriting logic uses FIFO, meaning that that the first copy that was created is also overwritten first.
If this option is not checked, trying to create new copies will not work.

## Usage

You can create a new copy in the scene by calling the method <xref:i5.Toolkit.Core.Spawners.Spawner.Spawn> on the spawner component.

Created copies are listed under the property <xref:i5.Toolkit.Core.Spawners.Spawner.SpawnedInstances>.
Moreover, the last spawned GameObject can be accessed using the property <xref:i5.Toolkit.Core.Spawners.Spawner.MostRecentlySpawnedObject>.

## Example Scene

The example scene contains a spawner object and a demo script which accesses the spawner.
If you press F5, a new instance of the given cylinder prefab is spawned.
You can try out the different settings of the spawner in this scene to find out how the spawner works in detail.