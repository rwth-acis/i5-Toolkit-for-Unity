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

    [MenuItem("i5 Toolkit/Get Semantic Version")]
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