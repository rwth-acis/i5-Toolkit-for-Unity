# Contributing

First of all, thank you for your interest in contributing to the toolkit.

This contribution guide should help you prepare your contribution.
It informs about some conventions that we introduced to structure the toolkit.

## Admissible Contributions

The goal of this open-source project i5 Toolkit for Unity is to provide and collect features, functionalities, modules or utility scripts that are useful and often required when working with Unity.

Please, make sure that your contribution fulfills the following criteria:

- Additions must target the 3D engine Unity
- New modules must be reusable and self-contained, i.e., developers must be able to integrate the module into an empty project with as little steps as possible.
- Additions should not introduce new dependencies.
- Additions must be tested on the current LTS and the legacy LTS version of Unity.

## Posting Pull Requests

To contribute to the project, create a fork on GitHub and work on this copy.
After finishing the implementation, post a pull request from your repository into the i5 Toolkit's develop branch.

### New Modules

If you want to add a new module to the toolkit, please provide a short description of the problem that your module solves in the pull request.
To describe your module's use case, you can refer to sample scenes that you have added to the module.

### Bugfixes

If you found a bug in the toolkit, please first create an issue, describing the bug with steps on how to reproduce it.
We will answer to the posted issue and can figure out together whether there is a quick fix to the bug.
If you already found a solution to this bug that requires changes to the toolkit's source code, create a pull request.
Reference the issue with the bug's description and also write down how you solved the bug.

## Project Structure

The toolkit follows Unity's recommended structure for Unity packages.
All content can be found under "*Assets/i5 Toolkit for Unity*".
There are four folders of relevance: Runtime, Editor, Samples, Tests

### Runtime

Code that should be executed in play mode and in the final built application, is bundled in the "Runtime" folder.
Every separate module should get its own folder.
Within this folder, structure assets according to Unity's guidelines.
So, create separate folders for each type of asset, e.g., 3D models, textures or scripts, and sort the content in there.
Do not place any scenes in the runtime folders.
If you want to create an example scene, place it in the Samples folder instead.

### Editor

Scripts which are editor extensions, e.g., custom inspectors or editor windows should be placed in the "Editor" folder.
The contents of this file are ignored in builds of Unity projects, i.e., they are exclusively for in-editor functionality.
Edit mode scripts can reference components from the Runtime folder but not vice versa.

The editor folder follows a similar structure as the runtime folder.
There should be one folder per module and within this module folder, assets should be divided by type.

### Samples

There is also a "Samples" or "Samples~" folder in the package.
This folder bundles the sample scenes that developers can explore when downloading the package.
During the development of the toolkit, we rename the folder to "Samples" so that they appear in Unity and so that we can edit the scenes.
However, for the final release, it is renamed again to "Samples~" since folders ending with a "~" are not imported by Unity.
Instead, the samples become available via the package manager where they can be downloaded via the list of samples.
To rename the folder, please, use the commands "*git mv*" instead of renaming the folder directly in a file explorer or using the "*mv*" command.
This way, the changes still get tracked by Git and the history of the files persists.
Otherwise, the renaming operation is interpreted by Git as a separate deletion of the old folder and a new addition of new content.

### Tests

The tests folder contains the unit tests.
They are again separated into edit mode tests, located in the "*Tests/Editor*" folder, and play mode tests in the "*Tests/Runtime*" folder.
For both play mode and edit mode tests, there are further helper scripts.
Helper functions for Play mode tests are in the "Tests/TestHelpers" folder and for edit mode tests, the "Tests/EditModeTestHelpers" folder exists.
These two folders should contain functionality for testing that is meant to be shipped to developers, too.
So, they can e.g. use the AsyncTest script to wait for async calls to finish.
The "Tests/TestUtilities" folder contains utility scripts for the tests specifically in this repository.
Thus, the difference between the helper folder and the utilities folder is that the helpers should be shipped with the package, whereas the utilities are internal aids for the specific unit tests of the package.

## Documentation

The documentation can be found in the "Documentation" folder on the project's root level.
It is built with DocFX.

### DocFX Documentation

To generate documentation, install docfx.
Then, navigate with a console to a location in the file system where the documentation should be generated.
We recommend navigating to the project's root since the resulting "_site" folder is tracked in the gitignore there.
Type the command `docfx Documentation/docfx.json` where the path should point to the docfx.json file in the Documentation folder.
If you navigated to a different location than the project's root folder, you need to adapt the path accordingly.
As a result, a folder with the name "_site" is generated with the corresponding HTML files.

With the command `docfx serve`, the "_site" folder is hosted locally on "http://localhost:8080", meaning that the resulting documentation page can be inspected in a browser.

#### Instruction Manual

The documentation consists of textual files in the "Documentation/manual" folder.
Every module should get a markdown file in this folder which describes the overall functionality and gives usage examples in natural language.
New markdown files must also be added to the list in the "toc.yml" file.
Add an entry of the following schema to the list.
The name is the human-readable name of the documentation page as it will be shown in the navigation bar.
The href should be a relative path to the markdown file for this documentation.

```
- name: My Module
  href: Filename.md
```

Documentation pages for modules should follow a general structure:

- The documentation page must start with a level one-heading with the module's name.
  Keep the module name consistent throughout the documentation.
- Following the heading, there should be the icon representation of the module.
  In order to avoid oversized icons, give the image the aspect ratio 3:1.
  So, it must have three times the width compared to the height.
  Place the icon in the middle third.
  Save this specific icon representation under "Documentation/resources/Logos".
  Vector graphics like .svg files are preferred.
- Explain the use case of the module.
  What problem does it solve?
  Why is it relevant for developers?
- Add a main part to the documentation that explains how to use the module, which features it has and possibly how to extend the module.
- Add a final section that explains the example scene.
  How is it structured and how can the developers test the functionality in the test scene?

#### API Documentation

Apart from the instruction manual, there is also an API documentation.
This part is automatically generated.
To fill it with content, document classes, as well as public properties and methods of these classes, with [XML documentation comments](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/).
New classes are automatically detected as long as they are placed in the given "Runtime" and "Editor" folders of the package.
Unit tests are not included in the API documentation as they are irrelevant for using the package.

### README

The README file of the repository introduces the project, informs about the current version, provides installation instructions and gives an overview of available modules.
New modules must be registered in the modules table.
Do not edit the README file in the repository's root folder directly as it is overwritten by the automated templates.
Instead edit the README file in the "Templates" folder.

In the modules table, the modules are listed in alphabetical order.
Add the icon representation of the module as a .svg vector graphic file.
Moreover, a cell must contain the name of the module.
Each cell links to the documentation page of the module.
Here, do not give exact links but use the version placeholders as already shown by the other table entries.
This way, we can evolve the documentation for different release versions whilst making sure that links within the documentation only lead to pages of the same version.

### In-Package Documentation

There is also a "Documentation~" folder in the package.
This folder should not be filled with a full documentation.
Instead, there is a single file "i5-Toolkit-for-Unity.md" with a link to the online documentation.
Do not alter this file as it is overwritten by the automated templates which are described in the next section.

## Templates

Certain files in the package require the current release number, e.g. the README file, the index file of the documentation, the documentation link in the package and the package.json file.
In order to keep the release numbers consistent, these files have master templates files in the "Templates" folder.
These master blueprints contain placeholders for the version number which are filled in with the actual version number during the release preparation.

### Affected Files

If you need to edit one of the following files, edit the template file instead.

| File in the Package | Edit This File Instead |
| --- | --- |
| README.md | Templates/Readme.md |
| Documentation/index.md | Templates/Readme.md |
| Assets/i5 Toolkit for Unity/README.md | Templates/Readme.md |
| Assets/i5 Toolkit for Unity/Documentation~/i5-Toolkit-for-Unity.md | Templates/Documentation.md |
| Assets/i5 Toolkit for Unity/package.json | Templates/package.json |
| Changelog.md | Templates/Changelog.md |
| Assets/i5 Toolkit for Unity/Changelog.md | Templates/Changelog.md |

### Placeholders

The following placeholders can be used:

- `${version}`: replaced by the current version number

In the README file the following additional placeholders are available:

- `${docPath}`: replaced by the path to the documentation
- `${docExtension}`: replaced either by md or html based on the documentation environment
- `${docImgPath}`: replaced by the path to the images of the documentation

### Release Preparation

When we prepare a new release, we execute the script setVersion.sh in the Templates folder.
To do this, the Templates folder must be your current working folder in a Linux shell.
Then, execute the command `./setVersion.sh 1.0.0`.
It takes a string as an argument, containing the version number, in the example 1.0.0.
The version number should be set to the next number of the release.
It must have a major, minor and patch number.

It you are contributing a feature, it suffices to modify the files in the Templates folder but you do not need to execute the setVersion script yourself.
We will execute the script as part of the release preparation with the next determined version number.
So, at this point any of your changes to the files in the Templates folder will automatically be distributed throughout the project.