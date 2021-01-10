using UnityEditor;

namespace i5.Toolkit.Core.GitVersion
{
    public static class GitEditorMenu
    {
        private static GitVersionCalculator gitVersion;

        private static void EnsureGitVersion()
        {
            if (gitVersion == null)
            {
                gitVersion = new GitVersionCalculator();
            }
        }

        [MenuItem("i5 Toolkit/Build Versioning/Get Semantic Version")]
        public static void TestVersion()
        {
            EnsureGitVersion();
            if (gitVersion.TryGetVersion(out string version))
            {
                UnityEngine.Debug.Log("Version: " + version);
            }
            else
            {
                UnityEngine.Debug.LogError($"Could not get version. Using default {version}. See previous output for error messages");
            }
        }

        [MenuItem("i5 Toolkit/Build Versioning/Get Git Branch")]
        public static void TestBranch()
        {
            EnsureGitVersion();
            if (gitVersion.TryGetBranch(out string branchName))
            {
                UnityEngine.Debug.Log("Branch: " + branchName);
            }
            else
            {
                UnityEngine.Debug.LogError($"Could not get branch. See previous output for error messages");
            }
        }

        [MenuItem("i5 Toolkit/Build Versioning/Get Total Commits on Branch")]
        public static void TestTotalCommits()
        {
            EnsureGitVersion();
            if (gitVersion.TryGetTotalCommitsOnBranch(out int commitCount))
            {
                UnityEngine.Debug.Log("Total number of commits on branch: " + commitCount);
            }
            else
            {
                UnityEngine.Debug.LogError("Could not get commit count. See previous output for error messages");
            }
        }
    }
}