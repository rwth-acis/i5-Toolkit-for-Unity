using i5.Toolkit.Core.Utilities;
using i5.Toolkit.Core.Experimental.SystemAdapters;
using System;

namespace i5.Toolkit.Core.VersionTool
{
    /// <summary>
    /// Logic for the build step that applies the git version
    /// </summary>
    public class GitVersionBuildStep
    {
        // placeholder that is replaced with the version number
        private const string gitVersionPlaceholder = "$gitVersion";
        // placeholder that is replaced with the branch name
        private const string branchPlaceholder = "$gitBranch";
        // placeholder that is replaced with the value of the environment variable $APP_VERSION
        private const string appVersionPlaceholder = "$appVersion";
        // name of the enviornment variable
        private const string appVersionVarName = "APP_VERSION";
        // name of the android environment variable
        private const string androidAppVersionVarName = "ANDROID_APP_VERSION";

        public const string toolName = "Version Tool";

        private IGitVersionCalculator gitVersion;
        private ISystemEnvironment systemEnvironment;

        /// <summary>
        /// Creates a new instance of the build logic step
        /// </summary>
        public GitVersionBuildStep()
        {
            gitVersion = new GitVersionCalculator();
            systemEnvironment = new SystemEnvironmentAdapter();
        }

        public bool ContainsPlaceholder(string versionString)
        {
            return versionString.Contains(gitVersionPlaceholder)
                || versionString.Contains(branchPlaceholder)
                || versionString.Contains(appVersionPlaceholder);
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
            if (!versionString.Contains(gitVersionPlaceholder))
            {
                return versionString;
            }

            i5Debug.Log("Version placeholder found.", this);

            i5Debug.Log("Running versioning tool to calculate semantic version number from Git tags", this);
            if (!gitVersion.TryGetVersion(out string version))
            {
                i5Debug.LogWarning($"Could not get version name. Version placeholder will be replaced with default {version}", this);
            }

            versionString = versionString.Replace(gitVersionPlaceholder, version);

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
            if (!versionString.Contains(appVersionPlaceholder))
            {
                return versionString;
            }

            // first check if APPVERSION variable was set
            string appVersionVar = systemEnvironment.GetEnvironmentVariable(appVersionVarName);

            if (!string.IsNullOrEmpty(appVersionVar))
            {
                i5Debug.Log($"Using environment variable {appVersionVarName} to replace version placeholder.\n" +
                    $"{appVersionVarName} has the value {appVersionVar}", this);
                versionString = versionString.Replace(appVersionPlaceholder, appVersionVar);
            }
            // else: try calculating it using git
            else
            {
                i5Debug.Log("Running versioning tool to calculate semantic version number from Git tags", this);
                if (!gitVersion.TryGetVersion(out string version))
                {
                    i5Debug.LogWarning($"Could not get version name. Version placeholder will be replaced with default {version}", this);
                }

                versionString = versionString.Replace(appVersionPlaceholder, version);
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
                // first try to get environment variable
                string appVersion = systemEnvironment.GetEnvironmentVariable(appVersionVarName);
                if (!string.IsNullOrEmpty(appVersion))
                {
                    i5Debug.Log($"Using environment variable {appVersionVarName} to set WSA version.\n" +
                        $"{appVersionVarName} has the value {appVersion}", this);
                    return VersionUtilities.StringToVersion(appVersion);
                }
                else
                {
                    gitVersion.TryGetVersion(out string versionString);
                    i5Debug.Log($"Using git to set WSA version\n" +
                        $"Git returned {versionString}", this);
                    return VersionUtilities.StringToVersion(versionString);
                }
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
                string strAndroidAppVersion = systemEnvironment.GetEnvironmentVariable(androidAppVersionVarName);
                if (!string.IsNullOrEmpty(strAndroidAppVersion))
                {
                    if (int.TryParse(strAndroidAppVersion, out int androidAppVersion))
                    {
                        i5Debug.Log($"Using environment variable {androidAppVersionVarName} to set Android version.\n" +
                            $"Android version will be set to {androidAppVersion}", this);
                        return androidAppVersion;
                    }
                }

                // if variable not set or is not an int
                gitVersion.TryGetTotalCommitsOnBranch(out int commitCount);
                i5Debug.Log($"Using git to set Android version.\n" +
                    $"Android version will be set to {commitCount}", this);
                return commitCount;
            }
        }
    }
}
