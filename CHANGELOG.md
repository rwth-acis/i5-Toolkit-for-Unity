# Changelog

This document keeps track of the changes between versions of the toolkit.

## 1.2.0 (Preview)

### Added
- In-application console which can show log outputs in deployed applications

### Added
- Json utilitiy to serialize and deserialize arrays at root level

### Changed
- Refactored unit test helpers into own assemblies so that they can be accessed separately

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