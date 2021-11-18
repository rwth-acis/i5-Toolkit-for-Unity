# File Cache

![File Cache](../resources/Logos/FileCache.svg)

## Use Case

Importing 3D assets and other resources from the web at runtime is a great way to keep the content of an application up to date without the need to update the client itself.
However, loading files from the web can take some time especially with bad internet connection.
This service allows it store downloaded files in a cache.
Thereby, files are loaded from the local file system instead of the Web after they are loaded once.
This increases the loading speed for files that are loaded multiple times.

A common use case for this is the download of 3D models from the Web.

## Usage

The file cache itself is a service that can be registered at the <xref:i5.Toolkit.Core.ServiceCore.ServiceManager>.
This service can handle the caching of files.
To further improve the usability of the service, there exist an <xref:i5.Toolkit.Core.Utilities.ContentLoaders.IContentLoader`1> called <xref:i5.Toolkit.Core.Caching.CacheAwareContentLoader> that provides an interface to interact with the file cache service.
This content loader can be set on any other module which needs to load content from the Web, e.g. on the [Obj Importer](Obj-Importer.md).

### Notes

The file cache is located in the namespace `i5.Toolkit.Core.Caching`.

### Settings

There are four settings that can be configured when initializing a <xref:i5.Toolkit.Core.Caching.FileCacheService>.

- If the parameter `sessionPersistence` is set to `true` the Cache will reload the cache state of the last session.
  The default value is `false`.
- The parameter `useSafeMode` activates the check of a MD5 checksum before loading a file from cache.
  It ensures that cached files cannot be swapped out as their checksum will change.
  The default value is `true`.
- With the parameter `cacheLocationOverride` one can set a different location to store the cache data in.
  The default value is `null`.
- The parameter `daysValid` sets the threshold for how old a file is alowed to be until it should not be used any more by the cache.
  The default is 365 days.
- The parameter `directoryAccessor` can be used to configure how the file directories are accessed.
  In a productive environment, it is advised to leave this option untouched but it can e.g. be used in tests to fake a directory system.

All those parameters can be set when initializing the file cache:

```[C#]
FileCacheService fc = new FileCacheService(sessionPersistence: false, useSafeMode: true, cacheLocationOverride: null, daysValid: 365);
```

### Example

This explanation will walk you through the process of using the CacheAwareContentLoader to download files and use the file cache.

First of all, we need to use the namespace for the cache and the general service core.

```[C#]
using i5.Toolkit.Core.Caching;
using i5.Toolkit.Core.ServiceCore;
```

Before we are able to use the file cache, we first of all need to instantiate this service and add it the service manager.
Since we only need to do this once, one will most likely do this in the `Start` method of a Unity MonoBehaviour class.
Once the <xref:i5.Toolkit.Core.Caching.FileCacheService> is registered, we can instantiate the <xref:i5.Toolkit.Core.Caching.CacheAwareContentLoader>.

```[C#]

CacheAwareContentLoader contentLoader;

void Start()
{
    //Register the File Cache
    FileCacheService fileCache = new FileCacheService();
    ServiceManager.RegisterService(fileCache);

    contentLoader = new CacheAwareContentLoader();
}
```

This is all the setup that is required.
Afterwards one can use the contentLoader normally.
For example, we will write an update method that loads a file from a web address.
Here, it is stored in the variable path if the user hits F5.

```[C#]
private async void Update()
{
    if (Input.GetKeyDown(KeyCode.F5))
    {
        WebResponse<string> resp = await contentLoader.LoadAsync(path);

        // retrieve the dowloaded or cached file in the WebResponse
    }
}
```

When hitting F5 the second time, the <xref:i5.Toolkit.Core.Caching.CacheAwareContentLoader> will use the cached file and therefore load it faster.

Another usage example can be found in the documentation of the [Obj Importer](Obj-Importer.md) and in the example scene of the Obj Importer.