namespace i5.Toolkit.Core.VersionTool
{
    /// <summary>
    /// Uses Git to calculate the version of the application
    /// </summary>
    public class GitVersionCalculator : IGitVersionCalculator
    {
        private IGitRunner gitRunner;

        /// <summary>
        /// Creates a new instance of the version calculator
        /// </summary>
        public GitVersionCalculator()
        {
            gitRunner = new GitRunner();
        }

        /// <summary>
        /// Tries to get the version string based on git tags
        /// Assign a tag of the form "v1.2" in git
        /// The method will output a version of the form "1.2.n" where n is the number of commits since this tag
        /// </summary>
        /// <param name="version">If a version could be retrieved, it will be passed to this out parameter</param>
        /// <returns>Returns true if the version could be read, otherwise false (e.g. when no tag exists or if git is not installed)</returns>
        public bool TryGetVersion(out string version)
        {
            int resCode = gitRunner.RunCommand("describe --tags --long --match \"v[0-9]*\"", out string output, out string errors);
            if (resCode != 0)
            {
                UnityEngine.Debug.LogWarning("Error running git: " + errors);
                version = "0.1.0";
                return false;
            }
            else
            {
                version = output;
                version = version.Replace('-', '.');
                version = version.Remove(version.LastIndexOf('.'));
                version = version.Replace("v", "");
                return true;
            }
        }

        /// <summary>
        /// Tries to get the name of the currently checked out branch in git
        /// </summary>
        /// <param name="branchName">If the branch name could be fetched, it is passed to this out parameter</param>
        /// <returns>Returns true if the branch name could be fetched, otherwise false</returns>
        public bool TryGetBranch(out string branchName)
        {
            int resCode = gitRunner.RunCommand(@"rev-parse --abbrev-ref HEAD", out string output, out string errors);
            if (resCode != 0)
            {
                UnityEngine.Debug.LogWarning("Error running git: " + errors);
                branchName = "UNKNOWN";
                return false;
            }
            else
            {
                branchName = output;
                return true;
            }
        }

        /// <summary>
        /// Tries to count the number of commits on the currently checked out branch
        /// </summary>
        /// <param name="commitCount">If the number of commits could be fetched, it is passed to this out parameter</param>
        /// <returns>Returns true if the number of commits could be calculated, false otherwise</returns>
        public bool TryGetTotalCommitsOnBranch(out int commitCount)
        {
            int resCode = gitRunner.RunCommand(@"rev-list --count HEAD", out string output, out string errors);
            if (resCode != 0)
            {
                UnityEngine.Debug.LogWarning("Error running git: " + errors);
                commitCount = 0;
                return false;
            }
            else
            {
                return int.TryParse(output, out commitCount);
            }
        }
    }
}