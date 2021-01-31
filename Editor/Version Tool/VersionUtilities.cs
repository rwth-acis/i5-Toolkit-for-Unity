using System;
using UnityEngine;

namespace i5.Toolkit.Core.VersionTool
{
    /// <summary>
    /// Common functions working with versions
    /// </summary>
    public static class VersionUtilities
    {
        /// <summary>
        /// Converts a version string of the form 1.2.3
        /// to a version object of the form 1.2.3.0
        /// If the version string is shorter, unset version numbers are set to 0
        /// </summary>
        /// <param name="versionString">The version as a string</param>
        /// <returns>Returns the parsed version or 0.0.1.0 if the string could not be parsed</returns>
        public static Version StringToVersion(string versionString)
        {
            if (Version.TryParse(versionString, out Version result))
            {
                int major = Mathf.Max(0, result.Major);
                int minor = Mathf.Max(0, result.Minor);
                int build = Mathf.Max(0, result.Build);
                return new Version(major, minor, build, 0);
            }
            else
            {
                return new Version(0, 0, 1, 0);
            }
        }
    }
}