using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace i5.Toolkit.Core.GitVersion
{
    public class PostBuildVersionCleanup : IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPostprocessBuild(BuildReport report)
        {
            RestoreVersions();
            ClearCache();
        }

        private void RestoreVersions()
        {
            PlayerSettings.bundleVersion = VersionCache.appVersion;
            PlayerSettings.WSA.packageVersion = VersionCache.wsaVersion;
            PlayerSettings.Android.bundleVersionCode = VersionCache.androidVersion;
        }

        private void ClearCache()
        {
            VersionCache.appVersion = "";
            VersionCache.wsaVersion = new System.Version();
            VersionCache.androidVersion = 0;
        }
    }
}