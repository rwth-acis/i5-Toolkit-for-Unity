# Obj Importer

![Obj Importer](../resources/Logos/ObjImporter.svg)

The obj importer can fetch 3D models in the .obj file format from the Web at runtime.
This way, content can dynamically be loaded from a Web storage.
It allows developers to maintain a list of 3D models online which can be tweaked and exchanged without updates to the deployed applications.
Moreover, users can upload 3D models to the Web and so, the application can load user-specified content.

The obj importer also fetches any referenced material files in the .mtl format.
Moreover, associated textures in materials are also loaded.

## Usage

Use the method `ImportAsync(url)` to load a 3D object from the given URL and import it as a new GameObject.
The method runs asynchronously because of the Web request.
Moreover, the geometry construction happens in the background on a separate thread so that there should be no noticable impact on the performance.

The specified URL should point directly to the .obj file.
If it does not point to a file that ends with .obj, a warning is given but the import is still tried.

## Example Scene
The example scene shows how to load different .obj files.
In the folder "Obj Models", different 3D models which have been created in Blender, are stored.
They can be loaded using the link to the raw object on GitHub.
The 3D models vary in complexity, the number of objects, materials and textures.