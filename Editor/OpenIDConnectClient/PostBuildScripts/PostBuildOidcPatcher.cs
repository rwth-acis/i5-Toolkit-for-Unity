using i5.Toolkit.Core.Utilities;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#endif
using UnityEngine;

public class PostBuildOidcPatcher
{
#if UNITY_EDITOR
    [PostProcessBuild(1)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        if (target == BuildTarget.WSAPlayer)
        {
            string appBaseFilePath = pathToBuiltProject + "/" + PlayerSettings.productName + "/App";
            if (File.Exists(appBaseFilePath + ".cpp"))
            {
                Debug.Log("[i5 Toolkit PostProcessBuild] Running OIDC Patcher...");
                string appBaseSourcePath = PathUtils.GetPackagePath() + "Editor/OpenIDConnectClient/PostBuildScripts/App";
                try
                {
                    File.Copy(appBaseSourcePath + ".cpp.txt", appBaseFilePath + ".cpp", true);
                    File.Copy(appBaseSourcePath + ".h.txt", appBaseFilePath + ".h", true);
                    Debug.Log("[i5 Toolkit PostProcessBuild] OIDC Patcher successfully included protocol redirect hook");
                }
                catch (IOException e)
                {
                    Debug.LogError("[i5 Toolkit PostProcessBuild] OIDC Patcher failed: " + e.ToString());
                }
            }
            else
            {
                Debug.LogWarning("[i5 Toolkit PostProcessBuild] OIDC Patcher did not run. The generated project does not seem to be a C++ project.");
            }
        }
    }
#endif
}
