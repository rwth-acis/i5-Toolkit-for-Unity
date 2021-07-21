using i5.Toolkit.Core.ProceduralGeometry;
using i5.Toolkit.Core.Utilities;
using i5.Toolkit.Core.Utilities.ContentLoaders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.ModelImporters
{
    /// <summary>
    /// Class for parsing and storing material libraries from .mtl files
    /// </summary>
    public class MtlLibrary : IMtlLibrary
    {
        /// <summary>
        /// Dictionary of loaded material libraries
        /// </summary>
        private Dictionary<string, Dictionary<string, MaterialConstructor>> libraries;

        /// <summary>
        /// If set to true, the service will log additional information, e.g. comments in the .mtl file
        /// </summary>
        public bool ExtendedLogging { get; set; }

        /// <summary>
        /// Gets or sets the module which loads the .mtl files
        /// </summary>
        public IContentLoader<string> ContentLoader { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MtlLibrary()
        {
            libraries = new Dictionary<string, Dictionary<string, MaterialConstructor>>();
            ContentLoader = new UnityWebRequestLoader();
        }

        /// <summary>
        /// Returns a material from a library as a material constructor instance
        /// </summary>
        /// <param name="materialLibrary">The library name which contains the material</param>
        /// <param name="materialName">The name of the material in the library</param>
        /// <returns>Returns a material constructor that resembles the material;
        /// returns null if the libarary or material does not exist</returns>
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

        /// <summary>
        /// Returns true if the library with the given name was loaded
        /// </summary>
        /// <param name="name">The name of the library</param>
        /// <returns>Returns true if the library was loaded</returns>
        public bool LibraryLoaded(string name)
        {
            return libraries.ContainsKey(name);
        }

        /// <summary>
        /// Asynchronously loads a material library from the specified URI
        /// </summary>
        /// <param name="absolutePath">Absolute path or uri to the .mtl file</param>
        /// <param name="libraryName">The name of the library</param>
        /// <returns>Returns true if the library could be loaded</returns>
        public async Task<bool> LoadLibraryAsyc(string absolutePath, string libraryName)
        {
            // if we have already cached the library, do not load it again
            if (libraries.ContainsKey(libraryName))
            {
                i5Debug.LogWarning("Library " + libraryName + " was already loaded", this);
                return true;
            }

            WebResponse<string> matLibResponse = await ContentLoader.LoadAsync(absolutePath);
            if (matLibResponse.Successful)
            {
                // we now have the entire content of the library
                string[] libraryContent = matLibResponse.Content.Split(
                    new[] { "\r\n", "\r", "\n" },
                    StringSplitOptions.None);
                // parse the lines to get a list of MaterialData which are described in the library
                List<MaterialConstructor> materialsInLibrary = ParseMaterialLibrary(absolutePath, libraryContent);
                // create the entry for the material library
                libraries.Add(libraryName, new Dictionary<string, MaterialConstructor>());
                // create the material entries for each material in the library
                foreach (MaterialConstructor mc in materialsInLibrary)
                {
                    libraries[libraryName].Add(mc.Name, mc);
                }
                return true;
            }
            else
            {
                i5Debug.LogError(matLibResponse.ErrorMessage, this);
                return false;
            }
        }

        /// <summary>
        /// Parses the contents of a .mtl file
        /// </summary>
        /// <param name="absolutePath">The absolute path or uri where the .mtl file is stored</param>
        /// <param name="libraryContent">The line array of the file's content</param>
        /// <returns>A list a material constructor for each material in the library</returns>
        private List<MaterialConstructor> ParseMaterialLibrary(string absolutePath, string[] libraryContent)
        {
            List<MaterialConstructor> materials = new List<MaterialConstructor>();
            MaterialConstructor current = null;
            int numberOfErrors = 0;

            for (int i = 0; i < libraryContent.Length; i++)
            {
                string trimmedLine = libraryContent[i].Trim();

                // newmtl and # (comment) can be executed also if there is no current material set
                // skip empty lines
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
                            // rewrite the URI to the texture's path and then add a texture constructor to the material
                            string fullPath = PathUtils.RewriteToAbsolutePath(absolutePath, texturePath);
                            current.SetTexture("_MainTex", new TextureConstructor(fullPath));
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

            // check if materials were created
            if (materials.Count == 0)
            {
                i5Debug.LogWarning(".mtl-file was read but no materials were parsed.", this);
            }

            return materials;
        }
    }
}