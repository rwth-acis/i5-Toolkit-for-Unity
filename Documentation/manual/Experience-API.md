# Experience API

![Experience API](../resources/Logos/ExperienceAPI.svg)

## Use Case

xAPI is a data capturing solution for learning applications.
Clients can observe learner's actions and send them to the xAPI which stores it on a server in a Learning Record Store.
An example for a Learning Record Store is [Learning Locker](https://docs.learninglocker.net/welcome/).

Further information about xAPI can be found [here](https://xapi.com/).

## Usage

To access the xAPI conveniently, use a <xref:i5.Toolkit.Core.ExperienceAPI.ExperienceAPIClient> object.

### Client Setup

First, initialize the <xref:i5.Toolkit.Core.ExperienceAPI.ExperienceAPIClient> object.
Specify the <xref:System.Uri> of your xAPI.
The important part here is to make sure that the Uri actually points to the xAPI base endpoint instead of your general Uri.
So specify something like `https://lrs.example.org/xAPI`.

As the second initialization parameter that needs to be set before using the object, enter an authorization token.
The authorization token is generated when you register a new client in your learning record store's administration settings.

> Your authorization token is a secret, so it should not be stored in the source control.
> Instead, create an external configuration file that is excluded from source control and which is first read.
> Refer to the example scene to find an instance where a scriptable object was created to store the authorization token.

Optionally, you can also specify the version of xAPI that you are using.
By default, this is set to 1.0.3.

### Sending Statements

Place triggers in your code that will send statements to the xAPI under certain circumstances.
For instance, you can send a statement once the user views certain content.
There are three alternatives for sending statements.
In the end, they all create a <xref:i5.Toolkit.Core.ExperienceAPI.Statement>, consisting of an actor, a verb and an object.

1. You can construct a <xref:i5.Toolkit.Core.ExperienceAPI.Statement> object first and then hand it to the <xref:i5.Toolkit.Core.ExperienceAPI.ExperienceAPIClient.SendStatementAsync*> method.
2. You can create the <xref:i5.Toolkit.Core.ExperienceAPI.Actor>, <xref:i5.Toolkit.Core.ExperienceAPI.Verb> and <xref:i5.Toolkit.Core.ExperienceAPI.XApiObject> and pass them to the <xref:i5.Toolkit.Core.ExperienceAPI.ExperienceAPIClient.SendStatementAsync*> method.
   The method will internally construct a statement from this that is then sent to the Learning Record Store.
3. You can pass the IDs of the actor, verb and object directly to the <xref:i5.Toolkit.Core.ExperienceAPI.ExperienceAPIClient.SendStatementAsync*>.
   This will first create <xref:i5.Toolkit.Core.ExperienceAPI.Actor>, <xref:i5.Toolkit.Core.ExperienceAPI.Verb> and <xref:i5.Toolkit.Core.ExperienceAPI.XApiObject> objects with the corresponding IDs and after that, it will fill construct the statement and send it.

As a result, you get back the Learning Record Store's response message.
If the response was successful, you will receive the ID of the statement in the response's body.

Internally, the actor ID always needs to start with `mailto:`, followed by the mail address that identifies the actor.
The <xref:i5.Toolkit.Core.ExperienceAPI.Actor> class is able to recognize if the prefix was already added.
If the actor ID does not start with `mailto:`, this prefix is automatically added.

> In order to end up with a clean architecture, it is recommended to use the [observer pattern](https://en.wikipedia.org/wiki/Observer_pattern).
> Write a class that observes the values that could trigger an xAPI call.
> If the values change to a certain value, trigger the xAPI call using this observer class.
> This way, you do not create a dependency of your core functionality to an xAPI connection.

## Example Scene

There is an example scene that allows you to see how the <xref:i5.Toolkit.Core.ExperienceAPI.ExperienceAPIClient> is used.

### Required Setup

The example scene shows how to use the xAPI client.
To try the functionality in the scene, you first need to have access to a Learning Record Store.
In the Learning Record Store, create a new client and copy its authorization token.
After that, return to the sample scene in Unity.
Right click in the Assets Browser and choose "Create > i5 Toolkit > xAPI Client".
Select the created file and paste your authorization token into the corresponding field in the inspector panel.
After that, select the "Tester" GameObject in the example scene.
In the inspector, drag and drop the created file into the "Credentials" slot of the Experience API Tester component.

Adapt the xAPI-Endpoint on this component to fit to the URI of the Learning Record Store that you are using.

### Try out the Example

Run the example scene.
If you press F5, a statement is sent to your Learning Record Store.
It will contain the actor "tester@i5toolkit.com", a verb with the ID "http://www.example.org/test" and an object with the ID "http://www.example.org/xApiClient".
The console will also log the resulting answer by your Learning Record Store.