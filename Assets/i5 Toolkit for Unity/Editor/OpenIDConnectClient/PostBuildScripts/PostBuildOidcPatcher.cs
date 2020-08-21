using i5.Toolkit.Core.Utilities;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#endif
using UnityEngine;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    public class PostBuildOidcPatcher
    {
#if UNITY_EDITOR

        private const string namespacePlaceholder = "<<MYNAMESPACE>>";

        [PostProcessBuild(1)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (target == BuildTarget.WSAPlayer)
            {
                string appBaseFilePath = pathToBuiltProject + "/" + PlayerSettings.productName + "/App";
                if (!File.Exists(appBaseFilePath + ".cpp"))
                {
                    Debug.LogError("[i5 Toolkit PostProcessBuild] OIDC patcher did not run. Could not find .cpp file.");
                    return;
                }
                if (!File.Exists(appBaseFilePath + ".h"))
                {
                    Debug.LogError("[i5 Toolkit PostProcessBuild] OIDC patcher did not run. Could not find .h file.");
                    return;
                }
                Debug.Log("[i5 Toolkit PostProcessBuild] Running OIDC patcher...");
                string appBaseSourcePath = PathUtils.GetPackagePath() + "Editor/OpenIDConnectClient/PostBuildScripts/App";
                try
                {
                    string spaceEscapedProductName = PlayerSettings.productName.Replace(' ', '_');

                    string cppContent = File.ReadAllText(appBaseSourcePath + ".cpp.txt");
                    cppContent = cppContent.Replace(namespacePlaceholder, spaceEscapedProductName);
                    File.WriteAllText(appBaseFilePath + ".cpp", cppContent);

                    string hContent = File.ReadAllText(appBaseSourcePath + ".h.txt");
                    hContent = hContent.Replace(namespacePlaceholder, spaceEscapedProductName);
                    File.WriteAllText(hContent, appBaseFilePath + ".h");
                    Debug.Log("[i5 Toolkit PostProcessBuild] OIDC patcher successfully included protocol redirect hook");
                }
                catch (IOException e)
                {
                    Debug.LogError("[i5 Toolkit PostProcessBuild] OIDC Patcher failed: " + e.ToString());
                }
            }
        }
#endif
    }
}