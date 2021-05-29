using i5.Toolkit.Core.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;

public static class ExternalNugetSetup
{
    private const string packageDirName = "ExternalNugetPackages";

#if !EXTERNAL_NUGET
    [MenuItem("i5 Toolkit/External NuGet Packages/Enable")]
#endif
    public static void EnableExternalNuget()
    {
        CreateNugetConfigFile();
        CreateNugetFolder();

        string existingDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        if (existingDefines.EndsWith(";"))
        {
            existingDefines += ";";
        }
        existingDefines += "EXTERNAL_NUGET;";
        SetDefinesForAllGroups(existingDefines);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

#if EXTERNAL_NUGET
    [MenuItem("i5 Toolkit/External NuGet Packages/Disable")]
#endif
    public static void DisableExternalNuget()
    {
        CleanupNugetConfigFile();

        string existingDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        existingDefines = existingDefines.Replace("EXTERNAL_NUGET;", "");
        existingDefines = existingDefines.Replace("EXTERNAL_NUGET", "");
        SetDefinesForAllGroups(existingDefines);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [MenuItem("i5 Toolkit/External NuGet Packages/Delete")]
    public static void DeleteExternalNuget()
    {
        DisableExternalNuget();

        string externalNugetDir = Path.Combine(Directory.GetCurrentDirectory(), packageDirName);
        if (Directory.Exists(externalNugetDir))
        {
            Directory.Delete(externalNugetDir);
        }
    }

    private static void SetDefinesForAllGroups(string defines)
    {
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, defines);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, defines);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Lumin, defines);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.PS4, defines);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Stadia, defines);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, defines);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Switch, defines);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.tvOS, defines);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.WebGL, defines);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.WSA, defines);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.XboxOne, defines);
    }

    private static void CreateNugetConfigFile()
    {
        string currentDir = Directory.GetCurrentDirectory();
        XDocument document = new XDocument(
            new XElement("configuration",
                new XElement("config",
                    new XElement("add", 
                        new XAttribute("key", "repositoryPath"),
                        new XAttribute("value", $"./{packageDirName}"))
                    )
                )
            );
        document.Save(Path.Combine(currentDir,"nuget.config"));
    }

    private static void CleanupNugetConfigFile()
    {
        string currentDir = Directory.GetCurrentDirectory();
        string nugetConfigPath = Path.Combine(currentDir, "nuget.config");
        if (File.Exists(nugetConfigPath))
        {
            XDocument document = XDocument.Load(nugetConfigPath);
            if (document.Root.Descendants().Count() == 2)
            {
                File.Delete(nugetConfigPath);
            }
            else
            {
                Debug.LogWarning("[ExternalNugetModule] nuget.config file seems to be modified. Therefore it as not deleted.");
            }
        }
    }

    private static void CreateNugetFolder()
    {
        string folderPath = Path.Combine(Directory.GetCurrentDirectory(), packageDirName);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
    }
}
