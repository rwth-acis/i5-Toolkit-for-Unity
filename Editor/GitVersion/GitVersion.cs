using System.Diagnostics;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class GitVersion
{
    public static string Version
    {
        get
        {
            int resCode = RunGit(@"describe --tags --long --match ‘v[0–9]*’", out string output, out string errors);
            if (resCode != 0)
            {
                UnityEngine.Debug.LogError("Error running git: " + errors);
                return "0.1";
            }
            else
            {
                return output;
            }
        }
    }

    [MenuItem("i5 Toolkit/Get Semantic Version")]
    public static void TestVersion()
    {
        UnityEngine.Debug.Log("Version: " + Version);
    }

    /// <summary>
    /// Runs git.exe with the specified arguments and returns the output.
    /// </summary>
    public static int RunGit(string arguments, out string output, out string errors)
    {
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