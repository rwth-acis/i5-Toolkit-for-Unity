using i5.Toolkit.ProceduralGeometry;
using i5.Toolkit.ServiceCore;
using i5.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace i5.Toolkit.ModelImporters
{

    public class ObjImporter : IService
    {
        private char[] splitter = new char[] { ' ' };

        // the id of the pool with GameObjects that are set up with a MeshFilter and Renderer
        private int meshObjectPoolId;

        private MtlLibraryService mtlLibraryService;

        public bool ExtendedLogging { get; set; }

        public void Initialize(ServiceManager owner)
        {
            if (ServiceManager.ServiceExists<MtlLibraryService>())
            {
                mtlLibraryService = ServiceManager.GetService<MtlLibraryService>();
            }
            else
            {
                mtlLibraryService = new MtlLibraryService();
                ServiceManager.RegisterService(mtlLibraryService);
            }

            meshObjectPoolId = ObjectPool<GameObject>.CreateNewPool();
        }

        public void Cleanup()
        {
            ObjectPool<GameObject>.RemovePool(meshObjectPoolId, (go) => { GameObject.Destroy(go); });

            ServiceManager.RemoveService<MtlLibraryService>();
        }

        public async Task<GameObject> ImportAsync(string url)
        {
            i5Debug.Log("Starting import", this);
            Uri uri = new Uri(url);
            Response resp = await FetchModelAsync(uri);

            if (!resp.Successful)
            {
                i5Debug.LogError("Error fetching obj. No object imported.\n" + resp.ResponseBody, this);
                return null;
            }

            GameObject parentObject = ObjectPool<GameObject>.RequestResource(() => { return new GameObject(); });
            parentObject.name = System.IO.Path.GetFileNameWithoutExtension(uri.LocalPath);

            List<ObjParseResult> parseResults = await ParseModelAsync(resp.ResponseBody);

            foreach(ObjParseResult parseResult in parseResults)
            {
                if (!mtlLibraryService.LibraryLoaded(parseResult.LibraryPath))
                {
                    string mtlUri = UriUtils.RewriteUriPath(uri, parseResult.LibraryPath);
                    string libraryName = System.IO.Path.GetFileNameWithoutExtension(uri.LocalPath);
                    bool successful = await mtlLibraryService.LoadLibraryAsyc(new Uri(mtlUri), libraryName);
                    if (!successful)
                    {
                        i5Debug.LogError("Could not load .mtl file " + parseResult.LibraryPath, this);
                    }
                }

                MaterialConstructor mat = mtlLibraryService.GetMaterialConstructor(
                    System.IO.Path.GetFileNameWithoutExtension(uri.LocalPath),
                    parseResult.MaterialName);

                await mat.FetchDependencies();

                parseResult.ObjectConstructor.Material = mat;

                parseResult.ObjectConstructor.ConstructObject(parentObject.transform);
            }
            
            return parentObject;
        }

        private async Task<Response> FetchModelAsync(Uri uri)
        {
            if (!System.IO.Path.GetExtension(uri.LocalPath).Equals(".obj"))
            {
                i5Debug.LogWarning("The given url does not seem to be a .obj file", this);
            }
            Response resp = await Rest.GetAsync(uri.ToString());
            return resp;
        }

        private async Task<List<ObjParseResult>> ParseModelAsync(string objContent)
        {
            List<ObjParseResult> res = await Task.Run(() =>
            {
                string[] contentLines = objContent.Split('\n');
                return ParseObj(contentLines);
            });
            return res;
        }

        private List<ObjParseResult> ParseObj(string[] contentLines)
        {
            List<ObjParseResult> constructedObjects = new List<ObjParseResult>();
            // the list of vertices, sorted by the original indices
            // this list is used to look up the correct vertices when the faces are defined
            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uvCoordinates = new List<Vector2>();
            // list of normal vectors which is used to look up the correct normals
            List<Vector3> normals = new List<Vector3>();
            // dictionary which maps unique vertices (with the same combination of vertex position, uv position and normal vector) to the index in the geometry constructor
            // they will later need to be separated into different  
            Dictionary<VertexData, int> vertexDataToIndex = new Dictionary<VertexData, int>();


            ObjParseResult current = new ObjParseResult();
            string materialLibrary = "";
            // count the errors in the parsing process
            int numberOfErrors = 0;

            // go through each line
            foreach (string line in contentLines)
            {
                // vertex coordinates are defined with a v (and only a v) at the beginning
                if (line.StartsWith("v "))
                {
                    Vector3 res;
                    // remove the v at the beginning and then parse to Vector3
                    bool success = ParserUtils.TryParseSpaceSeparatedVector3(line.Substring(1).Trim(), out res);
                    // we must add the vertex to the list, even if the conversion failed; otherwise the indices will not work
                    vertices.Add(res);

                    if (!success)
                    {
                        if (ExtendedLogging)
                        {
                            i5Debug.LogError("Could not parse vertex definition: " + line, this);
                        }
                        numberOfErrors++;
                    }
                }
                // vertex texture coordinates are defined with a "vt" at the beginning
                else if (line.StartsWith("vt "))
                {
                    Vector2 res;
                    // remove the vt at the beginning and then parse to Vector2
                    bool success = ParserUtils.TryParseSpaceSeparatedVector2(line.Substring(2).Trim(), out res);
                    // we must add the uv coordinates to the list, even if the conversion failed; otherwise the indices will not work
                    // the UV coordinates need to be mirrored on the X axis to show the texture the right way around
                    res = new Vector2(1 - res.x, res.y);
                    uvCoordinates.Add(res);

                    if (!success)
                    {
                        if (ExtendedLogging)
                        {
                            i5Debug.LogError("Could not parse UV coordinate definition: " + line, this);
                            numberOfErrors++;
                        }
                    }
                }
                // vertex normals are defined with a "vn" at the beginning
                else if (line.StartsWith("vn "))
                {
                    Vector3 res;
                    // remove the vn at the beginning and then parse to Vector3
                    bool success = ParserUtils.TryParseSpaceSeparatedVector3(line.Substring(2).Trim(), out res);
                    // we must add the normal to the list, even if the conversion failed; otherwise the indices will not work
                    normals.Add(res);

                    if (!success)
                    {
                        if (ExtendedLogging)
                        {
                            i5Debug.LogError("Could not parse normal vector definition: " + line, this);
                        }
                        numberOfErrors++;
                    }
                }
                // face definitions are defined with an "f" at the beginning
                else if (line.StartsWith("f "))
                {
                    // remove the f and split by spaces to get the individual indices of the face
                    string[] strFaceIndices = line.Substring(1).Trim().Split(splitter, StringSplitOptions.RemoveEmptyEntries);
                    // each of the strFaceIndices should contain either a number or the format "1/2/3".

                    int[] faceIndices = new int[strFaceIndices.Length];

                    // parse all string vertex indices
                    for (int i = 0; i < strFaceIndices.Length; i++)
                    {
                        VertexData vertexData;
                        bool parseSuccess = TryParseVertexData(strFaceIndices[i], out vertexData);

                        // register vertex or get index if vertex already exists
                        if (vertexDataToIndex.ContainsKey(vertexData))
                        {
                            faceIndices[i] = vertexDataToIndex[vertexData];
                        }
                        else
                        {
                            int geometryVertexIndex;
                            // add the vertex in the geometry constructor
                            // if UV index and normal index are defined:
                            if (vertexData.UseUvIndex && vertexData.UseNormalVectorIndex)
                            {
                                geometryVertexIndex = current.ObjectConstructor.GeometryConstructor.AddVertex(
                                vertices[vertexData.vertexIndex],
                                uvCoordinates[vertexData.uvIndex],
                                normals[vertexData.normalVectorIndex]);
                            }
                            // another option is that only the normal vector is defined
                            else if (vertexData.UseNormalVectorIndex)
                            {
                                geometryVertexIndex = current.ObjectConstructor.GeometryConstructor.AddVertex(
                                    vertices[vertexData.vertexIndex],
                                    normals[vertexData.normalVectorIndex]
                                    );
                            }
                            // nothing apart from the vertex coordinates is defined
                            else
                            {
                                geometryVertexIndex = current.ObjectConstructor.GeometryConstructor.AddVertex(
                                    vertices[vertexData.vertexIndex]
                                    );
                            }
                            vertexDataToIndex.Add(vertexData, geometryVertexIndex);
                            faceIndices[i] = geometryVertexIndex;
                        }

                        if (!parseSuccess)
                        {
                            if (ExtendedLogging)
                            {
                                i5Debug.LogError("Unable to parse indices: " + strFaceIndices[i], this);
                            }
                            numberOfErrors++;
                            break;
                        }
                    }

                    if (strFaceIndices.Length == 3) // it is a triangle
                    {
                        current.ObjectConstructor.GeometryConstructor.AddTriangle(faceIndices[0], faceIndices[1], faceIndices[2]);
                    }
                    else if (strFaceIndices.Length == 4) // it is a quad
                    {
                        current.ObjectConstructor.GeometryConstructor.AddQuad(faceIndices[0], faceIndices[1], faceIndices[2], faceIndices[3]);
                    }
                    else
                    {
                        int[] fanIndices = new int[faceIndices.Length - 1];
                        Array.Copy(faceIndices, 1, fanIndices, 0, fanIndices.Length);
                        current.ObjectConstructor.GeometryConstructor.AddTriangleFan(faceIndices[0], fanIndices);
                    }
                }
                else if (line.StartsWith("o "))
                {
                    // object names introduce new objects in Blender exports
                    if (current.ObjectConstructor.GeometryConstructor.Vertices.Count > 0)
                    {
                        // if the existing parse result already has content: add it to the list and start a new one
                        constructedObjects.Add(current);
                        current = new ObjParseResult();
                    }
                    current.ObjectConstructor.GeometryConstructor.Name = line.Substring(1).Trim();
                }
                else if (line.StartsWith("mtllib "))
                {
                    // reference to a material file; save this in the result so that we can query for this outside of the thread later
                    materialLibrary = line.Substring(6).Trim();
                }
                else if (line.StartsWith("usemtl "))
                {
                    // use the last seen library path to find the material
                    current.LibraryPath = materialLibrary;
                    // store the material name
                    current.MaterialName = line.Substring(6).Trim();
                }
                else if (line.StartsWith("#"))
                {
                    if (ExtendedLogging)
                    {
                        i5Debug.Log("Found a comment: " + line.Substring(1), this);
                    }
                }
            }

            if (current.ObjectConstructor.GeometryConstructor.Vertices.Count > 0)
            {
                constructedObjects.Add(current);
            }
            else
            {
                numberOfErrors++;
                i5Debug.LogError("There is an object without parsed vertices", this);
            }

            // check if objects could be constructed, if not: write an error message
            if (constructedObjects.Count == 0)
            {
                numberOfErrors++;
                i5Debug.LogError("No objects could be constructed. Please check if the given file has the right format.", this);
            }

            // check if any errors occured and print an error message
            if (numberOfErrors > 0)
            {
                string warningMsg = "The process finished with " + numberOfErrors + " errors.";
                // if something was created, tell this
                if (constructedObjects.Count > 0)
                {
                    warningMsg += " A partial mesh may still have been generated.";
                }
                // if no extended logging was used, notify the developer that extended logging gives more info
                if (!ExtendedLogging)
                {
                    warningMsg += " To see more details, activate extended logging";
                }
                i5Debug.LogWarning(warningMsg, this);
            }
            else
            {
                i5Debug.Log("Successfully parsed obj file.", this);
            }

            return constructedObjects;
        }

        /// <summary>
        /// Tries to parse a string of the format 1/2/3 to a VertexData object
        /// </summary>
        /// <param name="input">The string input which should be parsed</param>
        /// <param name="vertexData">The vertex data result; if conversion failed, this will have default values</param>
        /// <returns>True if the conversion was successful, otherwise false</returns>
        private bool TryParseVertexData(string input, out VertexData vertexData)
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