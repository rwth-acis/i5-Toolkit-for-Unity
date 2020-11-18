# Scene Documentation Tools

![Scene Documentation](../resources/Logos/SceneDocumentation.svg)

The scene documentation tools provides the means to document and label your scenes.

## Usage

Create an empty GameObject and add the component *Documentation Object* to it.
The GameObject in the scene will show an "i" icon.
This icon is only visible in Unity's editor but not in playmode nor the final application.
You can move the GameObject in the scene to move the icon around and mark different parts of the scene.

On the documentation object, you can specify a title and a description that is stored on the component.
Moreover, you can specify a URL that points to a Web document that contains more information.
If you have specified a URL, you can click the *Open Documentation URL* button in the inspector to open the URL.

The documentation object's type can be changed.
There are a couple of different types that change the icon which is shown on the GameObject.

| Type | Icon | Usage |
| --- | --- | --- |
| INFO | ![Info Icon](https://raw.githubusercontent.com/rwth-acis/i5-Toolkit-for-Unity/develop/Assets/i5%20Toolkit/Editor/Scene%20Documentation/Textures/Info.png)  | Refer to documentation, describe what a GameObject does or leave a note in the scene. |
| TODO | ![Todo Icon](https://raw.githubusercontent.com/rwth-acis/i5-Toolkit-for-Unity/develop/Assets/i5%20Toolkit/Editor/Scene%20Documentation/Textures/Todo.png) | Mark a GameObject, component configuration or part of the scene as unfinished so that you or another developer can work on this aspect later. |
| BUG | ![Bug Icon](https://raw.githubusercontent.com/rwth-acis/i5-Toolkit-for-Unity/develop/Assets/i5%20Toolkit/Editor/Scene%20Documentation/Textures/Bug.png) | Mark a part of the scene as a bug, e.g. missing collision geometry or intersecting 3D objects. |

## Example Scene



## Remarks

The icons on the GameObject are realized as Gizmos.
Gizmos only work if they are placed in a folder *Assets/Gizmos* which needs to be at the root of the *Assets* folder.
When creating the package, it copies the icons from its resources into your project's Assets folder.
Do not delete them - they will re-appear again the next time you start Unity.