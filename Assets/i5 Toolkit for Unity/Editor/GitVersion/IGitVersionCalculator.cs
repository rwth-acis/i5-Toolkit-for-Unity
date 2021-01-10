namespace i5.Toolkit.Core.GitVersion
{
    public interface IGitVersionCalculator
    {
        bool TryGetVersion(out string version);
        bool TryGetBranch(out string branchName);
        bool TryGetTotalCommitsOnBranch(out int commitCount);
    }
}
