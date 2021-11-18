# File Cache

![File Cache](../resources/Logos/FileCache.svg)

## Use Case

Importing 3D assets and other ressources from the web at runtime is a great way to keep the content of an application up to date without the need to update the client itself.
However, loading files from the web can take some time especially with bad internet connection.
This service allows it to easily utilise a cache.
Thereby files are loaded from the local filesystem instead of the web after they are loaded once.
This increases the loading speed for files that are loaded multiple times.
For sure it comes with the cost of memory space and some small aditional overhead to manage the cache.

## Usage

The file cache itself is a service that can be registered at the Service Manger.
This service can handle the caching of files.
To further improve the usability of the service, there exist a ContentLoader called CacheAwareContentLoader that provides an interface to interact with the file cache service.
This content loader can be set on any other module which needs to load content from the Web, e.g. on the Obj Importer.

The following example walks you through the process of using the ObjImporter with the file cache.

### Notes

The file cache is located in the namespace `i5.Toolkit.Core.Caching`.

### Settings

There are four settings that can be configured when initializing a File Cache.

- If the parameter `sessionPersistence` is set to `true` the Cache will reload the cache state of the last session.
  The default value is `false`.
- The parameter `useSafeMode` activates the check of a MD5 checksum before loading a file from cache.
  It ensures that cached files cannot be swapped out as their checksum will change.
  The default value is `false`.
- With the parameter `cacheLocationOverride` one can set a different location to store the cache data in.
  The default value is `null`.
- The parameter `daysValid` sets the threshold for how old a file is alowed to be until it should not be used any more by the cache.
  The default is 365 days.

All those parameters can be set when initializing the file cache:

```[C#]
FileCache fc = new FileCache(sessionPersistence: false, useSaveMode: true, cacheLocationOverride: null, daysValid: 365);
```

## Example Scene

This explaination will walk you through the process of using the CacheAwareContentLoader to download files and use the file cache.

First of all we need to use the namespace for the cache and the genereal service core.

```[C#]
using i5.Toolkit.Core.Caching;
using i5.Toolkit.Core.ServiceCore;
```

Before we are able to use the file cache we first of all need to instaziate this service and add it the service manager. Since we only need to do this once, one will most likely do this in the start method of a unity MonoBehaviour class. After the File Cache is reistered we can instanziate the CacheAwareContentLoader.

```[C#]

CacheAwareContentLoader contentLoader;

void Start()
    {
        //Register the File Cache
        FileCache fileCache = new FileCache();
        ServiceManager.RegisterService(fileCache);

        contentLoader = new CacheAwareContentLoader();
    }
```

This is all the setup that is required. Afterwards one can use the ContentLoader normally. For example we will write an update method that loads a file from a web adress (here stored in the variable path) when the user hits F5.

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

When hitting F5 the second the time the CachAwareContentLoader will use the cached file and therefore load the obj faster.