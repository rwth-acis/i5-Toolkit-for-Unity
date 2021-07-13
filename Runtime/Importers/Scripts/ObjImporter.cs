using i5.Toolkit.Core.ProceduralGeometry;
using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities;
using i5.Toolkit.Core.Utilities.ContentLoaders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.ModelImporters
{
    /// <summary>
    /// Service for importing and parsing .obj files
    /// </summary>
    public class ObjImporter : IService
    {
        private char[] splitter = new char[] { ' ' };

        // the id of the pool with GameObjects that are set up with a MeshFilter and Renderer
        private int meshObjectPoolId;

        /// <summary>
        /// instance of the MtlLibrary
        /// </summary>
        public MtlLibrary MtlLibrary { get; private set; }

        /// <summary>
        /// If set to true, additional information, e.g. comments in the .obj file, are logged
        /// </summary>
        public bool ExtendedLogging { get; set; }

        /// <summary>
        /// Module that should be used for fetching the .obj file's content
        /// </summary>
        public IContentLoader<string> ContentLoader { get; set; }

        /// <summary>
        /// Called by the service manager to initialize the service if it is started
        /// </summary>
        /// <param name="owner">The service manager that owns this service</param>
        public void Initialize(IServiceManager owner)
        {
            MtlLibrary = new MtlLibrary();

            // initialize the content loader
            if (ContentLoader == null)
            {
                ContentLoader = new UnityWebRequestLoader();
            }

            // reserve a mesh object pool id
            meshObjectPoolId = ObjectPool<GameObject>.CreateNewPool();
        }

        /// <summary>
        /// Called by the service manager to clean up the service if it is stopped
        /// </summary>
        public void Cleanup()
        {
            // give back the pool
            ObjectPool<GameObject>.RemovePool(meshObjectPoolId, (go) => { GameObject.Destroy(go); });
        }

        /// <summary>
        /// Asynchronously imports the given .obj file from the specified url either from the local file system or the web.
        /// </summary>
        /// <param name="path">The path to the .obj file that is either stored online or on the local file system</param>
        /// <returns>The GameObject that was created for the imported .obj</returns>
        public async Task<GameObject> ImportAsync(string path)
        {
            i5Debug.Log("Starting import", this);
            // fetch the model
            WebResponse<string> resp = await FetchModelAsync(path);

            // if there was an error, we cannot create anything
            if (!resp.Successful)
            {
                i5Debug.LogError("Error fetching obj. No object imported.\n" + resp.ErrorMessage, this);
                return null;
            }

            // create the parent object
            // it is a standard GameObject; its only purpose is to bundle the child objects
            GameObject parentObject = ObjectPool<GameObject>.RequestResource(() => { return new GameObject(); });
            parentObject.name = System.IO.Path.GetFileNameWithoutExtension(path);

            // parse the .obj file
            List<ObjParseResult> parseResults = await ParseModelAsync(resp.Content);

            // for each sub-object in the .obj file, an own parse result was created
            foreach (ObjParseResult parseResult in parseResults)
            {
                // check that the referenced mtl library is already loaded; if not: load it
                if (!MtlLibrary.LibraryLoaded(parseResult.LibraryPath))
                {
                    string mtlUri;
                    // check if the path is a local path or an web uri
                    if (System.IO.File.Exists(path))
                    {
                        string baseDirectory = System.IO.Path.GetDirectoryName(path);
                        string materialPath = parseResult.LibraryPath;
                        // check whether material path is given relative to the obj path or fully qualified
                        if (System.IO.File.Exists(materialPath))
                        {
                            // material path is absolute
                            mtlUri = materialPath;
                        }
                        else
                        {
                            // material path is relative
                            mtlUri = System.IO.Path.Combine(baseDirectory, materialPath);
                        }
                        mtlUri = System.IO.Path.Combine(baseDirectory, materialPath);
                    }
                    else
                    {
                        // try to interprete as web uri
                        Uri uri;
                        // check wether material path is absolute or relative
                        if (Uri.TryCreate(parseResult.LibraryPath, UriKind.Absolute, out uri))
                        {
                            // material path is absolute
                            mtlUri = uri.AbsoluteUri;
                        }
                        else
                        {
                            // material path is relative
                            uri = new Uri(path);
                            mtlUri = UriUtils.RewriteFileUriPath(uri, parseResult.LibraryPath);
                        }
                    }
                    

                    string libraryName = System.IO.Path.GetFileNameWithoutExtension(path);
                    bool successful = await MtlLibrary.LoadLibraryAsyc(new Uri(mtlUri, UriKind.Absolute), libraryName);
                    if (!successful)
                    {
                        i5Debug.LogError("Could not load .mtl file " + parseResult.LibraryPath, this);
                    }
                }

                // get the material constructor of the sub-object
                MaterialConstructor mat = MtlLibrary.GetMaterialConstructor(
                    System.IO.Path.GetFileNameWithoutExtension(path),
                    parseResult.MaterialName);

                if (mat != null)
                {
                    // first get dependencies; this will e.g. fetch referenced textures
                    await mat.FetchDependencies();

                    parseResult.ObjectConstructor.MaterialConstructor = mat;
                }

                // construct the object and make it a child of the parentObject
                parseResult.ObjectConstructor.ConstructObject(parentObject.transform);
            }

            return parentObject;
        }

        /// <summary>
        /// Fetches .obj model
        /// </summary>
        /// <param name="uri">The uri where the .obj file is stored</param>
        /// <returns>Returns a Web Response which contains the contents of the .obj file</returns>
        private async Task<WebResponse<string>> FetchModelAsync(string path)
        {
            if (!System.IO.Path.GetExtension(path).Equals(".obj"))
            {
                i5Debug.LogWarning("The given url does not seem to be a .obj file", this);
            }
            WebResponse<string> resp = await ContentLoader.LoadAsync(path);
            return resp;
        }

        /// <summary>
        /// Parses the model's content
        /// </summary>
        /// <param name="objContent">The content of the .obj file</param>
        /// <returns>Returns a list of parse results; one parse result for each object specified in the file</returns>
        private async Task<List<ObjParseResult>> ParseModelAsync(string objContent)
        {
            // split the content into lines and then parse them
            // do this on a separate thread so that we do not block the main thread
            List<ObjParseResult> res = await Task.Run(() =>
            {
                string[] contentLines = objContent.Split('\n');
                return ParseObj(contentLines);
            });
            return res;
        }

        /// <summary>
        /// Parses the lines of the .obj file
        /// </summary>
        /// <param name="contentLines">The content of the .obj file, separated into lines</param>
        /// <returns>Returns a list of parse results - one parse result for each sub-object specified in the content</returns>
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

            // add the currently edited object if it has content
            if (current.ObjectConstructor.GeometryConstructor.Vertices.Count > 0)
            {
                constructedObjects.Add(current);
            }
            else
            {
                numberOfErrors++;
                i5Debug.LogWarning("There is an object without parsed vertices", this);
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