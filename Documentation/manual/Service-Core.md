# Service Core System

![Service Core](../resources/Logos/ServiceCore.svg)

## General

The service system provides a way to add singletons to the project without the need to have GameObjects or MonoBehaviours.
Services are native C# classes which implement the <xref:i5.Toolkit.Core.ServiceCore.IService> interface.
Hence, the services also provide a way to abstract away from MonoBehaviours.

Whenever you need a centralized functionality that needs to be accessible globally, you can use services to realize it

## Usage

The central component of the service system is the <xref:i5.Toolkit.Core.ServiceCore.ServiceManager>.
It is the only component which needs to be placed on a GameObject.
However, you do not need to set up the GameObject yourself.
Whenever you use the <xref:i5.Toolkit.Core.ServiceCore.ServiceManager>, it automatically generates an instance on a GameObject and sets it up.

## Services

### Creating a Service

To create a service, create a class that does not inherit from <xref:UnityEngine.MonoBehaviour>.
The new service class must implement the interface <xref:i5.Toolkit.Core.ServiceCore.IService>.

If you need to call a function in every frame, implement the interface <xref:i5.Toolkit.Core.ServiceCore.IUpdateableService> instead.

### Updateable Services

A special type of service is the updateable service.
You can define a updateable service by creating a new C# class that implements only the <xref:i5.Toolkit.Core.ServiceCore.IUpdateableService> interface.
The interface provides the typical <xref:i5.Toolkit.Core.ServiceCore.IService> methods but also an <xref:i5.Toolkit.Core.ServiceCore.IUpdateableService.Update> method that is called every frame.
Moreover, it adds a property <xref:i5.Toolkit.Core.ServiceCore.IUpdateableService.Enabled>.
The code inside of <xref:i5.Toolkit.Core.ServiceCore.IUpdateableService.Update> is only executed if <xref:i5.Toolkit.Core.ServiceCore.IUpdateableService.Enabled> is set to `true`.

### Async Threaded Worker Service

One pre-defined service is the `AsyncThreadedWorkerService`.
Its purpose is to work on asynchronous, longer operations on a separate thread.
After the operation, a result is returned.
You can post operations to this service and the service will execute them on a separate thread.
If the operation block contains a callback, the callback method will be invoked once the operation has finished.
The result will be provided to the callback

*Important:* You cannot access the Unity API, e.g. GameObjects or Meshes, in the separate thread.
This means you cannot manipulate or read the scene in the operation.
Create your operation in a way that all information are available at the beginning of the operation and that you only need the result at the end.

## Registration of Services

Create an instance of the service using its constructor:
```
MyService myService = new MyService();
// do your initial configuration of the service here
```
After that, you can register a service by calling [`RegisterService`](xref:i5.Toolkit.Core.ServiceCore.ServiceManager.RegisterService*):
```
ServiceManager.RegisterService(myService)
```

After that, the service is available and can be addressed.
Registering the service automatically initializes it.

## Addressing Services

Once you have created and registered a service, you can access by calling [`GetService`](xref:i5.Toolkit.Core.ServiceCore.ServiceManager.GetService*)

```
ServiceManager.GetService<MyService>();
```

Specify the service class in the `GetService` method.

If the service is not registered, an <xref:System.InvalidOperationException> is thrown.
You can check if a service was registered by checking the bool return value of `ServiceManager.ServiceExists<MyService>()`.

## Removing Services

To remove the service, call [`RemoveService`](xref:i5.Toolkit.Core.ServiceCore.ServiceManager.RemoveService*)

```
ServiceManager.RemoveService<MyService>();
```

## Bootstrappers

You can create a bootstrapper script that automatically populates the service system.
The bootstrapper class needs to inherit from <xref:i5.Toolkit.Core.ServiceCore.BaseServiceBootstrapper>.
It must implement the methods <xref:i5.Toolkit.Core.ServiceCore.BaseServiceBootstrapper.RegisterServices> and <xref:i5.Toolkit.Core.ServiceCore.BaseServiceBootstrapper.UnRegisterServices> which are called once the component is started and once it is destroyed.
If you want to keep services after scene changes, either leave the <xref:i5.Toolkit.Core.ServiceCore.BaseServiceBootstrapper.UnRegisterServices> method empty or make the bootstrapper object persistent using <xref:i5.Toolkit.Core.Utilities.PersistenceScene.MarkPersistent(GameObject)>.

You do not need to use the <xref:i5.Toolkit.Core.ServiceCore.BaseServiceBootstrapper>.
Alternatively to a bootstrapper which adds all services at the beginning, you can dynamically register and unregister services at any time and in any script.

## Example Scene

There is an example scene that shows how to set up different kinds of services using the toolkit's service architecture.
The *Service Bootstrapper* GameObject contains a bootstrapper script which populates the service manager with services.

The demo scene contains an updateable service which logs the application's run time in intervals.
There is also a service which prints a statement to the console if you press F5.
Pressing F5 also triggers a simulated async operation which waits for a couple of seconds before printing a statement to the console.
The F5 functionality is triggered by a *Demo Service Client*.