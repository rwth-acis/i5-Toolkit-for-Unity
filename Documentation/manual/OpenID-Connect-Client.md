# OpenID Connect Client

![OpenID Connect Client](../resources/Logos/OpenIDConnectClient.svg)

## OpenID Connect

OpenID Connect is a single sign-on solution for user authentication.
Users can log in at a central login page that is provided by the OpenID Connect provider, e.g. Google or Learning Layers.
As a result of the successful login, the application receives an access token that can be used to request protected resources or data about the logged in user.

For users, OpenID Connect has the advantage that they only need one account at the OpenID Connect provider.
This account can be reused in all applications that support the provider's OpenID Connect sign-in.
Moreover, users do not have to trust that clients protect their login credentials and developers do not have to administer such credentials.
Critical information such as the user name and password are always entered at the central login page of the OpenID Connect provider.
This also means that the client never sees the user's credentials.
Instead, the client only receives an access token after the login.
Such an access token is only valid for a limited amount of time.
Moreover, the user has to agree to *scopes* which list the kind of information that the application is allowed to access.
The user can revoke these access rights at any time.

## Supported Platforms

The OpenID Connect client currently works on the following platforms:

- Unity editor (for testing)
- Standalone builds
- UWP (IL2CPP only; scripting runtime version must be set to ".NET 4.x Equivalent")
- Android
- iOS

> There is a Unity bug in Unity 2020 for UWP platforms that crashes the application once it opens a Web page.
> This means that the OpenID Connect client cannot be used when building an UWP app using Unity 2020.
> The bug does not occur in Unity 2019.4 and should be fixed in Unity 2021.1.
> For more information, check the corresponding [bug report](https://issuetracker.unity3d.com/issues/uwp-crash-with-exception-thrown-at-0x00007fffc01d7984-unityplayer-dot-dll-when-application-dot-openurl-is-called).

## Usage

### Service Initialization

Register a <xref:i5.Toolkit.Core.OpenIDConnectClient.OpenIDConnectService> at the <xref:i5.Toolkit.Core.ServiceCore.ServiceManager>.
This can e.g. be done using a [bootstrapper](Service-Core.md#bootstrappers).
When creating the service, make sure that its <xref:i5.Toolkit.Core.OpenIDConnectClient.OpenIDConnectService.OidcProvider> is set up.

Here is an example bootstrapper:
```[C#]
[SerializeField]
private ClientDataObject learningLayersClientData;

protected override void RegisterServices()
{
    OpenIDConnectService oidc = new OpenIDConnectService();
    oidc.OidcProvider = new LearningLayersOidcProvider();
    oidc.OidcProvider.ClientData = learningLayersClientData.clientData;
    // this example shows how the service can be used on an app for multiple platforms
#if !UNITY_EDITOR
    oidc.RedirectURI = "i5:/";
#else
    oidc.RedirectURI = "https://www.google.com";
#endif
    ServiceManager.RegisterService(oidc);
}
```

In this example, the <xref:i5.Toolkit.Core.OpenIDConnectClient.OpenIDConnectService.OidcProvider> is set up, e.g. using the <xref:i5.Toolkit.Core.OpenIDConnectClient.LearningLayersOidcProvider>.
To set it up, a redirect page is defined.
Note, that different redirect pages can be defined for different platforms.
This also has the advantage that different redirect methods can be used on these platforms.
In this example, Windows Store Apps use the custom URL schema "i5:/" whereas all other platforms redirect to Google.

### Switching between OpenID Connect Providers

The <xref:i5.Toolkit.Core.OpenIDConnectClient.OpenIDConnectService.OidcProvider> is an implementation that specifies how the authentication API of a provider should be used.
Currently, the toolkit has built-in support for the following OpenID Connect providers:

| Platform | Implementation |
| --- | --- |
[Learning Layers](https://api.learning-layers.eu/o/oauth2/) | <xref:i5.Toolkit.Core.OpenIDConnectClient.LearningLayersOidcProvider> |
[GitHub](https://docs.github.com/en/github/authenticating-to-github/connecting-with-third-party-applications) | <xref:i5.Toolkit.Core.OpenIDConnectClient.GitHubOidcProvider> |

You can add support for further OpenID Connect providers by creating a class that implements the <xref:i5.Toolkit.Core.OpenIDConnectClient.OpenIDConnectService.OidcProvider> interface.
The class has to define how to access the different API endpoints of the provider to retrieve information such as the access token.

In the example in the previous section, we assigned the <xref:i5.Toolkit.Core.OpenIDConnectClient.OpenIDConnectService.OidcProvider> during the initialization phase.
However, it is also possible to set this property just before calling the login function, e.g. to give the user a choice between different providers that are switched on the fly.
Each <xref:i5.Toolkit.Core.OpenIDConnectClient.OpenIDConnectService.OidcProvider> has to be initialized with their own client credentials before using the login procedure.
For parallel support of multiple providers, read the section about [Using Multiple Providers in Parallel](#using-multiple-providers-in-parallel)

### Adding the Client Credentials

An application can only use a provider's OpenID Connect API, if you have registered a client at the API.
This is a manual step that has to be done once at the beginning of the development.
During the registration process, the provider will generate a client ID and a client secret.
To access the API's methods, the client has to include these credentials in the requests.

You can find instructions on how to register clients in the section [Client Registration](#client-registration)

**Protect your client data**. As the name suggests, your client secret (and other client data as well) should be kept confidential.
Therefore, they should be excluded if the project's source code is uploaded to public places such as GitHub.
To solve this, the OpenID Connect Service can load client credentials from a separate file in the project.
To define the client credentials, right-click somewhere in your Assets folder or any subfolder and choose *"Create > i5 Toolkit > OpenID Connect Client Data* to create the file.
If you select it, you can enter the client credentials in Unity's inspector.
If you are using Git, you can now add the created file to *.gitignore* so that it will not be uploaded.
In the setup instructions of your project, include a note that each developer has to create their own client credentials and add them to the file.

Before calling the login procedure, assign the client data in the following way.
You can either do this in the service bootstrapper as part of the general initialization or you can do this on the fly, just before accessing the login flow.

```[C#]
// expose a field in the inspector for your client credentials
[SerializeField]
private ClientDataObject clientDataObject;
// assign the client data
ServiceManager.GetService<OpenIDConnectService>().OidcProvider.ClientData = clientDataObject.clientData;
```

### Defining the Redirect URI

Before the login page is shown, a redirect URI should be specified by setting the <xref:i5.Toolkit.Core.OpenIDConnectClient.OpenIDConnectService.RedirectURI> property of the service.
The importance of this redirect URI varies based on the target platform.

#### Editor & Standalone

> Setting the redirect URI for editor and standalone builds is optional.
> You can do it to improve the user experience.

In the editor and standalone builds, the toolkit starts an internal server to which the login automatically redirects.
This way, the server always fetches the necessary data which are provided in the redirect.
After that, the user is redirected to the specified URI which can e.g. point to your Web page that tells the user to return to the application.
So, for editor and standalone builds, setting the <xref:i5.Toolkit.Core.OpenIDConnectClient.OpenIDConnectService.RedirectURI> property is optional and can be used to improve the user's experience.

Optional: If you require a workflow where the OpenID Connect redirect has to go to a fixed URL, you can manually set the <xref:i5.Toolkit.Core.OpenIDConnectClient.RedirectServerListener.ListeningUri> to the loopback address and the fixed port:

```
ServiceManager.GetService<OpenIDConnectService>().ServerListener.ListeningUri = "http://127.0.0.1:8080";
```

#### Native Apps (UWP, Android, iOS)

> Native app builds require a redirect URI which needs to be set to a custom URI schema.
> The custom URI schema must be registered in the provider's client configuration and in Unity's player settings.

In native app builds, the redirect URI is essential.
Set it to a custom URI scheme, e.g. `i5:/`.
You can use either an empty path, so `i5:/` or the path "login" as in `i5:/login`.

More information how to register a deep link can be found on the documentation page of the [Deep Linking Service](Deep-Linking.md#registering-deep-links).
You do not need to set up the service as this is already handled internally by the <xref:i5.Toolkit.Core.OpenIDConnectClient.OpenIDConnectService>.
You only need to register the deep link in the Player Settings or in the *AndroidManifest.xml* on Android.

### Starting the Login Process

First, make sure that an OpenID Connect provider has been set.
The <xref:i5.Toolkit.Core.OpenIDConnectClient.IOidcProvider> has to be configured with the client credentials.
One way to initialize it is to attach a <xref:UnityEngine.MonoBehaviour> script to the button UI object in the scene that triggers the login.
The script has a public field where you can reference the client credentials.
Once the button is clicked, first create an instance of the <xref:i5.Toolkit.Core.OpenIDConnectClient.IOidcProvider> that should be used and assign the client credentials.
After that, assign the instance to the xref:i5.Toolkit.Core.OpenIDConnectClient.IOidcProvider> property of the service.

Before starting the login process, subscribe to the <xref:i5.Toolkit.Core.OpenIDConnectClient.OpenIDConnectService.LoginCompleted> event to get notified once the login procedure is completed.
To start the login process, call the <xref:i5.Toolkit.Core.OpenIDConnectClient.OpenIDConnectService.OpenLoginPage> method of the <xref:i5.Toolkit.Core.OpenIDConnectClient.OpenIDConnectService>.
This will show the login page of the provider to the user in the system's default browser.
Once the user is logged in, all the necessary redirects and requests to get the access token are made automatically.
After the login process has finished successfully, the <xref:i5.Toolkit.Core.OpenIDConnectClient.OpenIDConnectService.LoginCompleted> event is raised.


### Bringing the Application Back into Focus

After the login, the Web browser redirects to a URI to which the app listens and which is defined in the <xref:i5.Toolkit.Core.OpenIDConnectClient.OpenIDConnectService.RedirectURI>.
This is usually a custom app protocol or the loopback address where a local server is listening.
However, in some cases, the application has to make sure that it gets back into focus after the login.

#### Native Apps (UWP, Android, iOS)

On native apps on UWP, Android or iOS, the application is automatically brought back into focus if you have specified the custom URI schema as the service's <xref:i5.Toolkit.Core.OpenIDConnectClient.OpenIDConnectService.RedirectURI> and if you have added it as a protocol in the player settings.
Using a custom URI schema as a redirect URI is mandatory for this platform since the redirect contains the necessary data to finish the login.

#### Editor & Standalone

For these platforms, the OpenID Connect service only handles the redirect of information.
Without additional logic or configurations, the user manually has to return to the application.
You can set the <xref:i5.Toolkit.Core.OpenIDConnectClient.OpenIDConnectService.RedirectURI> to a Web page which should tell the user to return to the app.
You can also search for more platform-specific approaches that get the application back into view.

### Logout

To log out, call the <xref:i5.Toolkit.Core.OpenIDConnectClient.OpenIDConnectService.Logout> method of the OpenID Connect service.
There is also an event <xref:i5.Toolkit.Core.OpenIDConnectClient.OpenIDConnectService.LogoutCompleted> which is raised after the logout.

### Using Multiple Providers in Parallel

This section describes how to provide parallel logins that allow your user to be logged in at multiple providers in parallel, e.g. at Learning Layers and GitHub.
The initial problem is that there is only one <xref:i5.Toolkit.Core.OpenIDConnectClient.OpenIDConnectService> which only provides one <xref:i5.Toolkit.Core.OpenIDConnectClient.OpenIDConnectService.OidcProvider>.
The <i5.Toolkit.Core.ServiceCore.ServiceManager> also only allows registering one instance of each class.

Hence, the solution is to create a new class for each provider that you have and let it inherit from the <xref:i5.Toolkit.Core.OpenIDConnectClient.OpenIDConnectService>.
This way, you can register e.g. a `LearnignLayersOIDCService` and a `GitHubOIDCService` at the same time.

```[C#]
public class LearningLayersOidcService : OpenIDConnectService
{
}
```

Initialize the new service classes like usual and after that, you are able to access their login data and login workflows independently.

### Using Multiple Providers as a Selection Option

If you do not need a user to be logged in parallel but they need to choose between one of the given provider options, this solution also works.
Alternatively, you can switch out the <xref:i5.Toolkit.Core.OpenIDConnectClient.OpenIDConnectService.OidcProvider> since you do not need to store multiple access tokens in parallel.

## Client Registration

To set up an OpenID Connect client for a specific provider, the client needs to be registered at the provider.
As a result, the provider issues client credentials which usually consist of a client id and a client secret.
Once you have obtained the client credentials, [add them to the client](#adding-the-client-credentials).

### Creating a Learning Layers Client

To register a learning layers client, contact an administrator for the Learning Layers login.
If you are working on a thesis at the Chair of Computer Science 5, RWTH Aachen University, you can contact your advisor.
Send the admin/advisor the following JSON string.
Fill out the client ID, the name, the description and the redirectUris.
For the client ID, choose a unique name for your app.
The admin/advisor will answer with a mail that contains the client ID and client secret that you can then use for the login process.

```
{
    "clientId": "An ID for your client",
    "name": "Your app name",
    "description": "A description of your app",
    "baseUrl": "",
    "surrogateAuthRequired": false,
    "enabled": true,
    "alwaysDisplayInConsole": false,
    "clientAuthenticatorType": "client-secret",
    "redirectUris": [
        "http://127.0.0.1:*",
        "myScheme:/"
    ],
    "webOrigins": [
        "http://127.0.0.1"
    ],
    "notBefore": 0,
    "bearerOnly": false,
    "consentRequired": false,
    "standardFlowEnabled": true,
    "implicitFlowEnabled": false,
    "directAccessGrantsEnabled": false,
    "serviceAccountsEnabled": false,
    "publicClient": false,
    "frontchannelLogout": false,
    "protocol": "openid-connect",
    "attributes": {
        "saml.assertion.signature": "false",
        "saml.force.post.binding": "false",
        "saml.multivalued.roles": "false",
        "saml.encrypt": "false",
        "backchannel.logout.revoke.offline.tokens": "false",
        "saml.server.signature": "false",
        "saml.server.signature.keyinfo.ext": "false",
        "exclude.session.state.from.auth.response": "false",
        "backchannel.logout.session.required": "true",
        "client_credentials.use_refresh_token": "false",
        "saml_force_name_id_format": "false",
        "saml.client.signature": "false",
        "tls.client.certificate.bound.access.tokens": "false",
        "saml.authnstatement": "false",
        "display.on.consent.screen": "false",
        "saml.onetimeuse.condition": "false"
    },
    "authenticationFlowBindingOverrides": {},
    "fullScopeAllowed": true,
    "nodeReRegistrationTimeout": -1,
    "defaultClientScopes": [
        "web-origins",
        "role_list",
        "roles",
        "profile",
        "email"
    ],
    "optionalClientScopes": [
        "address",
        "phone",
        "offline_access",
        "microprofile-jwt"
    ],
    "access": {
        "view": true,
        "configure": true,
        "manage": true
    }
}
```

### Creating a GitHub client

To register a GitHub client, follow the steps in the [official documentation](https://docs.github.com/en/developers/apps/creating-an-oauth-app) to create a new OAuth app.

You can register a new app [here](https://github.com/settings/applications/new).

The important part is to add an "Authorization callback URL" which is the redirect URI.
To use deep linking, enter the protocol to which your app is registered, e.g. `i5://`.
To use the local server, e.g. for in-editor testing, enter `http://127.0.0.1`.

> Note that you cannot register multiple redirect URIs in GitHub apps, e.g. for cross-platform.
> To solve this, create different apps for each redirect URI.
> After that, initialize the <xref:i5.Toolkit.Core.OpenIDConnectClient.OpenIDConnectService.OidcProvider>'s <xref:i5.Toolkit.Core.OpenIDConnectClient.IOidcProvider.ClientData> with the data of the app with the corresponding redirect URI.

## Example Scenes

The OpenID Connect example contains multiple subfolders and scenes for different examples.

### Learning Layers Example

There is an example scene which shows how to set up and use the Learning Layers OpenID Connect client.
To use the scene, first register a client at the [Learning Layers provider](https://api.learning-layers.eu/o/oauth2/).
After that, create the client credentials file by right-clicking in the Assets browser and choosing "Create > i5 Toolkit > OpenID Connect Client Data".
Select the created file and enter the client id and client data in the inspector.
Then, select the "Service Bootstrapper" GameObject in the scene and drag and drop the file onto the field on the Learning Layers Bootstrapper.
After this initialization, you can start the scene.
If you press F5, the browser is opened with the Learning Layers login page.
Once you log in and return to the app, the console will print the access token and some information about the logged in user.

The important GameObjects in the example scene are the *Service Bootstrapper* and *Tester*.
The service manager bootstrapper on the *Service Bootstrapper* initializes the <xref:i5.Toolkit.Core.OpenIDConnectClient.OpenIDConnectService> and adds the provider's client data.
The *Tester* GameObject triggers the login procedure and reacts to the successful login.

### GitHub Example

The GitHub folder contains an example scene for trying out the login at GitHub.
First, [register an OAuth Client at GitHub](https://github.com/settings/applications/new).
To do so, follow the instructions in the section [Creating a GitHub client](#creating-a-github-client).
If you only want to test the demo in the editor, it suffices to create the client with the "Authorization callback URL" `http://127.0.0.1`.
Create the second OAuth client with your deep link protocol if you want to deploy the example scene to an app.

Once the client credentials are created, right click in Unity's Asset browser and choose "Create > i5 Toolkit > OpenID Connect Client Data".
After that, enter the client credentials that you just created.
If you created two clients, you need two OpenID Connect Client data files.

After that, select the "Service Bootstrapper" object in the example scene and assign the client data files in its bootstrapper component.
This can be done by dragging and dropping the client data file from the Asset browser into the component's field in the inspector.

With this setup, you can start the scene or build the application.
Press F5 to trigger the login procedure.

### Multiple Providers in Parallel

The example scene in the folder "Multiple Providers in Parallel" shows a possible app setup where a user can be logged in a multiple providers at the same time.
Here, Learning Layers and GitHub can be used in parallel.
To set up this example, create the client credentials for each of the providers.
Assign them to the bootstrapper fields on the "Service Bootstrapper" GameObject.
After that, you can press F1 to open the Learning Layers login.
F2 lets you verify that you are still logged in at Learning Layers and that the application memorized your access token.
Press F3 to log in at GitHub.
With F4, the application will log your GitHub username to show that it still knows that you are logged in.

Try out the following workflow: First log in at both Learning Layers and GitHub (F1 and F3) and after that, press F2 and F4 to verify that both providers exist in parallel and have indeed saved your access token.