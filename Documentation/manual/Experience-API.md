# Experience API

![Experience API](../resources/Logos/ExperienceAPI.svg)

## Use Case

xAPI is a data capturing solution for learning applications.
Clients can observe learner's actions and send them to the xAPI which stores it on a server in a Learning Record Store.
An example for a Learning Record Store is [Learning Locker](https://docs.learninglocker.net/welcome/).

Further information about xAPI can be found [here](https://xapi.com/).

## Usage

To access the xAPI conveniently, use an <xref:i5.Toolkit.Core.ExperienceAPI.ExperienceAPIClient> object.

### Standard & Advanced Mode

The xAPI module ships with a standard and an advanced mode.

#### Standard Mode

In the standard mode, no additional dependencies are required as the JSON serialization happens using Unity's build-in <xref:UnityEngine.JsonUtility>.
Here, limited statements that only make use of the actor, verb and object are allowed.
This is sufficient for statements like "Person X viewed Object Y".

#### Advanced Mode

In order to enable advanced features such as adding contexts, results etc. to the statement, you need to install a Newtonsoft JSON library.
It is recommended to use the [Unity package by jilleJr](https://github.com/jilleJr/Newtonsoft.Json-for-Unity).
If you import exactly this package, the module will automatically switch to advanced mode and use the library for JSON serialization.
In case you want to use another Newtonsoft JSON library, e.g. the [internal package by Unity](https://docs.unity3d.com/Packages/com.unity.nuget.newtonsoft-json@2.0/manual/index.html), you need to manually add the Scripting Define Symbol `NEWTONSOFT_JSON` in the Player Settings.
However, currently we recommend using the [Unity package by jilleJr](https://github.com/jilleJr/Newtonsoft.Json-for-Unity) for the advanced mode.

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

## xAPI Element Model
The library uses a custom implementation of the xAPI Standard data elements that can be found [here](https://github.com/adlnet/xAPI-Spec/blob/master/xAPI-Data.md#parttwo).
The fields in the defined classes correspond closely to the standard itself.
Each has its own ToJObject() method, which handles conversions to JSON.
The following elements are used for the implementation:

### Actor
The Actor defines who performed the action.
The class file <xref:i5.Toolkit.Core.ExperienceAPI.Actor> implements the Actor element.
It has two fields:
+ **<xref:i5.Toolkit.Core.ExperienceAPI.Actor.Mbox>** - Corresponds to the IRI of the Actor element, which in this implementation corresponds to the format "mailto:actor@email.com".
  The setter of this field makes sure that the field value starts with _"mailto:"_, so that it conforms with the standard.
  This field is required for a valid Actor element.
+ **<xref:i5.Toolkit.Core.ExperienceAPI.Actor.name>** - The name of the actor. Optional.

Additional steps when converting to a JObject (using the `ToJObject()` method):
+ The property **"objectType": "Agent"** is added.

### Verb
The Verb defines the action between an Actor and an Object.
The class file <xref:i5.Toolkit.Core.ExperienceAPI.Verb> implements the Verb element.
It has two fields:
+ **<xref:i5.Toolkit.Core.ExperienceAPI.Verb.id>** - Corresponds to the IRI of the Verb element, which must be of the format defined in the standard to be accepted by a LRS.
  The library does not validate conformity to the format. Required.
+ **<xref:i5.Toolkit.Core.ExperienceAPI.Verb.displayLanguageDictionary>** - An implementation of the "display" property in the standard.
  Holds <k,v> pairs where the key is the language code and the value is the name of the Verb in that language.
  Optional, the dictionary may be empty. When converting to JSON, a name will be added with the default "en-us" language code if none is provided (using the <xref:i5.Toolkit.Core.ExperienceAPI.Verb.CutToVerbName*> method).

It also has one notable method:
+ **<xref:i5.Toolkit.Core.ExperienceAPI.Verb.CutToVerbName*>** - This method can be used to extract the final part of a verb IRI, where the verb name is expected to be.
  For instance, from  'http://test.org/x/y/z/.../verbName', 'verbName' is retrieved.

Additional steps when converting to a JObject (using the `ToJObject()` method):
+ If the <xref:i5.Toolkit.Core.ExperienceAPI.Verb.displayLanguageDictionary> is empty, a name is extracted from the verb ID using the <xref:i5.Toolkit.Core.ExperienceAPI.Verb.CutToVerbName*> method.


### Object
The Object defines the thing that was acted on.
The class file <xref:i5.Toolkit.Core.ExperienceAPI.XApiObject> implements the Object element.
The name of the class is in this format as _object_ is a reserved name in C#.
The class implements the Object **only as an Activity** (objectType = Activity in the standard).
It has several fields:
+ **<xref:i5.Toolkit.Core.ExperienceAPI.XApiObject.id>** - Corresponds to the IRI of the Object element, which must be of the format defined in the standard to be accepted by a LRS.
  The library does not validate conformity to the format. Required.
+ **<xref:i5.Toolkit.Core.ExperienceAPI.XApiObject.nameDisplay>** - Dictionary that implements the _name_ language map from the standard.
  Holds <k,v> pairs where the key is the language code and the value is the name of the Object in that language.
  Optional, the dictionary may be empty.
+ **<xref:i5.Toolkit.Core.ExperienceAPI.XApiObject.descriptionDisplay>** - Dictionary that implements the _description_ language map from the standard.
  Holds <k,v> pairs where the key is the language code and the value is the description of the Object in that language.
  Optional, the dictionary may be empty.
+ **<xref:i5.Toolkit.Core.ExperienceAPI.XApiObject.type>** - The type of the Activity. Optional.

Additional steps when converting to a JObject (using the `ToJObject()` method):
+ The **objectType** is set to _"Activity"_
+ Names and descriptions are only taken from the dictionaries if their values are valid, i.e. not empty strings ("").

### Result
An optional property that represents a measured outcome related to the Statement in which it is included.
The class file <xref:i5.Toolkit.Core.ExperienceAPI.Result> implements the Result element.
It has the following fields, all of which are optional:
+ **<xref:i5.Toolkit.Core.ExperienceAPI.Result.success>** - Indicates whether or not the attempt on the Activity was successful.
+ **<xref:i5.Toolkit.Core.ExperienceAPI.Result.completion>** - Indicates whether or not the Activity was completed.
+ **<xref:i5.Toolkit.Core.ExperienceAPI.Result.response>** - A string message appropriately formatted for the given Activity.
+ **<xref:i5.Toolkit.Core.ExperienceAPI.Result.duration>** - Period of time over which the Statement occurred. Must be ISO 8601 Duration compatible.
+ **<xref:i5.Toolkit.Core.ExperienceAPI.Result.extensions>** - Implementation of the Result extensions as defined in the standard.
  Holds <k,v> pairs where the key is the extension key (must be a IRI) and the value is any meaningful string.
  Currently only strings as values for these extensions are supported.

It also has one notable method:
+ **<xref:i5.Toolkit.Core.ExperienceAPI.Result.AddMeasurementAttempt*>** - This method can be used if a measurement attempt wants to be recorded, that is to record a value that the user measured in some way.
  This is realized through the _extensions_ dictionary, which is why an IRI is needed besides the measurement value, both of which must be strings.

### Context
An optional property that provides a place to add contextual information to a Statement.
Currently, the library only supports adding parent activities as Context to the Statement activity.
The class file <xref:i5.Toolkit.Core.ExperienceAPI.Context> implements the Context element.
It has one field:
+ **<xref:i5.Toolkit.Core.ExperienceAPI.Context.ParentActivityIDs>** - A list holding the IDs of activities that serve as context-parent to this Statement.
  There is usually just one parent activity, but the standard allows for more. Optional.

### Statement
The complete xAPI Statement.
It is implemented by the <xref:i5.Toolkit.Core.ExperienceAPI.Statement> class file.
The following fields are defined:
+ **<xref:i5.Toolkit.Core.ExperienceAPI.Statement.actor>** - The Actor of the Statement. The type _Actor_ is defined above. Required.
+ **<xref:i5.Toolkit.Core.ExperienceAPI.Statement.verb>** - The Verb of the Statement. The type _Verb_ is defined above. Required.
+ **<xref:i5.Toolkit.Core.ExperienceAPI.Statement.object>** - The Object of the Statement. The type _XApiObject_ is defined above. Required.
+ **<xref:i5.Toolkit.Core.ExperienceAPI.Statement.result>** - The Result of the Statement. The type _Result_ is defined above. Optional.
+ **<xref:i5.Toolkit.Core.ExperienceAPI.Statement.context>** - The Context of the Statement. The type _Context_ is defined above. Optional.
+ **<xref:i5.Toolkit.Core.ExperienceAPI.Statement.timestamp>** - Defines when the experience occurred.

When constructing a statement either complete objects of Actor, Verb, and Object or their respective IRIs can be used.

Additional steps when converting to a JObject (using the `ToJObject()` method):
+ The timestamp is formatted to the required ISO 8601 format.

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