# App Console

![App Console](../resources/Logos/AppConsole.svg)

## Use Case

When deploying compiled applications to devices, the application may behave slightly different to the editor simulation.
For instance, erros could happen where a resource is not available on the device but is automatically available in the editor.
In such cases, it is difficult to debug the problem since the application has e.g. with UWP apps been converted to C++ sources using IL2CPP.
Since exceptions in Unity scripts stop the script silently, problems may not even come to attention immeditaly.
Therefore, the app console of the i5 Toolkit provides a way to monitor the log outputs and error logs of scripts in the deployed application.

## Usage

Under "i5 Toolkit for Unity/Runtime/App Console/Prefabs", there are pre-made consoles available.
Drag-and-drop a prefab into the scene and it will automatically work.

A console prefab has a console UI component (there are different variants of this component for different displays like TextMesh vs. TextMeshPro).
On this console UI, a checkbox *Capture In Background* is available.
If it is checked (default state), the console will still register logs even if it is deactivated.
In its unchecked state, it will unsubscribe from the application's log feed if it is deactivated.

### Notes

The app console is meant as a development tool for debugging purposes.
It should only be used in development editions of applications and should not be included in the final application for production.
The console has a noticable performance impact each time a log message is received.
Therefore, reduce the log output to the necessary parts that need to be visible and avoid many logs in Update calls.

## Creating Own Console Prefabs

To create own console prefabs, first construct the GameObjects.
The main thing which is required is a text display of some sort, e.g. a TextMesh or a TextMeshPro.
After that, create a UI component for the console.
The UI component must inherit from <xref:i5.Toolkit.Core.AppConsole.ConsoleUIBehaviour>.
This base class provides a member <xref:i5.Toolkit.Core.AppConsole.ConsoleUIBehaviour.consoleUI>.
Override the <xref:i5.Toolkit.Core.AppConsole.ConsoleUIBehaviour.Awake> method and initialize the text console UI member.
Example from the <xref:i5.Toolkit.Core.AppConsole.TextMeshProUGUIConsoleUI>:

```[C#]
[Tooltip("The text display which should show the messages")]
[SerializeField] private TextMeshProUGUI consoleTextDisplay;
[Tooltip("The formatter configuration which defines how messages are formatted")]
[SerializeField] protected LogFormatterConfiguration logFormatterConfiguration;

// initializes the text console UI
protected override void Awake()
{
    ITextDisplay textMeshProUGUIAdapter = new TextMeshProUGUITextAdapter(consoleTextDisplay);
    consoleUI = new TextConsoleUI(textMeshProUGUIAdapter, logFormatterConfiguration);

    base.Awake();
}
```

To initialize the console UI member, you need to provide a text display.
This is an adapter object that implements the <xref:i5.Toolkit.Core.Utilities.UnityAdapters.ITextDisplay> interface.
The toolkit already provides adapter objects for the following text display components which can be used:

- TextMesh: <xref:i5.Toolkit.Core.Utilities.UnityAdapters.TextMeshTextAdapter>
- TextMeshPro: <xref:i5.Toolkit.Core.Utilities.UnityAdapters.TextMeshProTextAdapter>
- TextMeshProUGUI: <xref:i5.Toolkit.Core.Utilities.UnityAdapters.TextMeshProUGUITextAdapter>

For other kinds of text displays, implement an own <xref:i5.Toolkit.Core.Utilities.UnityAdapters.ITextDisplay> adapter.

The <xref:i5.Toolkit.Core.AppConsole.ConsoleUIBehaviour.consoleUI> can also be initialized with a log formatter configuration object.

You can add additional functionality to the console UI, e.g. logic to show and hide the console or to add scrolling to the console's text.

## Example Scene
The package's examples contain a scene with a console prefab.
When executing the scene, press the the buttons F1 - F4 to generate log messages.
Each key prints a different type of log message:

- F1: Regular log that is generated using `Debug.Log`
- F2: Warning log that is generated using `Debug.LogWarning`
- F3: Error log that is generated using `Debug.LogError`
- F4: Exception log that is generated when an exception is thrown

If you press F5, you can toggle the visibility of the console.