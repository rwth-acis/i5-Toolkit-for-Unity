#if EXTERNAL_NUGET
using i5.Toolkit.Core.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;

public class ExternalNugetCSPostProcessor : AssetPostprocessor
{
    public override int GetPostprocessOrder()
    {
        return 20;
    }

    public static void OnGeneratedCSProjectFiles()
    {
        try
        {
            string[] csProjEntries = GetCSProjEntries();
            string projectPath = PathUtils.GetProjectPath();

            string[] projectFiles = Directory
            .GetFiles(projectPath, "*.csproj")
            .Where(csprojFile => csProjEntries.Any(line => line
                .Contains("\"" + Path.GetFileName(csprojFile) + "\""))).ToArray();

            foreach (string file in projectFiles)
            {
                AddReferencesToProject(file);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    private static string[] GetCSProjEntries()
    {
        string slnPath = Path.Combine(PathUtils.GetProjectPath(), $"{ProjectUtils.GetProjectName()}.sln");
        if (!File.Exists(slnPath))
        {
            return Array.Empty<string>();
        }

        string[] slnContent = File.ReadAllLines(slnPath);

        List<string> projectEntries = new List<string>();
        foreach (string line in slnContent)
        {
            if (line.StartsWith("Project("))
            {
                projectEntries.Add(line);
            }
        }
        return projectEntries.ToArray();
    }

    private static void AddReferencesToProject(string projectPath)
    {
        XDocument document;

        try
        {
            document = XDocument.Load(projectPath);
        }
        catch (Exception e)
        {
            Debug.LogError($"Could not load {projectPath}: {e}");

            return;
        }

        XElement projectRoot = document.Root;

        if (projectRoot != null)
        {
            XNamespace xmlns = projectRoot.Name.NamespaceName;
            AddNugetReferences(projectRoot, xmlns);
        }

        try
        {
            document.Save(projectPath);
        }
        catch (Exception e)
        {
            Debug.LogError($"Could not save {projectPath}: {e}");
        }
    }

    private static void AddNugetReferences(XContainer projectRoot, XNamespace xmlns)
    {
        string projectPath = PathUtils.GetProjectPath();
        DirectoryInfo externalNugetDir = new DirectoryInfo(Path.Combine(projectPath, ExternalNugetSetup.packageDirName));

        if (!externalNugetDir.Exists)
        {
            Debug.LogError($"The directory {externalNugetDir.Name} does not exist. Please create a file under {externalNugetDir.FullName} or run the external nuget package setup routine.");
            return;
        }

        IEnumerable<string> relativePaths = externalNugetDir
            .GetFiles("*", SearchOption.AllDirectories)
            .Select(x => x.FullName.Substring(projectPath.Length + 1));

        XElement itemGroup = new XElement(xmlns + "ItemGroup");

        foreach (string file in relativePaths)
        {
            if (new FileInfo(file).Extension == ".dll")
            {
                XElement reference = new XElement(xmlns + "Analyzer");
                reference.Add(new XAttribute("Include", file));
                itemGroup.Add(reference);
            }

            if (new FileInfo(file).Extension == ".json")
            {
                XElement reference = new XElement(xmlns + "AdditionalFiles");
                reference.Add(new XAttribute("Include", file));
                itemGroup.Add(reference);
            }

            if (new FileInfo(file).Extension == ".ruleset")
            {
                SetOrUpdateProperty(projectRoot, xmlns, "CodeAnalysisRuleSet", file);
            }
        }

        projectRoot.Add(itemGroup);
    }

    private static void SetOrUpdateProperty(XContainer root, XNamespace xmlns, string name, string content)
    {
        XElement element = root.Elements(xmlns + "PropertyGroup").Elements(xmlns + name).FirstOrDefault();

        if (element != null)
        {
            if (content == element.Value)
            {
                return;
            }

            int currentSubDirectoryCount = Regex.Matches(element.Value, "/").Count;
            int newSubDirectoryCount = Regex.Matches(content, "/").Count;

            if (currentSubDirectoryCount != 0 && currentSubDirectoryCount < newSubDirectoryCount)
            {
                return;
            }

            element.SetValue(content);
        }
        else
        {
            XElement propertyGroup = root.Elements(xmlns + "PropertyGroup")
                .FirstOrDefault(elem => !elem.Attributes(xmlns + "Condition").Any());

            if (propertyGroup == null)
            {
                propertyGroup = new XElement(xmlns + "PropertyGroup");

                root.AddFirst(propertyGroup);
            }

            propertyGroup.Add(new XElement(xmlns + name, string.Empty));
        }
    }
}
#endif