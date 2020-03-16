using i5.Toolkit.ProceduralGeometry;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.ModelImporters
{
    public static class ObjImporter
    {
        /// <summary>
        /// Parses the content of an obj file
        /// </summary>
        /// <param name="contentLines">The content of the obj file, sorted into lines</param>
        private static void ParseObjText(string[] contentLines)
        {
            GeometryConstructor geometryConstructor = new GeometryConstructor();

            // go through each line
            foreach (string line in contentLines)
            {
                // vertex coordinates are defined with a v (and only a v) at the beginning
                if (line.StartsWith("v "))
                {
                    bool success = false;
                    string[] strValues = line.Split(' ');
                    // there should be three coordinates (but can also be four for the w coordinate)
                    if (strValues.Length >= 3)
                    {
                        float vCoord1, vCoord2, vCoord3;
                        // parse the first three coordinates
                        // TODO: handle the case that 4 coordinate are defined
                        if (float.TryParse(strValues[0], out vCoord1) && float.TryParse(strValues[1], out vCoord2) && float.TryParse(strValues[2], out vCoord3))
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
                    string[] vertexData = line.Split(' ');
                    if (vertexData.Length != 3)
                    {
                        Debug.LogError("[ObjImporter] ObjImporter only supports triangular faces. If you use polygons, triangulate the mesh before importing");
                        continue;
                    }

                    int[] vertexIndices = new int[vertexData.Length];
                    int[] vertexUVIndices = new int[vertexData.Length];
                    int[] vertexNormalIndices = new int[vertexData.Length];

                    for (int i = 0; i < vertexData.Length; i++)
                    {
                        string[] strIndices = vertexData[i].Split(new char[] { '/' }, System.StringSplitOptions.None);
                        // if no "/" => we have a single number which is the vertex index
                        if (strIndices.Length == 1)
                        {

                        }
                    }
                }
            }
        }
    }
}