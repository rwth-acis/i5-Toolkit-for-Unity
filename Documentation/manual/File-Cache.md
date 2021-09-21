# File Cache

## Use Case

Importing 3D assets and other ressources from the web at runtime is a great way to keep the content of an application up to date with out the need to update the client itself. But loading files from the web can take some time especially with bad internet connection. This service allows it to easily utilise a cache. Thereby files are loaded from the local filesystem instead of the web after they are loaded once. This increases the loading speed for files that are loaded multiple times. For sure it comes with the cost of memory space and some small aditional overhead to manage the cache.

## Usage

The file cache itself is just a service that can be registered at the Service Manger. This service can then handle the caching of files. To further improve the usability of the service there exist also a ContentLoader called CacheAwareContentLoader that provides an easy interface to interact with the file cach service. Moreover the ObjImporter supports the usage of the file cache as well. The following example walks you through the process of using the ObjImporter with the file cache.

### Notes

The file cache is in the namespace i5.Toolkit.Core.ModelImporters.

## Example

This explaination will walk you through the process of setting up an Object Importer that uses the file cache.

First of all we need to use the namespace for the cache, the importer and the genereal service core. The file cache and the importer are both in the same namespace.

```[C#]
using i5.Toolkit.Core.ModelImporters;
using i5.Toolkit.Core.ServiceCore;
```

Before we are able to use the file cache we first of all need to instaziate this service and add it the service manager. Since we only need to do this once, one will most likely do this in the start method of a unity MonoBehaviour class. The same holds for registering the ObjImporter at the service manager.

```[C#]
void Start()
    {
        //Register the file cache
        FileCache objCache = new FileCache();
        ServiceManager.RegisterService(objCache);

        //Register the object importer
        ObjImporter importer = new ObjImporter();
        ServiceManager.RegisterService(importer);
    }
```

This is all the setup that is required. Afterwards one can use the obj importer normally. For example we will write an update method that loads a obj file file from a web adress when the user hits F5.

```[C#]
private async void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            GameObject obj = await ServiceManager.GetService<ObjImporter>().ImportAsync("example.com/file.obj");

            //do what ever you want to witht the obj
        }
    }
```

When hitting F5 the second the time the ObjImporter will use the cached file and therefore load the obj faster.