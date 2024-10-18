# Changelog

This document keeps track of the changes between versions of the toolkit.

## 1.9.5 (2024-10-19)

### Added
- Added VisionOS support for the OpenID Connect module

## 1.9.4 (2024-10-10)

### Fixed
- Prepared the toolkit for Unity 6 and fixed a compilation error in the async/await implementation due to the new async/await added by Unity

## 1.9.3 (2024-09-01)

### Added
- Added an option to directly set the access token in the Open ID Connect service

### Changed
- Fixed a link in the GitHub OpenID Connect implementation

## 1.9.2 (2024-01-16)

### Changed
- Updated the soft dependency of the xAPI module to com.unity.nuget.newtonsoft-json instead of the jilleJr package. Advanced mode of the xAPI module will now be activated if com.unity.nuget.newtonsoft-json is installed.

### Fixed
- Fixed an error in simple mode of the xAPI module where sent statements were rejected due to an incorrect serialization of the type field.

## 1.9.1 (2023-10-22)

### Added
- Added a RestConnector which encodes JSON payloads as byte arrays.

### Changed
- Spawner will now place the instantiated object at the spawner's position.

### Fixed
- GitHub login now uses the new RestConnector to encode JSON payloads in the expected format

## 1.9.0 (2023-02-13)

### Added
- Added Google as an OpenID Connect login provider.
- Added a VerboseLogging system:
  Generating log messages through this system means that they can be filtered based on importance.
  The verbosity level, so the strictness of the filtering, can be changed at runtime.

### Changed
- Refactored the implementation of OpenID Connect providers:
  All providers now share a base implementation so that new providers can be added quicker and without needing to duplicate code.
- OpenID Connect providers can now fetch the endpoints for the login functionality automatically based on the server's discovery document.
  New providers have to specify the base URL of the server and all other endpoints are fetched.
- The i5 Toolkit's WebRequest implementation is now setting a Content-Type header in the Put and Post requests.
- The i5 Toolkit's WebRequest implementation for Delete requests now creates a download handler so that the server's response can be read.

### Update Guide
- We have refactored the OpenID Connect implementation, so the following measures are necessary:
  - If you have implemented custom providers, make sure that they are now derived from the `AbstractOIDCProvider` base class.
    This also means that it is usually not necessary anymore to re-implement the methods.
    For most OpenID Connect providers, it suffices to provide the base URL of the server in the constructor for the new provider and the implementation will automatically find the endpoints using the OpenID Connect server's discovery document.
  - Similarly, the provider-specific user info and authorization info response formats need to be derived from `AbstractUserInfo` and `AbstractAuthorizationFlowAnswer`.
  - Replace all calls to the `OpenIDConnectService`'s method `OpenLoginPage()` with `OpenLoginPageAsync()`.
    You can use the async-await module from the `i5.Toolkit.Core.Utilities` namespace to await the call.

## 1.8.1 (2022-06-15)

### Changed
- Updated URLs for Learning Layers login provider
- Rocket.Chat login method now returns a WebResponse with the parsed response to help developers differentiate between network errors and login errors

### Fixed
- WebResponse objects of unsuccessful requests to the Rocket.Chat client now return the correct error messages instead of empty strings

## 1.8.0 (2022-05-18)

### Added
- Added Rocket.Chat client module to send and receive messages from Rocket.Chat
- Added PlayerPref utilities for arrays, vectors and colors
- Added Vector extensions for converting vectors to arrays and vice versa
- Added Color extensions for converting colors to arrays and vice versa

## 1.7.0 (2021-11-18)

### Added
- Added File Cache that allows the local storage of remote files to reduce load times in repeated requests
- Added Box Volume utility for defining cuboid volumes
- Added BoxCollider adapter to UnityAdapters namespace
- Added File and Directory Accessor adapter classes to SystemAdapters namespace

## 1.6.4 (2021-08-26)

### Added
- Added example scene showing how to use the ObjectPool with GameObjects

### Changed
- Updated documentation links in example scenes

### Fixed
- Fixed an error in the Obj Importer that occurred if an obj file without a linked mtl file was loaded

## 1.6.3 (2021-08-08)

### Added

- Extended functionality of Experience API module:
  Added the ability to specify contexts and results in xAPI statements using an advanced mode

### Changed

- Divided Experience API module into a standard and an advanced mode
- The advanced mode requires the Newtonsoft JSON library.
  If it is not added to the project, the module will fall back to the previous implementation that uses Unity's JsonUtiltiy serializer

## 1.6.2 (2021-07-21)

### Added

- Added support for local paths in the Obj Importer
- Added Unity adapters. This module is still in experimental stage as its interfaces will be extended in future releases
- Added utility functions for Vector3 component-wise operations and composed bounds calculation
- Added utility function to rewrite two paths into an absolute path
- Added further unit tests
- Extended documentation for the Object Pool module

### Changed

- Renamed `PathUtils` that fetch the package's path into `PackagePathUtils`

## 1.6.1 (2021-05-11)

### Changed

- Updated Learning Layers OpenID Connect Provider in order to adapt it to their update to Keycloak

## 1.6.0 (2021-04-22)

### Added

- Added an xAPI module which can be integrated into Unity applications to send analytics statements to Learning Record Stores

### Changed

- Changed the IRestConnector interface by adding a `PostAsync` method that can also take raw byte data as a payload.

### Removed

- Removed the old OpenID Connect patcher that injected C++ code into the built IL2CPP applications as it became obsolete with the refactored internal logic of the OpenID Connect Service

### Update Guide

- If you are using custom classes that implement the IRestConnector interface, add the the new `PostAsync` method to the class' implementation.

## 1.5.1 (2021-04-12)

### Added

- Added support for Android and iOS for the OpenID Connect Service

### Changed

- Refactored the OpenID Connect Service so that it uses the Deep Linking Service internally

## 1.5.0 (2021-04-08)

### Added

- Added a service and attribute to quickly set up APIs for mobile deep links

### Changed

- Changed minimum Unity version to 2019.4 since Unity 2018.4 reached the end of its LTS support

## 1.4.0 (2021-03-28)

### Added
- Added support for GitHub OpenID Connect login
- Added an option to the `IRestConnector` interface methods to specify a header dictionary
- Added an option to the `UnityWebRestConnector` to process a supplied header dictionary
- Added an example for the GitHub OpenID Connect login
- Added an example that shows how to use multiple OpenID Connect providers in parallel

## 1.3.4 (2021-03-18)

### Changed
- Project is now saved after running the Version Tool on builds so that Git does not pick up temporary changes

## 1.3.3 (2021-02-12)

### Changed
- Improved OpenID Connect support for workflows where the port needs to be set to a fixed number

## 1.3.2 (2021-01-31)

### Added
- Added option to Version Tool to specify version using environment variables `APP_VERSION` and `ANDROID_APP_VERISON`.
  If the environment variable is set, it is used instead of executing Git.

## 1.3.1 (2021-01-16)

### Fixed
- Fixed tag filter in Version Tool

## 1.3.0 (2021-01-14)

### Added
- Version Tool which automatically applies a version based on Git information
- Json Dictionary Utilities for serializing dictionaries

## 1.2.1 (2020-12-10)

### Fixed
- Fixed a compilation error that was caused by the OpenID Connect module when building with IL2CPP on other platforms than UWP

### Changed
- Added an instance of the app console to the OpenID Connect sample scene.
  This way, the console output with the login result can be seen in compiled applications.

## 1.2.0 (2020-12-07)

### Added
- In-application console which can show log outputs in deployed applications
- Json utilitiy to serialize and deserialize arrays at root level
- Unity adapter classes
  - Interfaces for text displays, activatable objects, UI rectangles and scroll views
  - Adapter for text UI (TextMesh, TextMeshPro and TextMeshProUGUI)
  - Adapter for GameObject, RectTransform, ScrollRect
- Experimental notification system

### Changed
- Sample scenes can now be accessed with the Unity Package UI (with Unity Package Manager 2.0 in Unity 2019.1 or later)
- Documentation moved to [GitHub pages](https://rwth-acis.github.io/i5-Toolkit-for-Unity)

## 1.1.0 (2020-08-24)

### Added
- Added OpenID Connect client implementation as a login solution for the editor, standalone and UWP.
- Added a solution to mark objects as persistent so that they are not unloaded on scene changes.
- Added UI operator which can simulate user interactions with the UI.
  This is useful for creating system tests where the user interaction should be automated.

### Changed
- Service Manager is not a MonoBehaviour anymore.
- Service bootstrappers can now be standalone components that do not depend on the existence of a service manager in the scene.
- WebResponses now provide the entire text of the response body in case of an error.

### Update Guide
- The IService interface now requires a IServiceManager in the Initialize method.
  To update, just replace `Initialize(ServiceManager owner)` with `Initialize(IServiceManager owner)` in your services.
- Service Manager is not a MonoBehaviour anymore.
  If you added the Service Manager to a scene in the editor, just remove the component.
- Bootstrappers now have to inherit from `BaseServiceBootstrapper` instead of `IServiceBootstrapper`.
  To update, change the inheritance and implement the methods `RegisterServices()` and `UnRegisterServices()` in your bootstrappers.
  Since the Service Manager is not a MonoBehaviour anymore, the bootstrapper component can now be placed anywhere in the scene.

## 1.0.1 (2020-07-02)

### Fixed
- Gizmo copy procedure now works on package installations

## 1.0.0 (2020-06-30)

### Added
- API for constructing procedural geometry, materials and fetching textures
- ObjImporter for importing .obj files and .mtl files from the Web
- Tool for creating extruded rectangles with rounded edges, e.g. for UI elements
- Object Pool for reusing resources
- GameObject spawners that support
  - Spawning multiple objects
  - Spawn limits
  - Managing spawned objects
- Scene documentation objects
- Utilities
  - Function for adding a component or getting an existing instance on a given GameObject
  - Functions for parsing space separated numbers to Vector2 or Vector3
  - Editor function for getting the path of the package
- Extensions for converting Vector2 and Vector3 to Color objects and vice versa
- Unit tests for the added modules
- Example scenes for the added modules
- Package logo and icons for the added modules
- Readme with setup instructions
- Changelog
- Package license
- Assembly definition files to structure the package
- JSON file for the package description