using i5.Toolkit.Core.Utilities;
using System;

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
        // placeholder that is replaced with the value of the environment variable $APP_VERSION
        private const string appVersionPlaceHolder = "$appVersion";
        // name of the enviornment variable
        private const string appVersionVarName = "APPVERSION";

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

            versionString = ReplaceGitVersionPlaceholder(versionString);
            versionString = ReplaceBranchPlaceholder(versionString);
            versionString = ReplaceAppVersionPlaceholder(versionString);

            return versionString;
        }

        // replaces the version placeholder
        private string ReplaceGitVersionPlaceholder(string versionString)
        {
            if (!versionString.Contains(gitVersionplaceholder))
            {
                return versionString;
            }

            i5Debug.Log("Version placeholder found.", this);

            i5Debug.Log("Running versioning tool to calculate semantic version number from Git tags", this);
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

        private string ReplaceAppVersionPlaceholder(string versionString)
        {
            if (!versionString.Contains(appVersionPlaceHolder))
            {
                return versionString;
            }

            // first check if APPVERSION variable was set
            string appVersionVar = Environment.GetEnvironmentVariable(appVersionVarName);

            if (!string.IsNullOrEmpty(appVersionVar))
            {
                i5Debug.Log("Using environment variable APPVERSION to replace version placeholder.", this);
                versionString = versionString.Replace(gitVersionplaceholder, appVersionVar);
            }
            // else: try calculating it using git
            else
            {
                i5Debug.Log("Running versioning tool to calculate semantic version number from Git tags", this);
                if (!gitVersion.TryGetVersion(out string version))
                {
                    i5Debug.LogWarning($"Could not get version name. Version placeholder will be replaced with default {version}", this);
                }

                versionString = versionString.Replace(gitVersionplaceholder, version);
            }
            return versionString;
        }

        /// <summary>
        /// Calculates the version which can be applied to WSA packages
        /// e.g. for UWP builds
        /// The version is extracted from the version string
        /// </summary>
        /// <returns>Returns the version for the WSA packages</returns>
        public Version WSAVersion
        {
            get
            {
                gitVersion.TryGetVersion(out string versionString);
                return VersionUtilities.StringToVersion(versionString);
            }
        }

        /// <summary>
        /// Calculates the version for Android installation packages
        /// This value is based on the number of commits in git on this branch
        /// </summary>
        /// <returns>Returns an integer number that is increased with each git commit</returns>        
        public int AndroidVersion
        {
            get
            {
                gitVersion.TryGetTotalCommitsOnBranch(out int commitCount);
                return commitCount;
            }
        }
    }
}
