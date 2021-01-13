using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace i5.Toolkit.Core.VersionTool
{
    public class PostBuildVersionCleanup : IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPostprocessBuild(BuildReport report)
        {
            RestoreVersions();
            Debug.Log($"[{GitVersionBuildStep.toolName}] Removing temporary cache");
            VersionCache.Remove();
        }

        private void RestoreVersions()
        {
            VersionCache cache = VersionCache.Load();
            Debug.Log($"[{GitVersionBuildStep.toolName}] Restoring version config:\n{PlayerSettings.bundleVersion}->{cache.appVersion}\n{PlayerSettings.WSA.packageVersion}->{cache.wsaVersion}\n{PlayerSettings.Android.bundleVersionCode}->{cache.androidVersion}");
            PlayerSettings.bundleVersion = cache.appVersion;
            PlayerSettings.WSA.packageVersion = cache.wsaVersion;
            PlayerSettings.Android.bundleVersionCode = cache.androidVersion;
        }
    }
}