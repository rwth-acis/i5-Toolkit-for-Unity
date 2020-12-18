#!/bin/bash

version=$1
versionReplaceString="s/\${version}/$version/g"

# =============== README =====================

readmeMainPath='../README.md'
readmeDocPath='../Documentation/index.md'
readmePackagePath='../Assets/i5 Toolkit for Unity/README.md'

# Copy modified README to repository front page, documentation and package
cp README.md "$readmeMainPath"
cp README.md "$readmeDocPath"
cp README.md "$readmePackagePath"

# replace the version and paths
sed -i -e "$versionReplaceString" -e "s/\${docPath}/https:\/\/rwth-acis.github.io\/i5-Toolkit-for-Unity\/$version\//g" -e "s/\${docImgPath}/Documentation\//g" -e "s/\${docExtension}/html/g" "$readmeMainPath"
sed -i -e "$versionReplaceString" -e "s/\${docPath}//g" -e "s/\${docImgPath}//g" -e "s/\${docExtension}/md/g" "$readmeDocPath"
sed -i -e "$versionReplaceString" -e "s/\${docPath}/https:\/\/rwth-acis.github.io\/i5-Toolkit-for-Unity\/$version\//g" -e "s/\${docImgPath}/https:\/\/rwth-acis.github.io\/i5-Toolkit-for-Unity\/$version\//g" -e "s/\${docExtension}/html/" "$readmePackagePath"

# =============== Documentation =====================

docPackagePath='../Assets/i5 Toolkit for Unity/Documentation~/i5-Toolkit-for-Unity.md'

# Copy Documentation file to package
cp Documentation.md "$docPackagePath"

# replace the version
sed -i "$versionReplaceString" "$docPackagePath"

# =============== Package =====================

packageJsonPath='../Assets/i5 Toolkit for Unity/package.json'

# Copy package file to package
cp package.json "$packageJsonPath"

# replace the version
sed -i "$versionReplaceString" "$packageJsonPath"

# =============== CHANGELOG =====================

# Copy changelog to repository front page and package
cp CHANGELOG.md ../CHANGELOG.md
cp CHANGELOG.md ../Assets/'i5 Toolkit for Unity'/CHANGELOG.md

# =============== Project Settings =====================
sed -i "/bundleVersion:/s/.*/  bundleVersion: $version/" "../ProjectSettings/ProjectSettings.asset"