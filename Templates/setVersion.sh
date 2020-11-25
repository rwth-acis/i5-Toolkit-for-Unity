#!/bin/bash

version=$1
versionReplaceString="s/\${version}/$version/"

# =============== README =====================

# Copy modified README to repository front page, documentation and package
cp README.md ../README.md
cp README.md ../Documentation/index.md
cp README.md ../Assets/'i5 Toolkit for Unity'/README.md

# replace the version
sed $versionReplaceString ../README.md
sed $versionReplaceString ../Documentation/index.md
sed $versionReplaceString ../Assets/'i5 Toolkit for Unity'/README.md

# =============== Documentation =====================

# Copy Documentation file to package
cp Documentation.md ../Assets/'i5 Toolkit for Unity'/Documentation~/i5-Toolkit-for-Unity.md

# replace the version
sed $versionReplaceString ../Assets/'i5 Toolkit for Unity'/Documentation~/i5-Toolkit-for-Unity.md

# =============== Package =====================

# Copy package file to package
cp package.json ../Assets/'i5 Toolkit for Unity'/package.json

# replace the version
sed $versionReplaceString ../Assets/'i5 Toolkits for Unity'/package.json

# =============== CHANGELOG =====================

# Copy changelog to repository front page and package
cp CHANGELOG.md ../CHANGELOG.md
cp CHANGELOG.md ../Assets/'i5 Toolkit for Unity'/CHANGELOG.md