namespace i5.Toolkit.Core.VersionTool
{
    /// <summary>
    /// Version calculator based on Git
    /// </summary>
    public interface IGitVersionCalculator
    {
        /// <summary>
        /// Tries to calculate a version string based on git's tag information
        /// </summary>
        /// <param name="version">If the version could be calculated, it is passed to this out parameter</param>
        /// <returns>Returns true if the version could be calulcated, otherwise false</returns>
        bool TryGetVersion(out string version);

        /// <summary>
        /// Tries to get the name of the currently checked out git branch
        /// </summary>
        /// <param name="branchName">If the branch name could be fetched, it is passed to this out parameter</param>
        /// <returns>Returns true if the branch name could be fetched, otherwise false</returns>
        bool TryGetBranch(out string branchName);

        /// <summary>
        /// Tries to count the number of commits on the currently checked out branch
        /// </summary>
        /// <param name="commitCount">If the number of commits could be calculated, it is passed to this out parameter</param>
        /// <returns>Returns true if the number of commits could be calculated, otherwise false</returns>
        bool TryGetTotalCommitsOnBranch(out int commitCount);
    }
}
