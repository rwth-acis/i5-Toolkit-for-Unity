using UnityEditor;

namespace i5.Toolkit.Core.VersionTool
{
    /// <summary>
    /// Creates a menu entry in the top menu of Unity
    /// </summary>
    public static class GitEditorMenu
    {
        // cache the git version
        private static GitVersionCalculator gitVersion;

        // ensures that the git version exists
        private static void EnsureGitVersion()
        {
            if (gitVersion == null)
            {
                gitVersion = new GitVersionCalculator();
            }
        }

        /// <summary>
        /// Outputs the version based on the Git tag information
        /// </summary>
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

        /// <summary>
        /// Gets the name of the currently checked out git branch
        /// </summary>
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

        /// <summary>
        /// Gets the total number of commits on the branch
        /// </summary>
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