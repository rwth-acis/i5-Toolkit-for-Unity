using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEditor;
using UnityEngine;

namespace i5.Toolkit.Core.AnalyzerLinker
{
    public static class AnalyzerLoader
    {
        private static string analyzersDir = Path.Combine(Application.dataPath, "RoslynAnalyzers");
        private const string styleCopAnalyzerDllUrl = "https://github.com/tertle/com.bovinelabs.analyzers/blob/master/RoslynAnalyzers/StyleCopAnalyzers/StyleCop.Analyzers.dll?raw=true";
        private const string styleCopCodeFixesDllUrl = "https://github.com/tertle/com.bovinelabs.analyzers/blob/master/RoslynAnalyzers/StyleCopAnalyzers/StyleCop.Analyzers.CodeFixes.dll?raw=true";

#if UNITY_2020_1_OR_NEWER
        [MenuItem("i5 Toolkit/Load Analyzers")]
#endif
        public static void LoadAnalyzers()
        {
#if !UNITY_2020_1_OR_NEWER
            Debug.LogError("Analyzers are only supported by Unity 2020 or higher.");
#endif

            if (!Directory.Exists(analyzersDir))
            {
                Directory.CreateDirectory(analyzersDir);
            }

            WebClient client = new WebClient();
            string styleCopCodeAnalyzerPath = Path.Combine(analyzersDir, "StyleCop.Analyzers.dll");
            string styleCopCodeFixesPath = Path.Combine(analyzersDir, "StyleCop.Analyzers.CodeFixes.dll");
            client.DownloadFile(styleCopAnalyzerDllUrl, styleCopCodeAnalyzerPath);
            client.DownloadFile(styleCopCodeFixesDllUrl, styleCopCodeFixesPath);

            AssetDatabase.Refresh();

            ImportDll("Assets/RoslynAnalyzers/StyleCop.Analyzers.dll");
            ImportDll("Assets/RoslynAnalyzers/StyleCop.Analyzers.CodeFixes.dll");
        }

        private static void ImportDll(string path)
        {
            PluginImporter importer = AssetImporter.GetAtPath(path) as PluginImporter;
            importer.SetCompatibleWithAnyPlatform(false);
            //importer.SetCompatibleWithEditor(false);
            //importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows, false);
            //importer.SetCompatibleWithPlatform(BuildTarget.Android, false);
            //importer.SetCompatibleWithPlatform(BuildTarget.iOS, false);
            //importer.SetCompatibleWithPlatform(BuildTarget.Lumin, false);

            Object dll = AssetDatabase.LoadMainAssetAtPath(path);
            AssetDatabase.SetLabels(dll, new string[] { "RoslynAnalyzer" });
        }
    }
}