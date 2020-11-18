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
Moreover, the user has to agree to *scopes* which list the kind information that the application is allowed to access.
The user can revoke these access rights at any time.

## Supported Platforms

The OpenID Connect client currently works on the following platforms:

- Unity editor (for testing)
- Standalone builds
- UWP (IL2CPP only; scripting runtime version must be set to ".NET 4.x Equivalent")

## Usage

### Service Initialization

Register a `OpenIDConnectService` at the [service manager](Service-Core.md).
This can e.g. be done using a [bootstrapper](Service-Core.md#bootstrappers) by following these steps:

1. In the bootstrapper script, create a field of the type `OpenIDConnectServiceConfiguration` that is accessible in the inspector.
   Add the bootstrapper and the service manager to the same GameObject in the scene.
   In the inspector, the bootstrapper component will give you options to specify a redirect page.
2. In the `InitializeServiceManager()` method, create a new `OpenIDConnectService` object.
   You can pass the `OpenIDConnectServiceConfiguration` object in the inspector so that the service will use the given settings.
3. Before you can access the OpenID Connect workflow, you have to assign the service's `OidcProvier`.
   The `OidcProvider` is the specific implementation to access the API of a OpenID Connect provider, e.g. Learning Layers.
4. Finally, register the service instance at the service manager.

Here is an example bootstrapper:
```[C#]
public class LearningLayersBootstrapper : MonoBehaviour, IServiceManagerBootstrapper
{
    [SerializeField]
    private OpenIDConnectServiceConfiguration openIdConnectServiceConfiguration;

    public void InitializeServiceManager()
    {
        OpenIDConnectService oidc = new OpenIDConnectService(openIdConnectServiceConfiguration);
        oidc.OidcProvider = new LearningLayersOIDCProvider();
        ServiceManager.RegisterService(oidc);
    }
}
```

### Switching between OpenID Connect Providers

The `OidcProvider` is an implementation that specifies how the authentication API of a provider should be used.
Currently, the toolkit has built-in support for the following OpenID Connect providers:

- [Learning Layers](https://api.learning-layers.eu/o/oauth2/)

You can add support for further OpenID Connect providers by creating a class that implements the `IOidcProvider` interface.
The class has to define how to access the different API endpoints of the provider to retrieve information such as the access token.

In the example in the previous section, we assigned the `OidcProvider` during the initialization phase.
However, it is also possible to set this property just before calling the login function, e.g. to give the user a choice between different providers.
Each `OidcProvider` has to be initialized with their own client credentials.

### Adding the Client Credentials

An application can only use a provider's OpenID Connect API, if you have registered a client at the API.
This is a manual step that has to be done once at the beginning of the development.
During the registration process, the provider will generate a client ID and a client secret.
To access the API's methods, the client has to include these credentials in the requests.

You can find instructions on how to register clients in the section [Client Registration]{#ClientRegistration}

**Protect your client data**. As the name suggests, your client secret (and other client data as well) should be kept confidential.
Therefore, they should be excluded if the project's source code is uploaded to public places such as GitHub.
To solve this, the OpenID Connect Service can load client credentials from a separate file in the project.
To define the client credentials, right-click somewhere in your Assets folder or any subfolder and choose *"Create > i5 Toolkit > OpenID Connect Client Data* to create the file.
If you select it, you can enter the client credentials in Unity's inspector.
If you are using Git, you can now add the created file to *.gitignore* so that it will not be uploaded.
In the setup instructions of your project, include a note that each developer has to create their own client credentials and add them to the file.

### Defining the Redirect URI

Before the login page is shown, a redirect URI should be specified by setting the `RedirectURI` property of the service.
The importance of this redirect URI varies based on the target platform.

#### Editor & Standalone

> Setting the redirect URI for editor and standalone builds is optional.
> You can do it to improve the user experience.

In the editor and standalone builds, the toolkits starts an internal server to which the login automatically redirects.
This way, the server always fetches the necessary data which are provided in the redirect.
After that, the user is redirected to the specified URI which can e.g. point to your Web page that tells the user to return to the application.
So, for editor and standalone builds, setting the `RedirectURI` property is optional and can be used to improve the user's experience.

#### UWP

> UWP builds require a redirect URI which needs to be set to a custom URI schema.
> The custom URI schema must be registered in the provider's client configuration and in Unity's player settings.

In UWP builds, the redirect URI is essential.
Set it to a custom URI scheme, e.g. `i5:/`.
After that, go to the player settings (Edit > Project Settings and select the player tab).
Make sure that you are in the UWP settings (the tab with the Windows logo) and nagivate to the "Publishing Settings".
There is an entry "protocol" where you can enter the custom URI scheme.
So, in this example, you would enter "i5" - so omit the ":/" part here.
When this is done, the built app will open whenever an URI that starts with i5:/ is called.

Make sure that in the player settings under "Other Settings", the scripting runtime version is set to .NET 4.x Equivalent and the scripting backend is IL2CPP.
To retrieve the data that is contained in the login redirect, the i5 Toolkit has an OIDC patcher which will post-process the built IL2CPP.
When building the app, there should be an entry in the log console about the OIDC patcher running successully.
The patcher will add a hook to the generated C++ files which links the received redirect data back into the Unity C# world.

The redirect from the login page to the custom URI schema only works if you add the custom URI schema to the list of allowed redirect URIs in the client's configuration on the provider's Web page.
So, in our example, you need to add "i5:/" as an allowed redirect URI, e.g. at the Learning Layers client configuration page.

### Starting the Login Process

First, make sure that an OpenID Connect provider has been set.
The `IOidcProvider` has to be configured with the client credentials.
One way to initialize it is to attach a `MonoBehaviour` script to the button UI object in the scene that triggers the login.
The script has a public field where you can reference the client credentials.
Once the button is clicked, first create an instance of the `IOidcProvider` that should be used and assign the client credentials.
After that, assign the instance to the `OidcProvider` property of the service.

Before starting the login process, subscribe to the `LoginCompleted` event to get notified once the login procedure is completed.
To start the login process, call the `OpenLoginPage()` method of the `OpenIDConnectService`.
This will show the login page of the provider to the user in the system's default browser.
Once the user is logged in, all the necessary redirects and requests to get the access token are made automatically.
After the login process has finished successfully, the `LoginCompleted` event is raised.


### Bringing the Application Back into Focus

#### UWP

On UWP, the application is automatically brought back into focus if you have specified the custom URI schema as the service's `RedirectURI` and if you have added it as a protocol in the player settings.
Using a custom URI schema as a redirect URI is mandatory for this platform since the redirect contains the necessary data to finish the login.

#### Editor & Standalone

For these platforms, the OpenID Connect service only handles the redirect of information.
Without additional logic or configurations, the user manually has to return to the application.
You can set the `RedirectURI` to a Web page which should tell the user to return to the app.
You can also search for more platform-specific approaches that get the application back into view.

### Logout

To log out, call the `Logout` method of the OpenID Connect service.
There is also an event `LogoutCompleted` which is raised after the logout.

## Client Registration {#ClientRegistration}

### Creating a Learning Layers Client

To register a learning layers client, follow these steps:
1. Go to [https://api.learning-layers.eu/o/oauth2/](https://api.learning-layers.eu/o/oauth2/).
2. Click the "Log in" button.
3. Login with your user credentials or create a new account if you do not have one.
4. In the left side bar, select "Self-service client registration".
5. Click the button "Register a new client".
6. Enter a client name.
7. Add redirect URIs, e.g. http://127.0.0.1.
   If your app uses a custom URI schema, you must add the redirect URI to the list, e.g. mySchema:/.
8. If you want to, you can customize the approval page that is shown to the user when the OpenID Connect scopes have to be authorized.
   You can add a logo and links to Web pages of the terms of service, policy and home page.
   Moreover, you can add furhter e-mail contacts apart from your own one to indicate who is responsible for this client.
   All configurations from this step are optional and do not influence the functionality of the OpenID Connect workflow.
9. Scroll up again and select the "Access" tab on the page.
10. Add the scopes that you need to the list of scopes.
    Only the scopes which are listed here can be requested by the client.
11. Make sure that "Grant Types" is set to "authorization code".
12. Go to the "Other" tab.
    Change the default max age to 1440000 so that the access token is valid for a longer time.
13. Leave all other settings at their default values.
    Click the "Save" button at the top or bottom to finalize the client generation.
14. You will now be presented with the client credentials.
    Copy the values in the fields "Client ID", "Client Secret", "Client Configuration URL" and "Registration Access Token" and save them somewhere on your hard drive.
    It is important that you keep these values secret and you need them to authorize the client.
    The values are also required if you later want to edit the client's settings.
    
To change settings at a later point, enter the asked values on the right under "Self-service client registration".

To use the client credentials in the application, proceed with the section [Adding the Client Credentials](#adding-the-client-credentials).

## Example Scene

There is an example scene which shows how to set up and use the Learning Layers OpenID Connect client.
To use the scene, first register a client at the [Learning Layers provider](https://api.learning-layers.eu/o/oauth2/).
After that, create the client credentials file by right-clicking in the Assets browser and choosing "Create > i5 Toolkit > OpenID Connect Client Data".
Select the created file and enter the client id and client data in the inspector.
Then, select the "Tester" GameObject in the scene and drag and drop the file onto the field on the OpenID Connect Tester.
After these initialization, you can start the scene.
If you press F5, the browser is opened with the Learning Layers login page.
Once you log in and return to the app, the console will print the access token and same information about the logged in user.

The important GameObjects in example scene are the *Service Bootstrapper* and *Tester*.
The service manager bootstrapper on the *Service Bootstrapper* initializes the `OpenIDConnectService`.
The *Tester* GameObject contains the configuration of the Learning Layers OpenID Connect client.
It also triggers the login procedure and reacts to the successful login.