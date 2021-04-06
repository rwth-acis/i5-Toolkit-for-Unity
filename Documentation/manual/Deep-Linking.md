# Deep Linking

![Deep Linking](../resources/Logos/DeepLink.svg)

## Mobile Deep Links

Mobile deep links are special links on Web pages.
They allow users to seamlessly switch into an installed application and trigger an action there.
These deep links do not start with "http://" or "https://" but use a custom schema that can be freely chosen.
For instance, it is possible to register an app to react to all links that start with "i5://".
To ensure that the schema is unique, it is also possible to use reverse DNS notation, e.g. by writing "com.i5.Toolkit://".

For more information on mobile deep links can be found [here](https://en.wikipedia.org/wiki/Mobile_deep_linking).

## Use Case

The mobile deep links can be used to quickly access functionalities and services of installed applications.
As an example, it is possible to tell the app to immediately load certain content or open a specific editor.
A common deep link that can be found on the Web is the "mailto://" schema.
If a user clicks the link, the mail client opens and automatically sets up a new mail where the receiver mail is automatically filled based on the link's content.

## Usage

Follow these steps to integrate deep linking into your application:

1. Add a <xref:i5.Toolkit.Core.DeepLinkAPI.DeepLinkingService> to your application.
2. Add the <xref:i5.Toolkit.Core.DeepLinkAPI.DeepLinkAttribute> to a method that should react to a deep link.
   When specifying the attribute, set the path to which it should react, e.g. "myDeepLink" if it should react to deep links like "i5://myDeepLink".
   The path is case-insensitive.
3. Optionally, you can also enter a schema in the attribute's definition.
   If you do not add a schema, all schemas are recognized which have the same path.
   For instance, `[DeepLink("myDeepLink")]` will be activated by any URL with the path myDeepLink, e.g. "i5://myDeepLink" but also "rwth://myDeepLink", etc.
   If the schema is specified, only links which match this exact schema target the given method.
   So, `[DeepLink(schema: "i5", path: "myDeepLink")]` will only be called by the deep link "i5://myDeepLink" but e.g. not by "rwth://myDeepLink".
4. *Important*: To optimize performance, the <xref:i5.Toolkit.Core.DeepLinkAPI.DeepLinkingService> does not scan the entire code for the methods with attributes.
   Instead, you need to add the class that contains the method manually using <xref:i5.Toolkit.Core.DeepLinkAPI.DeepLinkingService.AddDeepLinkListener(System.Object)>.
5. To clean up, you can remove a listener class again using <xref:i5.Toolkit.Core.DeepLinkAPI.DeepLinkingService.RemoveDeepLinkListener(System.Object)>

### Recommendations

It is possible to mark any method in the code as a deep link receiver.
However, you should only choose methods that are available after the automatic initialization procedure of your application.
Moreover, the methods should stay available during the entire lifetime of the application.

This is due to the fact that the deep links should be state-free.
No matter where in the application you are, a received deep link should always have the same effect.
Moreover, when launching the application via a deep link, it should directly react to the deep link's content without user intervention.
It would be confusing for users if they start via a deep link, get into the normal main menu, can interact with it and e.g. only if they open the settings menu, the deep link is suddenly recognized and has an effect.

To keep the architecture clean, it is recommended to create few API-definition classes that bundle deep link paths instead of scattering them throughout the application's code.
These API-definition classes should be available from the beginning of the application and should persist until the application is terminated.

## Example Scene

