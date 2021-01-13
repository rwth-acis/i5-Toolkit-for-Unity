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

            CacheVersionConfig();

            GitVersionBuildStep buildStep = new GitVersionBuildStep();

            if (buildStep.ContainsPlaceholder(versionString))
            {
                versionString = buildStep.ReplacePlaceholders(versionString);

                PlayerSettings.bundleVersion = versionString;
                PlayerSettings.WSA.packageVersion = buildStep.WSAVersion(versionString);
                PlayerSettings.Android.bundleVersionCode = buildStep.AndroidVersion();
            }
            else
            {
                Debug.Log($"[{GitVersionBuildStep.toolName}] Version placeholders not found. To use automatic semantic versioning with Git, add a placeholder to the application's version string");
            }
        }

        private void CacheVersionConfig()
        {
            Debug.Log($"[{GitVersionBuildStep.toolName}] Caching version config:\n{PlayerSettings.bundleVersion}\n{PlayerSettings.WSA.packageVersion}\n{PlayerSettings.Android.bundleVersionCode}");
            VersionCache cache = new VersionCache();
            cache.appVersion = PlayerSettings.bundleVersion;
            cache.wsaVersion = PlayerSettings.WSA.packageVersion;
            cache.androidVersion = PlayerSettings.Android.bundleVersionCode;

            Debug.Log($"[{GitVersionBuildStep.toolName}] Saved temporary cache");
            cache.Save();
        }
    }
}