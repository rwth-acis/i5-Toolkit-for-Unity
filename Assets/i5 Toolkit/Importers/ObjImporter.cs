using i5.Toolkit.ProceduralGeometry;
using i5.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace i5.Toolkit.ModelImporters
{
    public static class ObjImporter
    {
        public static bool ExtendedLogging { get; set; } = false;


        /// <summary>
        /// Parses the content of an obj file
        /// </summary>
        /// <param name="contentLines">The content of the obj file, sorted into lines</param>
        public static GeometryConstructor ParseObjText(string[] contentLines)
        {
            // the list of vertices, sorted by the original indices
            // this list is used to look up the correct vertices when the faces are defined
            List<Vector3> vertices = new List<Vector3>();
            // list of normal vectors which is used to look up the correct normals
            List<Vector3> normals = new List<Vector3>();
            // dictionary which collects all vertices with the same index but with different normals in the face definitions
            // they will later need to be separated into different  
            Dictionary<int, List<VertexData>> verticesToNormals = new Dictionary<int, List<VertexData>>();


            GeometryConstructor geometryConstructor = new GeometryConstructor();
            // count the errors in the parsing process
            int numberOfErrors = 0;

            // go through each line
            foreach (string line in contentLines)
            {
                // vertex coordinates are defined with a v (and only a v) at the beginning
                if (line.StartsWith("v "))
                {
                    // TODO: handle Vector4 (there could be 4 coordinates in the obj)
                    Vector3 res;
                    // remove the v at the beginning and then parse to Vector3
                    bool success = ParserUtils.TryParseSpaceSeparatedVector3(line.Substring(1), out res);
                    // we must add the vertex to the list, even if the conversion failed; otherwise the indices will not work
                    vertices.Add(res);

                    if (!success)
                    {
                        if (ExtendedLogging)
                        {
                            Debug.LogError("[ObjImporter] Could not parse vertex definition: " + line);
                        }
                        numberOfErrors++;
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
                    Vector3 res;
                    // remove the vn at the beginning and then parse to Vector3
                    bool success = ParserUtils.TryParseSpaceSeparatedVector3(line.Substring(2), out res);
                    // we must add the normal to the list, even if the conversion failed; otherwise the indices will not work
                    normals.Add(res);

                    if (!success)
                    {
                        if (ExtendedLogging)
                        {
                            Debug.LogError("[ObjImporter] Could not parse normal vector definition: " + line);
                        }
                        numberOfErrors++;
                    }
                }
                // face definitions are defined with an "f" at the beginning
                else if (line.StartsWith("f "))
                {
                    // remove the f and split by spaces to get the individual indices of the face
                    string[] strFaceIndices = line.Substring(1).Trim().Split(' ');
                    // each of the strFaceIndices should contain either a number or the format "1/2/3".
                    // there should be three or four of these indices to define triangles and quads
                    if (strFaceIndices.Length != 3 && strFaceIndices.Length != 4)
                    {
                        Debug.LogError("[ObjImporter] ObjImporter only supports triangles or quad faces. If you use other polygons, triangulate the mesh before importing");
                        numberOfErrors++;
                        continue;
                    }

                    //int[] vertexIndices = new int[strFaceIndices.Length];
                    //int[] vertexUVIndices = new int[strFaceIndices.Length];
                    //int[] vertexNormalIndices = new int[strFaceIndices.Length];

                    // go over all vertex data
                    for (int i = 0; i < strFaceIndices.Length; i++)
                    {
                        VertexData vertexData;
                        bool parseSuccess = TryParseVertexData(strFaceIndices[i], out vertexData);

                        // TODO: construct face and register vertices

                        if (!parseSuccess)
                        {
                            if (ExtendedLogging)
                            {
                                Debug.LogError("[ObjImporter] Unable to parse indices: " + strFaceIndices[i]);
                            }
                            numberOfErrors++;
                        }
                    }

                    if (strFaceIndices.Length == 3)
                    {
                        geometryConstructor.AddTriangle(vertexIndices[0], vertexIndices[1], vertexIndices[2]);
                    }
                    else // it is a quad
                    {
                        geometryConstructor.AddQuad(vertexIndices[0], vertexIndices[1], vertexIndices[2], vertexIndices[3]);
                    }
                    // TODO: add UV and normals to geometryConstructor
                }
                else if (line.StartsWith("#"))
                {
                    if (ExtendedLogging)
                    {
                        Debug.Log("[ObjImporter] Found a comment: " + line.Substring(1));
                    }
                }
            }

            // check if any errors occured and print an error message
            if (numberOfErrors > 0)
            {
                string errorMsg = "[ObjImporter] The process finished with " + numberOfErrors + ". A partial mesh may still have been generated.";
                // if no extended logging was used, notify the developer that extended logging gives more info
                if (!ExtendedLogging)
                {
                    errorMsg += " To see more details, activate extended logging";
                }
                Debug.LogError(errorMsg);
            }

            return geometryConstructor;
        }

        /// <summary>
        /// Tries to parse a string of the format 1/2/3 to a VertexData object
        /// </summary>
        /// <param name="input">The string input which should be parsed</param>
        /// <param name="vertexData">The vertex data result; if conversion failed, this will have default values</param>
        /// <returns>True if the conversion was successful, otherwise false</returns>
        private static bool TryParseVertexData(string input, out VertexData vertexData)
        {
            bool parseSuccess = true;
            string[] strIndices = input.Split(new char[] { '/' }, System.StringSplitOptions.None);

            // in any case: parse the first index
            int vertexIndex;
            parseSuccess &= int.TryParse(strIndices[0], out vertexIndex);
            // correct by one since obj format starts counting at 1 and not 0
            vertexIndex--;

            // if we only have one single number: we are finished
            if (strIndices.Length == 1)
            {
                vertexData = new VertexData(vertexIndex);
                return parseSuccess;
            }

            // also get the other two fields if they exist
            if (parseSuccess && strIndices.Length == 3)
            {
                // parse the last entry
                int normalIndex;
                parseSuccess &= int.TryParse(strIndices[2], out normalIndex);
                // decrease by one since obj format starts counting at 1 and not 0
                normalIndex--;

                // the middle entry might be empty
                if (!string.IsNullOrEmpty(strIndices[1]))
                {
                    int uvIndex;
                    parseSuccess &= int.TryParse(strIndices[1], out uvIndex);
                    // decrease by one since obj format starts counting at 1 and not 0
                    uvIndex--;

                    vertexData = new VertexData(vertexIndex, uvIndex, normalIndex);
                    return parseSuccess;
                }
                // else: no middle entry: UVs not used
                else
                {
                    vertexData = new VertexData(vertexIndex, normalIndex);
                    return parseSuccess;
                }
            }
            // if we do not have a single number and not the format 1/2/3 or 1//2, the format is wrong
            else
            {
                parseSuccess = false;
                vertexData = default;
                return parseSuccess;
            }
        }
    }
}