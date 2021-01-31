# Version Tool

![Version Tool](../resources/Logos/Version-Tool.svg)

## Semantic Versioning

Applications and packages should be released using the [semantic versioning schema](https://semver.org/).
It consists of a major release number, a minor release number, a patch number and sometimes a revision number.
These numbers are separated by dots which leads to versions such as 1.2.3.
Here, 1 is the major release, 2 the minor release and 3 the patch number.

Each of the numbers is incremented by some rules. The major version is only increased if the app's API fundamentally changes so that breaking changes are introduced.
The minor number is bumped if new features are added but the API is still compatible.
The patch number is incremented if bugfixes are added which do not break the application.

## Use Case

The version number of an application should be incremented to show what kind of changes have happened but also to make sure that testers can overwrite already installed versions on a device.
If the version number is not incremented or even lowered, testers first need to deinstall apps on devices such as Android, UWP and iOS and can only install the new version of the app after that.
This means that in a setup where a continuos integration pipeline creates installer files, the version should be incremented automatically.
Developers can determine the major and minor version.
The patch number can be incremented automatically.

Since Unity stores the app's version number in the project settings, changing the version number is a change that needs to be added to the source control.
This means that the version number cannot be stored explicitly in the Unity project itself since calculating and applying a version number contributes another entry in the source control's history.
The versioning tool allows developers to add placeholder variables to the version string in Unity's project settings.
They can then define the major and minor version using Git tags, e.g. v1.2.
When building the installer files of the app, the versioning tool automatically calculates the version number from the available information in Git and writes it into the built application.

## Usage

The version tool allows you to add placeholders in the project's version string.
You can add any number of placeholders and you can combine them with other text.

To edit your project's version number go to "Edit > Project Settings..." in the top menu of Unity's editor.
After that, select the "Player" tab in the opened window.
At the top, there is a field version.
Enter the version schema with the placeholders here.

The following placeholders are available:

| Placeholder | Meaning | Example Value |
| --- | --- | --- |
| `$gitVersion` | Gets the version which consists of a major, minor and patch number based on Git tags. | 1.2.3 |
| `$gitBranch` | Gets the current branch name | develop |
| `$appVersion` | Gets the version from the environment variable `APP_VERSION`. If the variable is not set, it behaves like `$gitVersion`. | 1.2.3 |

In order to calculate the correct version numbers, tag your application's releases with Git tags.
The tags must have the form `v1.2`, so start them with a "v", followed by the major and minor version number.

The version is automatically applied to a built application.
This means that the version number is not available in the Unity editor but only in the compiled application package.

The version number is currently applied to installation files for the following platforms:

| Platform | Used Format |
| --- | --- |
| Standalone | Uses the version string that was set in the player settings and where the placeholders are replaced |
| UWP | Uses a version number format with four numbers: 1.2.3.4. The first three numbers are reconstructed from the `$appVersion` placeholder. The last number is always 0 as it is reserved by the Windows Store. |
| Android | Uses one single number. It is calculated from the total number of commits that have been made on the currently checked out branch. |

Some CI systems do not allow Unity to start other processes so that Git cannot be executed.
In these cases, you can use the `$appVersion` placeholder and you can set the environment variables to bypass the Git execution.

For UWP, the number can be overwritten by setting the environment variable `APP_VERSION`.
If it is non-empty, the value of the variable is used.
Otherwise, Git is executed to calculate the version.

For Android, the number can also be overwritten by setting the environment variable `ANDROID_APP_VERSION`.
If this variable is set, it is parsed to an integer and applied to the Android version.
In case that the variable is not set or the value cannot be parsed to an integer, the version tool tries to fetch the number of commits on the branch using Git.

## Recommended Setup

It is recommended to follow the [gitflow workflow](https://www.atlassian.com/de/git/tutorials/comparing-workflows/gitflow-workflow).
You should use a main branch which contains stable releases, a develop branch which works towards the next release and feature branches for individual features.

To get correct version numbers, tag releases on the master branch in the form `v1.0`.
Hence, start the tag with a `v`, followed by the major and minor release number.

The given way of calculating versions is not able to give each commit on every branch a unique version.
If two features are developed in parallel, they share the same version numbers since their number of commits since the last tag can be identical.
Therefore, it is recommended to produce versioned builds from the main branch.
On the master branch, each version is unique since branches that are merged into master are put into a sequential order again.

If you want to provide preview builds, you can provide builds from the develop branch but you should label them as preview to indicate that their versions are not final and might change on the master branch.

Some CI runner do not allow Unity to access Git.
In this case, you can use the environment variable `APP_VERSION` to calculate the version as part of the CI script.
To also pre-calculate the Android version, set the `ANDROID_APP_VERSION` environment variable.
The version tool then uses the values of these environment variables.

To calculate the version of the application in the same way as the version tool, use the following Linux bash script:

```
#! /bin/bash

DESCRIBE=`git describe --tags --long --match v[0-9]\*`

VERSION=`echo $DESCRIBE | awk '{split($0,a,"-"); print a[1]}'`
BUILD=`echo $DESCRIBE | awk '{split($0,a,"-"); print a[2]}'`

if [[ "${DESCRIBE}" =~ ^[A-Fa-f0-9]+$ ]]; then
    VERSION="0.0.0"
    BUILD=`git rev-list HEAD --count`
    BUILD=${DESCRIBE}
fi

if [ "${BUILD}" = "" ]; then
    BUILD='0'
fi

echo ${VERSION:1}.${BUILD}
```

Assuming that the script is called `get_version.sh`, you can set the environment variable using:

```
APP_VERSION=`./ci/get_version.sh`
```

## Testing
You can test the data that the versioning tool reads from Git using the menu at the top in the Unity editor:
The available entries are:

| Menu Entry | Output | Example Output |
| --- | --- | --- |
| i5 Toolkit > Build Versioning > Get Semantic Version | Gets the major and minor version from Git tags and the patch version from the number of commits since the last git tag. The calculated version is output as a log message | 1.2.3 |
| i5 Toolkit > Build Versioning > Get Git Branch | Logs the name of the currently checked out Git branch in the console | develop |
| i5 Toolkit > Build Versioning > Get Total Commits on Branch | Counts the total number of commits that are tracked on the currently checked out branch and logs them in the console. | Total number of commits on branch: 42 |