# i5 Debug

## Use Case

The <xref:i5.Toolkit.Core.Utilities.i5Debug> class provides pre-formatted log commands.
They provide more information about the origin of the log message by writing the class name in front of the log message.
If the class is a MonoBehaviour, it will also highlight the associated GameObject in the hierarchy if the log message is clicked in the console.

## Usage

Log messages can be created in a similar way to the standard debug logs in Unity:

| Unity Debug Log | i5 Debug Log |
| --- | --- |
| `Debug.Log("text")` | [`i5Debug.Log("text", this)`](xref:i5.Toolkit.Core.Utilities.i5Debug.Log*) |
| `Debug.LogWarning("text")` | [`i5Debug.LogWarning("text", this)`](xref:i5.Toolkit.Core.Utilities.i5Debug.LogWarning*) |
| `Debug.LogError("text")` | [`i5Debug.LogError("text", this)`](xref:i5.Toolkit.Core.Utilities.i5Debug.LogError*) |

## Functionality

The i5Debug is a formatter which takes additional information as input and formats them into a log output.
The output is logged using Unity's standard `Debug` class.
This also means that it can be used in combination with standard `Debug.Log` outputs.
