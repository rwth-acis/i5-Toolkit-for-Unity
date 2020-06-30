# i5 Toolkit for Unity

![i5 Toolkit for Unity](https://raw.githubusercontent.com/rwth-acis/i5-Toolkit-for-Unity/develop/Logos/Logo%20wide.svg)

This toolkit contains a collection of features which can be reused in Unity projects.
It is a foundation for new projects, kickstarting the development with already completed tools.

![Continuous Integration](https://github.com/rwth-acis/i5-Toolkit-for-Unity/workflows/Continuous%20Integration/badge.svg)

## Modules

The i5 Toolkit provides a series of modules and features that can be used in projects.

| &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<br/> [![Modified 3D Objects](https://raw.githubusercontent.com/rwth-acis/i5-Toolkit-for-Unity/master/Logos/Rounded%20Corners.svg)<br/>Modified 3D Objects](https://github.com/rwth-acis/i5-Toolkit-for-Unity/wiki/Modified-3D-Objects)<br/>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;  | &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<br/> [![Object Pool](https://raw.githubusercontent.com/rwth-acis/i5-Toolkit-for-Unity/master/Logos/Object%20Pool.svg)<br/>Object Pool](https://github.com/rwth-acis/i5-Toolkit-for-Unity/wiki/Object-Pool)<br/>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; | &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<br/> [![Obj Importer](https://raw.githubusercontent.com/rwth-acis/i5-Toolkit-for-Unity/master/Logos/Obj%20Importer.svg)<br/>Obj Importer](https://github.com/rwth-acis/i5-Toolkit-for-Unity/wiki/Obj-Importer)<br/>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; |
| :---: | :---: | :---: |
| [![Procedural Geometry](https://raw.githubusercontent.com/rwth-acis/i5-Toolkit-for-Unity/master/Logos/Procedural%20Geometry.svg)<br/>**Procedural Geometry**](https://github.com/rwth-acis/i5-Toolkit-for-Unity/wiki/Procedural-Geometry) | [![Scene Documentation](https://raw.githubusercontent.com/rwth-acis/i5-Toolkit-for-Unity/master/Logos/Scene%20Documentation.svg)<br/>**Scene Documentation**](https://github.com/rwth-acis/i5-Toolkit-for-Unity/wiki/Scene-Documentation) |  [![Service Core](https://raw.githubusercontent.com/rwth-acis/i5-Toolkit-for-Unity/master/Logos/ServiceCore.svg)<br/>**Service Core**](https://github.com/rwth-acis/i5-Toolkit-for-Unity/wiki/Service-Core)
| [![Spawner](https://raw.githubusercontent.com/rwth-acis/i5-Toolkit-for-Unity/master/Logos/Spawner.svg)<br/>**Spawner**](https://github.com/rwth-acis/i5-Toolkit-for-Unity/wiki/Spawner) | |

## Setup

There are different ways to add the package to a project.

### Alternative 1: Unity Dependency File with Git (Unity 2018.3 or later) (Recommended)

The tool is available as a package for the Unity Package Manager. It can be included in new projects by referencing the git-repository on GitHub in the dependency file of the Unity project:

1. Open your project's root folder in a file explorer.
2. Navigate to the Packages folder and open the file manifest.json. It contains a list of package dependencies which are loaded into the project.
3. To add a specific version of the tool to the dependencies, add the following line inside of the "dependencies" object and replace [version] with the release number, e.g. "v1.0".
   To receive the latest version, replace [version] with upm.
   `"com.i5.versiontool": "https://github.com/rwth-acis/Unity-VersionTool.git#[version]"`
   After that, Unity will automatically download and import the package.

If you specify "upm" to get the latest version, be aware that the package is not automatically updated.
This command just pulls the latest version which is available at that time.
To update to the newest current version, remove the package again and re-download it.

### Alternative 2: Unity Package Manager UI with Git (Unity 2019.3 or later)

The package can be downloaded from a git-repository in the package manager's UI.

1. In Unity, go to Window > Package Manger.
2. Click on the plus-button in the top left corner of the package manager and select "add".
3. Enter https://github.com/rwth-acis/i5-Toolkit-for-Unity.git#[version] into the text field where [version] is replaced with the release number, e.g. "v1.0" or upm for the latest version.
   Confirm the download by clicking on the "add" button.

If you specify "upm" to get the latest version, be aware that the package is not automatically updated.
This command just pulls the latest version which is available at that time.
To update to the newest current version, remove the package again and re-download it.

### Alternative 3: Import custom package (Unity 2017 or later)

Another option is to import the package as a .unitypackage.

1. Download the .unitypackage-file which is supplied with the corresponding release on the releases page.
2. With your project opened, perform a right-click on the assets browser in Unity. Select "Import Package > Custom Packge" from the context menu.
3. Navigate to the path where you downloaded the .unitypackage-file, select it and confirm by clicking the "Open" buttom
4. A dialog window opens where you can select which files should be imported. Select everything and click on "Import".

Important for alternative 3: If you are updating from an earlier version, it is recommended to delete the existing "i5 Toolkit" folder.
After that, import the new package.

## Example Scenes

The different modules and features are presented in example scenes which can be found in the [GitHub repository](https://github.com/rwth-acis/i5-Toolkit-for-Unity).
You can use the example scenes as an interactive documentation, an experimentation playground and to test the features.

## Unit Tests
The project is tested using Unit tests.
Continuous Integration using GitHub Actions has been set up to test and deploy new versions of the package.

## Related Projects

For Mixed Reality development, also check out the [i5 Toolkit for MR](https://github.com/rwth-acis/i5-Toolkit-for-Mixed-Reality).
It is an extension package that builds upon the functionality of this package and is optimized for Mixed Reality.