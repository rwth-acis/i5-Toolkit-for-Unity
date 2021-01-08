using System.Diagnostics;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class GitVersion
{
    public static bool TryGetVersion(out string version)
    {
        int resCode = RunGit(@"describe --tags --long --match ‘v[0–9]*’", out string output, out string errors);
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
            return true;
        }
    }

    public static bool TryGetBranch(out string branchName)
    {
        int resCode = RunGit(@"rev-parse --abbrev-ref HEAD", out string output, out string errors);
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

    public static bool TryGetTotalCommitsOnBranch(out int commitCount)
    {
        int resCode = RunGit(@"rev-list --count HEAD", out string output, out string errors);
        if (resCode != 0)
        {
            UnityEngine.Debug.LogWarning("Error running git commit counter: " + errors);
            commitCount = 0;
            return false;
        }
        else
        {
            return int.TryParse(output, out commitCount);
        }
    }

    [MenuItem("i5 Toolkit/Build Versioning/Get Semantic Version")]
    public static void TestVersion()
    {
        if (TryGetVersion(out string version))
        {
            UnityEngine.Debug.Log("Version: " + version);
        }
        else
        {
            UnityEngine.Debug.LogError($"Could not get version. Using default {version}. See previous output for error messages");
        }
    }

    [MenuItem("i5 Toolkit/Build Versioning/Get Git Branch")]
    public static void TestBranch()
    {
        if (TryGetBranch(out string branchName))
        {
            UnityEngine.Debug.Log("Branch: " + branchName);
        }
        else
        {
            UnityEngine.Debug.LogError($"Could not get branch. See previous output for error messages");
        }
    }

    [MenuItem("i5 Toolkit/Build Versioning/Get Total Commits on Branch")]
    public static void TestTotalCommits()
    {
        if (TryGetTotalCommitsOnBranch(out int commitCount))
        {
            UnityEngine.Debug.Log("Total number of commits on branch: " + commitCount);
        }
        else
        {
            UnityEngine.Debug.LogError("Could not get commit count. See previous output for error messages");
        }
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