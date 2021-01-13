using System.Diagnostics;
using System.Text;
using UnityEngine;

namespace i5.Toolkit.Core.VersionTool
{
    /// <summary>
    /// Bridge to the git command line interface
    /// </summary>
    public class GitRunner : IGitRunner
    {
        /// <summary>
        /// Runs git with the specified arguments
        /// </summary>
        /// <param name="arguments">The argument string which is passed to git</param>
        /// <param name="output">The standard output of the command</param>
        /// <param name="errors">The error output of the command</param>
        /// <returns>The exit code of the command</returns>
        public int RunCommand(string arguments, out string output, out string errors)
        {
            // create a new process that does not require interaction and that does not open a new window
            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    FileName = "git",
                    Arguments = arguments,
                    WorkingDirectory = Application.dataPath
                };

                StringBuilder stdoutputBuilder = new StringBuilder();
                StringBuilder errorBuilder = new StringBuilder();
                process.OutputDataReceived += (_, args) => stdoutputBuilder.AppendLine(args.Data);
                process.ErrorDataReceived += (_, args) => errorBuilder.AppendLine(args.Data);

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();

                output = stdoutputBuilder.ToString().TrimEnd();
                errors = errorBuilder.ToString().TrimEnd();
                return process.ExitCode;
            }
        }
    }
}