using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace i5.Toolkit.Core.GitVersion
{
    public class PreBuildVersioning : IPreprocessBuildWithReport
    {
        private const string toolName = "i5 Build Versioning Tool";
        private const string gitVersionplaceholder = "$gitVersion";
        private const string branchPlaceholder = "$branch";

        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            string versionString = PlayerSettings.bundleVersion;

            GitVersionCalculator gitVersion = new GitVersionCalculator();

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

            if (versionString.Contains(branchPlaceholder))
            {
                Debug.Log($"[{toolName}] Branch placeholder found. Running git to get the branch name");
                if (!gitVersion.TryGetBranch(out string branch))
                {
                    Debug.LogWarning($"[{toolName}] Could not get branch name. Branch placeholder will be replaced with UNKNOWN");
                }

                versionString = versionString.Replace(branchPlaceholder, branch);
            }

            PlayerSettings.bundleVersion = versionString;
            Version packageVersion = StringToVersion(PlayerSettings.bundleVersion);
            PlayerSettings.WSA.packageVersion = packageVersion;
            if (gitVersion.TryGetTotalCommitsOnBranch(out int commitCount))
            {
                PlayerSettings.Android.bundleVersionCode = commitCount;
            }
        }

        private Version StringToVersion(string strVersion)
        {
            string[] versionFragments = strVersion.Split('.');
            int[] intVersion = new int[3];
            int index = 0;
            foreach (string fragment in versionFragments)
            {
                if (int.TryParse(fragment, out int intFragment))
                {
                    if (index < 3)
                    {
                        intVersion[index] = intFragment;
                        index++;
                    }
                    else
                    {
                        Debug.LogWarning($"[{toolName}] Version includes too many numbers");
                    }
                }
            }
            return new Version(intVersion[0], intVersion[1], intVersion[2], 0);
        }
    }
}