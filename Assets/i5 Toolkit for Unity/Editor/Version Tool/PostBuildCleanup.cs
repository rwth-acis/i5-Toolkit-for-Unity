using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace i5.Toolkit.Core.VersionTool
{
    /// <summary>
    /// Clean up step that is executed after the build
    /// Resets version values which have been altered by the version tool
    /// This happens so that version control does not detect changes after the build
    /// </summary>
    public class PostBuildVersionCleanup : IPostprocessBuildWithReport
    {
        /// <summary>
        /// The position in the execution order in which this script is executed after the build
        /// </summary>
        public int callbackOrder => 0;

        /// <summary>
        /// Called once the build has finished
        /// Restores version project settings that have been altered by the version tool
        /// </summary>
        /// <param name="report">A report about the build</param>
        public void OnPostprocessBuild(BuildReport report)
        {
            RestoreVersions();
            Debug.Log($"[{GitVersionBuildStep.toolName}] Removing temporary cache");
            VersionCache.Remove();
        }

        // restores the versions
        private void RestoreVersions()
        {
            VersionCache cache = VersionCache.Load();
            Debug.Log($"[{GitVersionBuildStep.toolName}] Restoring version config:\n" +
                $"{PlayerSettings.bundleVersion}->{cache.appVersion}\n" +
                $"{PlayerSettings.WSA.packageVersion}->{cache.wsaVersion}\n" +
                $"{PlayerSettings.Android.bundleVersionCode}->{cache.androidVersion}\n");
            PlayerSettings.bundleVersion = cache.appVersion;
            PlayerSettings.WSA.packageVersion = cache.wsaVersion;
            PlayerSettings.Android.bundleVersionCode = cache.androidVersion;
            AssetDatabase.SaveAssets();
        }
    }
}