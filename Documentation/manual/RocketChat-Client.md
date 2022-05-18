# Rocket.Chat Client

![Rocket.Chat Client](../resources/Logos/RocketChatClient.svg)

## Use Case

The Rocket.Chat client contains an integration with the chat program [Rocket.Chat](https://rocket.chat).
Using this module, the application can send or receive messages in Rocket.Chat at runtime.
For instance, this allows the integration of chatbots into Unity, e.g., to drive dialogues by non-player characters.
Moreover, this can be useful for sending notifications about actions that users perform in the application, thereby gaining awareness in the community.

## Usage

### Service Initialization

First, the <xref:i5.Toolkit.Core.RocketChatClient.RocketChatService> has to be added to the <xref:i5.Toolkit.Core.ServiceCore.ServiceManager> so that the service is available.
While creating the service, a host address needs to be provided.
This is the host address of your Rocket.Chat instance.
You can find your Rocket.Chat's address in the following way:

1. Open the Rocket.Chat client and select your server on the very left.
2. Underneath the list of channels and direct messages on the bottom left, there is an icon that can be customized for your server.
   Right-click on this icon and select "Copy link address" from the context menu.
3. Paste the resulting link somewhere and shorten it so that it ends with the country code.
   It should, e.g., not contain the `/home` part anymore.
   Moreover, remove the `https://` at the beginning.
   So, if your link read `https://my-chat.mydomain.com/home`, shorten it to `my-chat.mydomain.com`.
4. Pass the resulting link to the constructor for <xref:i5.Toolkit.Core.RocketChatClient.RocketChatService> in your source code.

### Login

#### Option 1: Personal Access Token / AuthToken

You can create a personal access token in your account and use this to log in.
This is recommended for additional security as you can create different tokens for separate applications and can also revoke tokens in case that a token was leaked.

To set up a personal access token, follow these steps:
1. Click on your profile image in the Rocket.Chat client.
2. After that, select "My Account".
3. There is a category "Personal Access Tokens".
   Click on it.
4. Enter any name in the text field.
   The content that you enter here is just a human-readable identifier so that you can differentiate your tokens.
   For instance, you can enter the name of the application which will use this token.
5. Click the "Add" button to generate the token.
6. A pop-up appears with the string of the token.
   We will need this token string in the login process.
   It also states your username's identifier but this is not required for the login as it is already encoded in the token.

With the generated token, call <xref:i5.Toolkit.Core.RocketChatClient.RocketChatService.LoginAsync(System.String)> and pass this token to the login.
Continue by [handling the result of this call](#login-result).

#### Option 2: User Credentials

Instead of creating a personal access token in Rocket.Chat's profile settings, it can be more intuitive for the user to log in using the username and password.
This is also supported by the module.
Construct text fields that let the user enter the credentials.
After that, call <xref:i5.Toolkit.Core.RocketChatClient.RocketChatService.LoginAsync(System.String, System.String)> and pass the specified username and password to it.
Continue in section [login result](#login-result) to check if the login was successful.

#### Login Result

Await the result of the query to `LoginAsync` using `bool success = await LoginAsync(...)`.
This requires adding the using statement `using i5.Toolkit.Core.Utilities.Async` to the class.
After that, success will be set to `true` if the server was able to successfully log the user in.

### Send/Post a Message

To send a message, you first need to [log in](#login).
After that, call the function <xref:i5.Toolkit.Core.RocketChatClient.RocketChatService.PostMessageAsync*>
Specify a target channel, group or person.
For this particular method, you can use the human-readable names of the channels or usernames.
For channels, start the id with a `#`, followed by the name of the channel, e.g., `#general`.
To address users in direct messages, start the id with an `@`, followed by the username of the receiver, e.g., `@myreceiver`.
Alternatively, you can also directly use the internal id strings of channels or users.
In this case, the prefix is not required anymore since the internal id already specified whether the target is a channel or a user.

Then, pass the message content to the method.
Await the result of the call.
It will give you feedback whether the sending operation was successful.

### List Public Channels of the Logged-In User

You can get all public channels that a user is part of by calling <xref:i5.Toolkit.Core.RocketChatClient.RocketChatService.GetChannelListJoinedAsync>.
This function is, e.g., helpful for getting the internal id of a particular channel.

### List Private Channels, Groups, Teams of the Logged-In User

You can get all private channels, groups or teams that a user is part of by calling <xref:i5.Toolkit.Core.RocketChatClient.RocketChatService.GetGroupListAsync>.
This function is, e.g., helpful for getting the internal id of a particular channel.

### Get Information about the Logged-In User

To fetch the properties of the logged-in account, call <xref:i5.Toolkit.Core.RocketChatClient.RocketChatService.GetMeAsync>.

### Receive Messages

To receive messages, the service needs to start listening for messages and your code needs to subscribe to the event to get notified about incoming messages.
In the end, do not forget to un-subscribe from the server.

#### Subscribe to a Room

So, first call <xref:i5.Toolkit.Core.RocketChatClient.RocketChatService.SubscribeRoomMessageAsync*>.
This method requires the id of the room (channel, direct messages, etc.) that you want to listen to.
This id has to be the internal id that you can find by listing the [public channels](#list-public-channels-of-the-logged-in-user) or [private groups](#list-private-channels-groups-teams-of-the-logged-in-user) of the user.
The human-readable name with the `#` or `@` at the beginning does not work.

For the other unique id that the method requires, you can specify any string.
It is used to distinguish different subscriptions.

#### Subscribe to Service Events

After that, subscribe your code to the event <xref:i5.Toolkit.Core.RocketChatClient.RocketChatService.OnMessageReceived>.
Every time that a new message appears in the monitored room, the event is invoked and you can extract information about the message from the given arguments.

#### Un-Subscribe

When starting the subscription to the server, the service builds up a stream that is kept open.
Once the application does not need to listen for incoming messages anymore, close this subscription.
Do this by calling <xref:i5.Toolkit.Core.RocketChatClient.RocketChatService.UnsubscribeRoomMessageAsync*>.
Provide the same unique id that identifies the subscription to indicate which active subscription should be closed.

To clean everything up, also un-subscribe your code from the event <xref:i5.Toolkit.Core.RocketChatClient.RocketChatService.OnMessageReceived>

## Example Scene

The examples contain a scene where the functionalities of the module can be tried out.
It displays a canvas where you can log in, list public and private channels, send messages or receive messages.
The results of the calls can be seen in Unity's log output console.