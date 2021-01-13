using i5.Toolkit.Core.Utilities;
using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace i5.Toolkit.Core.VersionTool
{
    /// <summary>
    /// Logic for the build step that applies the git version
    /// </summary>
    public class GitVersionBuildStep
    {
        // placeholder that is replaced with the version number
        private const string gitVersionplaceholder = "$gitVersion";
        // placeholder that is replaced with the branch name
        private const string branchPlaceholder = "$gitBranch";

        public const string toolName = "Version Tool";

        private IGitVersionCalculator gitVersion;

        /// <summary>
        /// Creates a new instance of the build logic step
        /// </summary>
        public GitVersionBuildStep()
        {
            gitVersion = new GitVersionCalculator();
        }

        public bool ContainsPlaceholder(string versionString)
        {
            return versionString.Contains(gitVersionplaceholder)
                || versionString.Contains(branchPlaceholder);
        }

        /// <summary>
        /// Replaces all registered placeholders in the given version string
        /// </summary>
        /// <param name="versionString">The version string that contains placeholders</param>
        /// <returns>The version string where placeholders are replaced by the calculated values</returns>
        public string ReplacePlaceholders(string versionString)
        {
            GitVersionCalculator gitVersion = new GitVersionCalculator();

            versionString = ReplaceVersionPlaceholder(versionString);
            versionString = ReplaceBranchPlaceholder(versionString);

            return versionString;
        }

        // replaces the version placeholder
        private string ReplaceVersionPlaceholder(string versionString)
        {
            if (!versionString.Contains(gitVersionplaceholder))
            {
                return versionString;
            }

            i5Debug.Log("Version placeholder found. Running versioning tool to calculate semantic version number from Git tags", this);
            if (!gitVersion.TryGetVersion(out string version))
            {
                i5Debug.LogWarning($"Could not get version name. Version placeholder will be replaced with default {version}", this);
            }

            versionString = versionString.Replace(gitVersionplaceholder, version);

            return versionString;
        }

        // replaces the branch placeholder
        private string ReplaceBranchPlaceholder(string versionString)
        {
            if (!versionString.Contains(branchPlaceholder))
            {
                return versionString;
            }

            i5Debug.Log("Branch placeholder found. Running git to get the branch name", this);
            if (!gitVersion.TryGetBranch(out string branch))
            {
                i5Debug.LogWarning("Could not get branch name. Branch placeholder will be replaced with UNKNOWN", this);
            }

            versionString = versionString.Replace(branchPlaceholder, branch);
            return versionString;
        }

        /// <summary>
        /// Calculates the version which can be applied to WSA packages
        /// e.g. for UWP builds
        /// The version is extracted from the version string
        /// </summary>
        /// <param name="versionString">The version string on which the version should be based</param>
        /// <returns>Returns the version for the WSA packages</returns>
        public Version WSAVersion(string versionString)
        {
            Regex rgx = new Regex("[^0-9.]");
            versionString = rgx.Replace(versionString, "");
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

        /// <summary>
        /// Calculates the version for Android installation packages
        /// This value is based on the number of commits in git on this branch
        /// </summary>
        /// <returns>Returns an integer number that is increased with each git commit</returns>
        public int AndroidVersion()
        {
            gitVersion.TryGetTotalCommitsOnBranch(out int commitCount);
            return commitCount;
        }
    }
}
