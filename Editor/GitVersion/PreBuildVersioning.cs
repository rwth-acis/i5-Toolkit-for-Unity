using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace i5.Toolkit.Core.GitVersion
{
    /// <summary>
    /// Pre-Build step which replaces placeholders
    /// </summary>
    public class PreBuildVersioning : IPreprocessBuildWithReport
    {
        /// <summary>
        /// The order in which the step is integrated into the build process
        /// </summary>
        public int callbackOrder => 0;

        /// <summary>
        /// Called when the build is preprocessed
        /// </summary>
        /// <param name="report">The build report</param>
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