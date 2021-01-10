namespace i5.Toolkit.Core.GitVersion
{
    public class GitVersionCalculator : IGitVersionCalculator
    {
        private IGitRunner gitRunner;

        public GitVersionCalculator()
        {
            gitRunner = new GitRunner();
        }

        public bool TryGetVersion(out string version)
        {
            int resCode = gitRunner.RunCommand(@"describe --tags --long --match ‘v[0–9]*’", out string output, out string errors);
            if (resCode != 0)
            {
                UnityEngine.Debug.LogWarning("Error running git: " + errors);
                version = "0.1";
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