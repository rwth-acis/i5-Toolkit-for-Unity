# Obj Importer

![Obj Importer](../resources/Logos/ObjImporter.svg)

The obj importer can fetch 3D models in the .obj file format from the Web or local storage at runtime.
This way, content can dynamically be loaded from outside of the project.
It allows developers to maintain a list of 3D models online which can be tweaked and exchanged without updates to the deployed applications.
Moreover, users can upload 3D models to the Web and so, the application can load user-specified content.
With the local loading, 3D models from the Web can first be downloaded and cached for quicker access.

The obj importer also fetches any referenced material files in the .mtl format.
Moreover, associated textures in materials are also loaded.

## Usage

### Register the Service

First, register the <xref:i5.Toolkit.Core.ModelImporters.ObjImporter> as a service at the <xref:i5.Toolkit.Core.ServiceCore.ServiceManager>;

```[C#]
ObjImporter objImporter = new ObjImporter();
ServiceManager.RegisterService(objImporter);
```

When registering the service, it will automatically register an <xref:i5.Toolkit.Core.ModelImporters.MtlLibrary> service, too.
This service is required for importing the material files and caching them so that they can be reused.

### Import Models

Use the method <xref:i5.Toolkit.Core.ModelImporters.ObjImporter.ImportAsync(System.String)> to load a 3D object from the given URL or file path and import it as a new GameObject.
The method runs asynchronously to request the file from disk or to load it from the Web.
Moreover, the geometry construction happens in the background on a separate thread so that there should be no noticable impact on the performance.

```[C#]
string url = "https://raw.githubusercontent.com/rwth-acis/i5-Toolkit-for-Unity/master/Assets/i5%20Toolkit%20for%20Unity/Samples%7E/Importers/ObjImporter/Obj%20Models/Monkey_textured.obj"
GameObject obj = await ServiceManager.GetService<ObjImporter>().ImportAsync(url);
```

The specified path should point directly to the .obj file.
If it does not point to a file that ends with .obj, a warning is given but the import is still tried.

### Debugging

The service provides a field <xref:i5.Toolkit.Core.ModelImporters.ObjImporter.ExtendedLogging>.
If it is set to `true`, the ObjImporter gives more detailed log outputs about the import process.

```[C#]
ServiceManager.GetService<ObjImporter>().ExtendedLogging = true;
```

## Example Scene

The example scene shows how to load different .obj files.
In the folder "Obj Models", different 3D models which have been created in Blender, are stored.
They can be loaded using the link to the raw object on GitHub.
The 3D models vary in complexity, the number of objects, materials and textures.

To find the url of an example 3D model, navigate to the [model folder of the repository](https://github.com/rwth-acis/i5-Toolkit-for-Unity/tree/master/Assets/i5%20Toolkit%20for%20Unity/Samples%7E/Importers/ObjImporter/Obj%20Models), select one of the .obj files  and click on "View raw".
After that, copy the url from your browser into the url field of the ServiceClient in the ObjImporter example scene.
You can also enter absolute local paths.
The example provides a series of 3D models that can be tried out.

In PlayMode, press F5 to trigger the import.
You can also change the url field of the ServiceManager during PlayMode and import different 3D models into one session by pressing F5 after changing the model url.