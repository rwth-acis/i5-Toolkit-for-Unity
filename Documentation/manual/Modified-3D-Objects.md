# Modified 3D Objects

![Rounded Corners](../resources/Logos/RoundedCorners.svg)

## Rounded Corners

The rounded corners object is an extruded rectangle with rounded corners.
It can be used for 3D UI elements, e.g. to create menus or buttons with rounded edges.

### Usage

To use the rounded corners object, instantiate the prefab located under "i5 Toolkit/Runtime/Modified 3D Objects/Prefabs/Rounded Corners.prefab" in the scene.
The prefab has a *Rounded Corners* component where options such as the size, depth and corner radius can be set.

| Property | Purpose |
| --- | --- |
| Width | Specifies the width of the object |
| Height | Specifies the height of the object |
| Depth | Specifies the depth/thickness of the object |
| Corner Radius | Specifies how large the rounded corners should be. The value is relative to the edge length and should be between 0 (no rounded edges) and 0.5 (elliptic shape). Values outside of this range are automatically clamped. It is not recommended to use the exact extreme values 0 and 0.5 since they lead to overlapping vertices. |
| Subdivisions | Specifies how many vertices make up the rounded corner. Smaller values lead to a more lightweight mesh and better performance but the corners will appear jagged rounded. Higher values smooth the curvature of the corner but create denser meshes with higher performance impact. Usually, a value of 3 should be sufficient. |
| Exact Colliders | If deactivated, a box collider is used to approximate the shape of this object. If you require exact collision detection on the rounded corners, activate this setting. It will replace the box collider with a mesh collider. However, mesh colliders have a higher performance impact, so activate this option only if necessary. |

Do not scale the object by its transform since this will stretch the corners.
Instead, use the width, height and depth settings since they preserve the circular corners.

### Test Scene

There is a test scene where the settings of the prefab are demonstrated.