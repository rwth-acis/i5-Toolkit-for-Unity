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
                Debug.Log("[i5 Toolkit PostProcessBuild] Running OIDC Patcher");
                string appBaseSourcePath = PathUtils.GetPackagePath() + "Editor/OpenIDConnectClient/PostBuildScripts/App";
                File.Copy(appBaseSourcePath + ".cpp", appBaseFilePath + ".cpp", true);
                File.Copy(appBaseSourcePath + ".h", appBaseFilePath + ".h", true);
                Debug.Log("[i5 Toolkit PostProcessBuild] OIDC Patcher included protocol redirect hook");
            }
            
        }
    }
#endif
}
