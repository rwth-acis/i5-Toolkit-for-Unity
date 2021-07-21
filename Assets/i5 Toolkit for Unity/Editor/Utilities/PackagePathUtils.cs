#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace i5.Toolkit.Core.Utilities
{
    /// <summary>
    /// Utilities for paths
    /// </summary>
    public class PackagePathUtils
    {
        /// <summary>
        /// The name of the package
        /// </summary>
        private const string packageName = "i5 Toolkit for Unity";
        private const string packageIdentifier = "com.i5.toolkit.core";

        private static string cachedPath;

        /// <summary>
        /// Returns the path where the toolkit is located
        /// Use this if you want to address files inside of the toolkit
        /// The package can exist in the Assets folder, e.g. during development or by importing the unitypackage
        /// It exists in a Packages folder outside of the assets if it was loaded by the package manager
        /// </summary>
        /// <returns>Returns the path where the toolkit is located</returns>
        public static string GetPackagePath()
        {
            if (!string.IsNullOrEmpty(cachedPath))
            {
                return cachedPath;
            }

            // for development or unitypackages, the toolkit is located under "Assets/i5 Toolkit"
            if (Directory.Exists("Assets/" + packageName))
            {
                cachedPath = "Assets/" + packageName + "/";
                return cachedPath;
            }
            // for packages, the package is located in a Packages folder outside of the Assets folder
            else
            {
                cachedPath = "Packages/" + packageIdentifier + "/";
                return cachedPath;
            }
        }
    }
}
#endif