using System;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace i5.Toolkit.Core.GitVersion
{
    public class GitVersionBuildStep
    {
        private const string toolName = "i5 Build Versioning Tool";
        private const string gitVersionplaceholder = "$gitVersion";
        private const string branchPlaceholder = "$branch";

        private IGitVersionCalculator gitVersion;

        public GitVersionBuildStep()
        {
            gitVersion = new GitVersionCalculator();
        }

        public string ReplacePlaceholders(string versionString)
        {
            GitVersionCalculator gitVersion = new GitVersionCalculator();

            versionString = ReplaceVersionPlaceholder(versionString);
            versionString = ReplaceBranchPlaceholder(versionString);

            return versionString;
        }

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

        public int AndroidVersion()
        {
            gitVersion.TryGetTotalCommitsOnBranch(out int commitCount);
            return commitCount;
        }
    }
}
