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

        private static readonly char[] underscoreEscapeCharacters = { ' ', '-', '.' };
        private static readonly char[] additionalEscapeCharacters = { ',' };

        [PostProcessBuild(1)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (target == BuildTarget.WSAPlayer)
            {
                string baseFilePath = $"{pathToBuiltProject}/{PlayerSettings.productName}/";
                if (!File.Exists(baseFilePath + "App.cpp"))
                {
                    Debug.LogError("[i5 Toolkit PostProcessBuild] OIDC patcher did not run. Could not find App.cpp file.");
                    return;
                }
                if (!File.Exists(baseFilePath + "App.h"))
                {
                    Debug.LogError("[i5 Toolkit PostProcessBuild] OIDC patcher did not run. Could not find App.h file.");
                    return;
                }
                if (!File.Exists(baseFilePath + "Main.cpp"))
                {
                    Debug.LogError("[i5 Toolkit PostProcessBuild] OIDC patcher did not run. Could not find Main.cpp file.");
                    return;
                }

                Debug.Log("[i5 Toolkit PostProcessBuild] Running OIDC patcher...");
                string baseSourcePath = $"{PathUtils.GetPackagePath()}Editor/OpenIDConnectClient/PostBuildScripts/";

                try
                {
                    string spaceEscapedProductName = PlayerSettings.productName;
                    for (int i = 0; i < underscoreEscapeCharacters.Length; i++)
                    {
                        spaceEscapedProductName = spaceEscapedProductName.Replace(underscoreEscapeCharacters[i], '_');
                    }
                    foreach(char c in Path.GetInvalidPathChars())
                    {
                        spaceEscapedProductName = spaceEscapedProductName.Replace(c.ToString(), "");
                    }

                    string cppContent = File.ReadAllText(baseSourcePath + "App.cpp.txt");
                    cppContent = cppContent.Replace(namespacePlaceholder, spaceEscapedProductName);
                    File.WriteAllText(baseFilePath + "App.cpp", cppContent);

                    string hContent = File.ReadAllText(baseSourcePath + "App.h.txt");
                    hContent = hContent.Replace(namespacePlaceholder, spaceEscapedProductName);
                    File.WriteAllText(baseFilePath + "App.h", hContent);

                    string mainContent = File.ReadAllText(baseSourcePath + "Main.cpp.txt");
                    mainContent = mainContent.Replace(namespacePlaceholder, spaceEscapedProductName);
                    File.WriteAllText(baseFilePath + "Main.cpp", mainContent);

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