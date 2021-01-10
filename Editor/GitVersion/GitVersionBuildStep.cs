using System;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace i5.Toolkit.Core.GitVersion
{
    /// <summary>
    /// Logic for the build step that applies the git version
    /// </summary>
    public class GitVersionBuildStep
    {
        // name of the tool which is shown in the logs
        private const string toolName = "i5 Build Versioning Tool";
        // placeholder that is replaced with the version number
        private const string gitVersionplaceholder = "$gitVersion";
        // placeholder that is replaced with the branch name
        private const string branchPlaceholder = "$branch";

        private IGitVersionCalculator gitVersion;

        /// <summary>
        /// Creates a new instance of the build logic step
        /// </summary>
        public GitVersionBuildStep()
        {
            gitVersion = new GitVersionCalculator();
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
                Debug.Log($"[{toolName}] Version placeholder not found. To use automatic semantic versioning with Git, write the placeholder {gitVersionplaceholder} into the application's version");
            }
            else
            {
                Debug.Log($"[{toolName}] Version placeholder found. Running versioning tool to calculate semantic version number from Git tags");
                if (!gitVersion.TryGetVersion(out string version))
                {
                    Debug.LogWarning($"[{toolName}] Could not get version name. Version placeholder will be replaced with default {version}");
                }

                versionString = versionString.Replace(gitVersionplaceholder, version);
            }

            return versionString;
        }

        // replaces the branch placeholder
        private string ReplaceBranchPlaceholder(string versionString)
        {
            if (versionString.Contains(branchPlaceholder))
            {
                Debug.Log($"[{toolName}] Branch placeholder found. Running git to get the branch name");
                if (!gitVersion.TryGetBranch(out string branch))
                {
                    Debug.LogWarning($"[{toolName}] Could not get branch name. Branch placeholder will be replaced with UNKNOWN");
                }

                versionString = versionString.Replace(branchPlaceholder, branch);
            }
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
                return new Version(result.Major, result.Minor, result.Build, 0);
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
