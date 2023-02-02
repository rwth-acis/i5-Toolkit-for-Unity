# Verbose Logging System

![Verbose Logging](../resources/Logos/AppConsole.svg)

## Use Case

Unity provides a `Debug.Log` output class, as well as `Debug.LogWarning` and `Debug.LogError`.
Developers can use these statements to create messages for the console in the Unity editor.
Moreover, on deployed Unity applications, the statements are collected in a log file.
So, if an error occurs, the log file can be sent to the developer to investigate what caused the error.
However, the log statements cause a performance loss as the application immediately writes them to the log file.
Moreover, depending on the use case, adding many debug logs might be more confusing than just posting the minimum necessary.

Hence, the i5 Toolkit provides its own logging system where the verbosity level can be configured.
This allows applications to log statements only if the level of detail is required.
Production-ready applications and development prototypes can have different verbosity levels configured.
Moreover, it is possible to change the verbosity level at runtime.
This means that the application can, e.g., provide a debug mode which the user can activate to increase the level of detail on the logging.
The log statements print their verbosity level so that generated log files can also be filtered.

## Usage

Applying the logging module consists of two parts:
First, the concept for generating messages and assigning them to a verbosity level is discussed and after that, filtering of messages is shown.

### Message Logging

There are five levels of verbosity.
Developers can state for each log message into which category the message belongs.

The following guide can be considered when deciding which category to assign to each method:

| Category | Description | Method |
| --- | --- | --- |
| Critical | A log message about a failure or error that is critical to the further execution of the program. For instance, a module is in an unforeseen state and going forward, it is not clear whether the application still functions correctly. | <xref:i5.Toolkit.Core.VerboseLogging.LogCritical*> |
| Error | A log message about an error which does not endanger the further execution of the program. For instance, the application tried loading a save file but could not parse it, so the loading operation was aborted. | <xref:i5.Toolkit.Core.VerboseLogging.LogError*> |
| Warning | A log message about a check which resulted in an unexpected value. This is not really an error yet but should be addressed. For instance, the application noticed that the player left the designated play aera and was reset. | <xref:i5.Toolkit.Core.VerboseLogging.LogWarning*> |
| Info | A log message that informs about what the application is roughly doing. However, the log messages only inform about major events.  For instance, the application logs that the user changed the graphics quality in the settings menu. | <xref:i5.Toolkit.Core.VerboseLogging.LogInfo*> |
| Debug | A log message which can be used for debugging the application and giving the developer additional hints about what is going on. It can, e.g., provides key values during calculations. For instance, a player buys an item and the log messages output the number of resources that the player has in the inventory before and after the transaction. | <xref:i5.Toolkit.Core.VerboseLogging.LogDebug*> |
| Trace | A log message which helps track exactly which code execution paths the application takes. It logs methods which are entered, which branch of an if-else condition was selected, etc. For instance, the application logs that it is trying to cast a raycast in the scene and whether it found anything. | <xref:i5.Toolkit.Core.VerboseLogging.LogTrace*> |

Exceptions can additionally be logged using the method <xref:i5.Toolkit.Core.VerboseLogging.LogException*>.
Exceptions are either logged using the `Cricial` or `Error` categories, depending on the value provided to the `isCritical` parameter.

The `AppLog.Log<Level>` methods are convenient shortcuts.
It is also possible to call the method <xref:i5.Toolkit.Core.VerboseLogging.Log*> and provide to it the log level.

Just like `Debug.Log*`, objects can be passed to `AppLog.Log*` to define the context of the statement.
Clicking a log message with a provided context in the Unity console will highlight the GameObject on which it occurred in the hierarchy.
Moreover, the statement will print additional information about the context object.

### Setting the Level of Verbosity

The level of verbosity can be set centrally by altering the property <xref:i5.Toolkit.Core.VerboseLogging.MinimumLogLevel>.
All messages which are on the given level or on a higher level are logged, while all other messages are suppressed.
For instance, if the minimum log level is set to `Warning`, the system will log messages that are on the levels `Critical`, `Error` and `Warning`.
If the minimum log level is set to `Trace`, every message of any category will be logged.

One way of using the level of verbosity is to set it once a startup script, e.g., depending on the version number, whether it is a development build or other factors.

The level of verbosity can also be dynamically changed during runtime.
So, an application can, e.g., start with a minimum log level of `Critical` and later change it to `Info` to get more detailed outputs for a specific section of the program.

Note that messages cannot be retrieved retroactively.
Messages below the minimum log level are never produced and so they cannot be restored later.
Changing the minimum log level during runtime applies to future log statements but not past ones.

The otehr way around, you can first generate more messages than needed and later filter a log file with a text program to include less messages by searching for the log level.
Since every message starts with its log level, all messages of a particular level can quickly be found both in the Unity editor and in log files.

### In-Editor Functionality

In the editor, the log messages can be colored according to the level.
By default, this feature is activated but it can be deactivated using the property <xref:i5.Toolkit.Core.VerboseLogging.UseColors>.
Moreover, the colors can be adjusted for each level individually which might be necessary depending on the Unity version and the chosen color scheme.

The coloring of messages is only applied in the Unity editor and neither the coloring nor the markup which produces the color are added to log files outside of the Unity editor.

## Adjusting an Existing Application to Use Verbose Logging

In order to use the verbose logging feature, all existing calls to `Debug.Log`, `Debug.LogWarning`, `Debug.LogError` and `Debug.LogException` have to be replaced.

Replace `Debug.Log` with either `AppLog.LogInfo`, `AppLog.LogDebug` or `AppLog.LogTrace` depending on the level of detail that the given log statement represents.

Replace `Debug.LogWarning` with `AppLog.LogWarning`.

Replace `Debug.LogError` with `AppLog.LogError` or `AppLog.LogCritical` depending on how critical the error is.

Finally, replace all calls to `Debug.LogException` with `AppLog.LogException`.
Fill the Boolean parameter `isCritical` with true or false, depending on whether you perceive the exception as of critical importance.

There is also no problem with using `Debug.Log*` calls in parallel to the `AppLog.Log*` calls.
Messages to `Debug.Log*` will always be logged, independent of the set verbosity level.
However, it is recommended to not mix the two to keep the code consistent.

Any errors which are logged directly by Unity as soon as they appear, will still be logged.
This is an intented behavior as these kinds of errors and exceptions are regarded as critical.

## Example Scene

The example scene generates messages of different verbosity levels.
In the script, the minimum log level can be adjusted to see how the set value affects which messages are logged and which ones are suppressed.