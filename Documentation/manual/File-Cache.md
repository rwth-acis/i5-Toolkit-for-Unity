# File Cache

## Use Case

Importing 3D assets and other ressources from the web at runtime is a great way to keep the content of an application up to date with out the need to update the client itself. But loading files from the web can take some time especially with bad internet connection. This service allows it to easily utilise a cache. Thereby files are loaded from the local filesystem instead of the web after they are loaded once. This increases the loading speed for files that are loaded multiple times. For sure it comes with the cost of memory space and some small aditional overhead to manage the cache.

## Usage

The File Cache itself is just a service that can be registered at the Service Manger. This service can then handle the caching of files. To further improve the usability of the service there exist also a ContentLoader called CacheAwareContentLoader that provides an easy interface to interact with the file cach service. Moreover the ObjImporter supports the usage of the File Cache as well. The following example walks you through the process of using the ObjImporter with the File Cache.

### Notes

The File Cache is in the namespace i5.Toolkit.Core.Caching.

### Settings

There are four settings that can be done when initializing a File Cache.

If the parameter `sessionPersistence` is set to `true` the Cache will reload the cache state of the last session. The default value is `false`.

The parameter `useSaveMode` activates the check of a MD5 checksum before loading a file from cache. This is intended to lower the risk of loading a modified file from cache. The default value is `false`.

With the parameter `cacheLocationOverride` one can set a different location to store the cache data in. The default value is `null`.

The parameter `daysValid` sets the threshold for how old a file is alowed to be until it should not be used any more by the cache. The default is 365 days.

All those parameters can be set when initializing the File Cache:

```[C#]
FileCache fc = new FileCache(sessionPersistence: false, useSaveMode: true, cacheLocationOverride: null, daysValid: 365)
```

## Example

This explaination will walk you through the process of using the CacheAwareContentLoader to download files and use the File Cache.

First of all we need to use the namespace for the cache and the genereal service core.

```[C#]
using i5.Toolkit.Core.Caching;
using i5.Toolkit.Core.ServiceCore;
```

Before we are able to use the File Cache we first of all need to instaziate this service and add it the service manager. Since we only need to do this once, one will most likely do this in the start method of a unity MonoBehaviour class. After the File Cache is reistered we can instanziate the CacheAwareContentLoader.

```[C#]
void Start()
    {
        //Register the File Cache
        FileCache fileCache = new FileCache();
        ServiceManager.RegisterService(fileCache);

        CachAwareContentLoader contentLoader = new CachAwareContentLoader();
    }
```

This is all the setup that is required. Afterwards one can use the ContentLoader normally. For example we will write an update method that loads a file from a web adress (here stored in the variable path) when the user hits F5.

```[C#]
private async void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            WebResponse<string> resp = await ContentLoader.LoadAsync(path);

            //do what ever you want to with the dowloaded file in the WebResponse
        }
    }
```

When hitting F5 the second the time the CachAwareContentLoader will use the cached file and therefore load the obj faster.