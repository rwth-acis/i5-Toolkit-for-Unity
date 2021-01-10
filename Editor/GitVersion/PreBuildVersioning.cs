using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace i5.Toolkit.Core.GitVersion
{
    public class PreBuildVersioning : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            string versionString = PlayerSettings.bundleVersion;

            GitVersionBuildStep buildStep = new GitVersionBuildStep();

            versionString = buildStep.ReplacePlaceholders(versionString);

            PlayerSettings.bundleVersion = versionString;
            PlayerSettings.WSA.packageVersion = buildStep.WSAVersion(versionString);
            PlayerSettings.Android.bundleVersionCode = buildStep.AndroidVersion();
        }
    }
}