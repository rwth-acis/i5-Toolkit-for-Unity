# App Console

![App Console](../resources/Logos/AppConsole.svg)

## Use Case

When deploying compiled applications to devices, the application may behave slightly different to the editor simulation.
For instance, erros could happen where a resource is not available on the device but is automatically available in the editor.
In such cases, it is difficult to debug the problem since the application has e.g. with UWP apps been converted to C++ sources using IL2CPP.
Since exceptions in Unity scripts stop the script silently, problems may not even come to attention immeditaly.
Therefore, the app console of the i5 Toolkit provides a way to monitor the log outputs and error logs of scripts in the deployed application.

## Usage



### Notes

The app console is meant as a development tool for debugging purposes.
It should only be used in development editions of applications and should not be included in the final application for production.
The console has a noticable performance impact each time a log message is received.
Therefore, reduce the log output to the necessary parts that need to be visible and avoid many logs in Update calls.

## Creating Own Consoles



## Example Scene
