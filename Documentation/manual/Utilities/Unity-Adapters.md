# Unity Adapters

Unity adapters are a growing collection of interfaces and adapter classes to hide the Unity API and provide an abstraction layer on which the application logic can be implemented.
Each of the interfaces defines a contract, e.g. for a text display, which defines what an object can do.
This way, application scripts can work with the interfaces and do not need to worry about the underlying functionality.
The Unity API is wrapped into adapter objects which implement the required interfaces.

## Use Case

Unity adapters can be used to decouple application logic from the presentation in the scene.
For instance, this allows a script to display text on an <xref:i5.Toolkit.Core.Utilities.UnityAdapters.ITextDisplay>.
This interface allows the script to set text but it does not need to know about the implementation of the text display.
By initializing the <xref:i5.Toolkit.Core.Utilities.UnityAdapters.ITextDisplay> object with an adapter, the same application logic can work with different UI implementations, e.g. TextMesh or TextMeshPro.

## Interfaces

The toolkit provides the following interfaces:

| Interface | Provided funtionality |
| --- | --- |
| <xref:i5.Toolkit.Core.Utilities.UnityAdapters.IActivateable> | Activate and deactivate an object |
| <xref:i5.Toolkit.Core.Utilities.UnityAdapters.IRectangle> | Rectangular 2D (UI) element |
| <xref:i5.Toolkit.Core.Utilities.UnityAdapters.IScrollView> | Scrollable view |
| <xref:i5.Toolkit.Core.Utilities.UnityAdapters.ITextDisplay> | UI display to show text to show text | to the user

## Adapters

The toolkit provides adapters for the following Unity objects:

| Unity Class | Adapter Class |
| --- | --- |
| <xref:UnityEngine.GameObject> | <xref:i5.Toolkit.Core.Utilities.UnityAdapters.GameObjectAdapter> |
| <xref:UnityEngine.RectTransform> | <xref:i5.Toolkit.Core.Utilities.UnityAdapters.RectTransformAdapter> |
| ScrollRect | <xref:i5.Toolkit.Core.Utilities.UnityAdapters.ScrollRectAdapter> |
| <xref:UnityEngine.TextMesh> | <xref:i5.Toolkit.Core.Utilities.UnityAdapters.TextMeshTextAdapter> |
| TextMeshPro | <xref:i5.Toolkit.Core.Utilities.UnityAdapters.TextMeshProTextAdapter> |
| TextMeshProUGUI | <xref:i5.Toolkit.Core.Utilities.UnityAdapters.TextMeshProUGUITextAdapter> |

## Example

An example can be found in the source code of the [App Console's](../App-Console.md) UI.
It works with a general text display interface which allows the realizaion of consoles with TextMeshes or TextMeshPro.