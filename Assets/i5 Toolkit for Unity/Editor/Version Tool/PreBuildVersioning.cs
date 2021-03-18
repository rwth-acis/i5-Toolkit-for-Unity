using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace i5.Toolkit.Core.VersionTool
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

            CacheVersionConfig();

            GitVersionBuildStep buildStep = new GitVersionBuildStep();

            if (buildStep.ContainsPlaceholder(versionString))
            {
                versionString = buildStep.ReplacePlaceholders(versionString);
                // in any case: adjust the main version setting
                PlayerSettings.bundleVersion = versionString;

                // check if additional changes are required if we are building for another platform
                switch (report.summary.platformGroup)
                {
                    case BuildTargetGroup.WSA:
                        PlayerSettings.WSA.packageVersion = buildStep.WSAVersion;
                        break;
                    case BuildTargetGroup.Android:
                        PlayerSettings.Android.bundleVersionCode = buildStep.AndroidVersion;
                        break;
                }
            }
            else
            {
                Debug.Log($"[{GitVersionBuildStep.toolName}] Version placeholders not found. To use automatic semantic versioning with Git, add a placeholder to the application's version string");
            }
        }

        // caches the project's original version configuration
        // so that it can be restored after the build
        private void CacheVersionConfig()
        {
            Debug.Log($"[{GitVersionBuildStep.toolName}] Caching version config:\n" +
                $"Version: {PlayerSettings.bundleVersion}\n" +
                $"UWP version: {PlayerSettings.WSA.packageVersion}\n" +
                $"Android version: {PlayerSettings.Android.bundleVersionCode}\n");
            VersionCache cache = new VersionCache();
            cache.appVersion = PlayerSettings.bundleVersion;
            cache.wsaVersion = PlayerSettings.WSA.packageVersion;
            cache.androidVersion = PlayerSettings.Android.bundleVersionCode;

            cache.Save();
            Debug.Log($"[{GitVersionBuildStep.toolName}] Saved temporary cache");
        }
    }
}