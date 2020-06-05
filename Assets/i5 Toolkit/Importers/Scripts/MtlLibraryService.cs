using i5.Toolkit.ServiceCore;
using i5.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using UnityEngine;

public class MtlLibraryService : IService
{
    private Dictionary<string, Dictionary<string, MaterialConstructor>> libraries;

    public bool ExtendedLogging { get; set; }

    public void Cleanup()
    {
    }

    public void Initialize(ServiceManager owner)
    {
        libraries = new Dictionary<string, Dictionary<string, MaterialConstructor>>();
    }

    public MaterialConstructor GetMaterialConstructor(string materialLibrary, string materialName)
    {
        // if either the library or the material in the library does not exist: return null
        if (!libraries.ContainsKey(materialLibrary) || !libraries[materialLibrary].ContainsKey(materialName))
        {
            return null;
        }
        else
        {
            return libraries[materialLibrary][materialName];
        }
    }

    public bool LibraryLoaded(string name)
    {
        return libraries.ContainsKey(name);
    }

    public async Task<bool> LoadLibraryAsyc(Uri uri, string libraryName)
    {
        // if we have already cached the library, do not load it again
        if (libraries.ContainsKey(libraryName))
        {
            return true;
        }

        Response matLibResponse = await Rest.GetAsync(uri.ToString());
        if (matLibResponse.Successful)
        {
            // we now have the entire content of the library
            string[] libraryContent = matLibResponse.ResponseBody.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None);
            // parse the lines to get a list of MaterialData which are described in the library
            List<MaterialConstructor> materialsInLibrary = ParseMaterialLibrary(uri, libraryContent);
            // create the entry for the material library
            libraries.Add(libraryName, new Dictionary<string, MaterialConstructor>());
            // create the material entries for each material in the library
            foreach(MaterialConstructor mc in materialsInLibrary)
            {
                libraries[libraryName].Add(mc.Name, mc);
            }
            return true;
        }
        else
        {
            i5Debug.LogError(matLibResponse.ResponseBody, this);
            return false;
        }
    }

    private List<MaterialConstructor> ParseMaterialLibrary(Uri uri, string[] libraryContent)
    {
        List<MaterialConstructor> materials = new List<MaterialConstructor>();
        MaterialConstructor current = null;
        int numberOfErrors = 0;

        for (int i = 0; i < libraryContent.Length; i++)
        {
            string trimmedLine = libraryContent[i].Trim();

            // newmtl and # (comment) can be executed also if there is no current material set
            if (string.IsNullOrEmpty(trimmedLine))
            {
                continue;
            }
            else if (trimmedLine.StartsWith("newmtl"))
            {
                if (current != null)
                {
                    materials.Add(current);
                }
                current = new MaterialConstructor();
                current.Name = trimmedLine.Substring(6).TrimStart();
            }
            else if (trimmedLine.StartsWith("#"))
            {
                if (ExtendedLogging)
                {
                    i5Debug.Log("Comment found: " + trimmedLine.Substring(1).TrimStart(), this);
                }
            }
            // all other commands require an already initialized material
            // this means that the line newmtl must have been read at least once until now
            else
            {
                if (current != null)
                {
                    // Kd sets the diffuse color
                    if (trimmedLine.StartsWith("Kd"))
                    {
                        string[] strValues = trimmedLine.Substring(2).TrimStart().Split(' ');
                        if (strValues.Length != 3)
                        {
                            numberOfErrors++;
                            if (ExtendedLogging)
                            {
                                i5Debug.LogError("Expected three color values but found " + strValues.Length, this);
                            }
                            continue;
                        }
                        if (ParserUtils.TryParseStringArrayToVector3(strValues, out Vector3 colorVector))
                        {
                            // could successfully parse color vector
                            Color albedo = colorVector.ToColor();
                            current.Color = albedo;
                        }
                        else
                        {
                            numberOfErrors++;
                            if (ExtendedLogging)
                            {
                                i5Debug.LogError("Could not parse color data", this);
                            }
                        }
                    }
                    // Ks sets the specular intensity
                    else if (trimmedLine.StartsWith("Ks"))
                    {
                        string[] strValues = trimmedLine.Substring(2).TrimStart().Split(' ');
                        if (strValues.Length != 3 && strValues.Length != 1)
                        {
                            numberOfErrors++;
                            if (ExtendedLogging)
                            {
                                i5Debug.LogError("Expected one or three smoothness values but found " + strValues.Length, this);
                            }
                            continue;
                        }
                        // we assume that all values are equal
                        if (float.TryParse(strValues[0], NumberStyles.Any, CultureInfo.InvariantCulture, out float smoothness))
                        {
                            // could successfully parse smoothness
                            current.SetFloat("_Glossiness", smoothness);
                        }
                        else
                        {
                            numberOfErrors++;
                            if (ExtendedLogging)
                            {
                                i5Debug.LogError("Could not parse color data", this);
                            }
                        }
                    }
                    // map_Kd sets the albedo texture
                    else if (trimmedLine.StartsWith("map_Kd"))
                    {
                        string texturePath = trimmedLine.Substring(6).TrimStart();
                        string fullUri = UriUtils.RewriteUriPath(uri, texturePath);
                        current.SetTexture("_MainTex", new TextureConstructor(fullUri));
                    }
                }
                else
                {
                    i5Debug.LogWarning("Material instruction line found but material has not yet been initialized. The .mtl-file is probably malformed.", this);
                }
            }
        }

        if (current != null)
        {
            materials.Add(current);
        }

        if (materials.Count == 0)
        {
            i5Debug.LogWarning(".mtl-file was read but no materials were parsed.", this);
        }

        return materials;
    }
}
