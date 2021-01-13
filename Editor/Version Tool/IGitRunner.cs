namespace i5.Toolkit.Core.VersionTool
{
    /// <summary>
    /// Bridge to git's command line interface
    /// </summary>
    public interface IGitRunner
    {
        /// <summary>
        /// Runs git with the given commands
        /// </summary>
        /// <param name="arguments">The arguments which should be passed to git</param>
        /// <param name="output">The output which is produced by git</param>
        /// <param name="errors">The errors which are produced by git</param>
        /// <returns>Returns the exit code of the command</returns>
        int RunCommand(string arguments, out string output, out string errors);
    }
}
