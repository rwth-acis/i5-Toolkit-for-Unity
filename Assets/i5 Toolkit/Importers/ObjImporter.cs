using i5.Toolkit.ProceduralGeometry;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace i5.Toolkit.ModelImporters
{
    public static class ObjImporter
    {
        /// <summary>
        /// Parses the content of an obj file
        /// </summary>
        /// <param name="contentLines">The content of the obj file, sorted into lines</param>
        public static GeometryConstructor ParseObjText(string[] contentLines)
        {
            GeometryConstructor geometryConstructor = new GeometryConstructor();

            // go through each line
            foreach (string line in contentLines)
            {
                // vertex coordinates are defined with a v (and only a v) at the beginning
                if (line.StartsWith("v "))
                {
                    bool success = false;
                    // remove the v at the beginning and then split by spaces to get the coordinates
                    string[] strValues = line.Substring(1).Trim().Split(' ');
                    // there should be three coordinates (but can also be four for the w coordinate)
                    if (strValues.Length >= 3)
                    {
                        float vCoord1, vCoord2, vCoord3;
                        // parse the first three coordinates
                        // TODO: handle the case that 4 coordinate are defined
                        if (float.TryParse(strValues[0], NumberStyles.Any, CultureInfo.InvariantCulture, out vCoord1)
                            && float.TryParse(strValues[1], NumberStyles.Any, CultureInfo.InvariantCulture, out vCoord2)
                            && float.TryParse(strValues[2], NumberStyles.Any, CultureInfo.InvariantCulture, out vCoord3))
                        {
                            geometryConstructor.AddVertex(new Vector3(vCoord1, vCoord2, vCoord3));
                            success = true;
                        }
                    }

                    if (!success)
                    {
                        Debug.LogError("[ObjImporter] Could not parse vertex definition line: " + line);
                    }
                }
                // vertex texture coordinates are defined with a "vt" at the beginning
                else if (line.StartsWith("vt "))
                {
                    // TODO: implement UV support for geometry constructor
                }
                // vertex normals are defined with a "vn" at the beginning
                else if (line.StartsWith("vn "))
                {
                    // TODO: implement normal support for geometry constructor
                }
                // face definitions are defined with an "f" at the beginning
                else if (line.StartsWith("f "))
                {
                    // remove the f and split by spaces to get the individual indices of the face
                    string[] vertexData = line.Substring(1).Trim().Split(' ');
                    if (vertexData.Length != 3)
                    {
                        Debug.LogError("[ObjImporter] ObjImporter only supports triangular faces. If you use polygons, triangulate the mesh before importing");
                        continue;
                    }

                    int[] vertexIndices = new int[vertexData.Length];
                    int[] vertexUVIndices = new int[vertexData.Length];
                    int[] vertexNormalIndices = new int[vertexData.Length];

                    // go over all vertex data
                    for (int i = 0; i < vertexData.Length; i++)
                    {
                        bool parseSuccess = true;
                        string[] strIndices = vertexData[i].Split(new char[] { '/' }, System.StringSplitOptions.None);

                        // in any case: parse the first index
                        // tmpParsedIndex: temporary variable which is used to get the index which was just read
                        // tmpParsedIndex is reused for all three indices
                        int tmpParsedIndex;
                        parseSuccess &= int.TryParse(strIndices[0], out tmpParsedIndex);
                        // correct by one since obj format starts counting at 1 and not 0
                        vertexIndices[i] = tmpParsedIndex - 1;

                        // also get the other two fields if they exist
                        if (parseSuccess && strIndices.Length == 3)
                        {
                            // the middle entry might be empty
                            if (!string.IsNullOrEmpty(strIndices[1]))
                            {
                                parseSuccess &= int.TryParse(strIndices[1], out tmpParsedIndex);
                                // correct by one since obj format starts counting at 1 and not 0
                                vertexUVIndices[i] = tmpParsedIndex - 1;
                            }
                            // parse the last entry
                            parseSuccess &= int.TryParse(strIndices[2], out tmpParsedIndex);
                            vertexNormalIndices[i] = tmpParsedIndex - 1;
                        }
                        // if we do not have a single number and not the format 1/2/3 or 1//2, the format is wrong
                        else if (strIndices.Length != 1)
                        {
                            parseSuccess = false;
                        }

                        if (!parseSuccess)
                        {
                            Debug.LogError("[ObjImporter] Unable to parse indices: " + vertexData[i]);
                        }
                    }

                    geometryConstructor.AddTriangle(vertexIndices[0], vertexIndices[1], vertexIndices[2]);
                    // TODO: add UV and normals to geometryConstructor
                }
                else if (line.StartsWith("#"))
                {
                    Debug.Log("[ObjImporter] Found a comment: " + line.Substring(1));
                }
            }

            return geometryConstructor;
        }
    }
}