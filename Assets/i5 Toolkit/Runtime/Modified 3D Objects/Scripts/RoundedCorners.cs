using i5.Toolkit.Core.ProceduralGeometry;
using i5.Toolkit.Core.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.Modified3DObjects
{
    /// <summary>
    /// Constructs a rounded 3D rectangle with depth
    /// </summary>
    [RequireComponent(typeof(MeshFilter), typeof(BoxCollider), typeof(MeshCollider))]
    [ExecuteAlways]
    public class RoundedCorners : MonoBehaviour
    {
        [Tooltip("The width of the element")]
        public float width = 1f;
        [Tooltip("The height of the element")]
        public float height = 1f;
        [Tooltip("The depth of the element")]
        public float depth = 0.01f;
        [Tooltip("The corner radius of the element")]
        public float cornerRadius = 0.1f;
        [Tooltip("Number of subdivisions for the rounded corners")]
        public int subdivisions = 3;
        [Tooltip("Uses an exact mesh collider instead of the box collider. Should only be used if really necessary.")]
        public bool exactColliders = false;

        private MeshFilter meshFilter;
        private BoxCollider boxCollider;
        private MeshCollider meshCollider;

        /// <summary>
        /// The corner radius which is actually used
        /// Equal to cornerRadius whenever possible
        /// </summary>
        private float realCornerRadius;

        /// <summary>
        /// Flag which is set if the settings have been changed
        /// </summary>
        private bool settingsChanged;

        /// <summary>
        /// Generates the mesh based on the settings of the menu
        /// </summary>
        /// <returns></returns>
        private Mesh GenerateMesh()
        {
            GeometryConstructor constructor = new GeometryConstructor();

            // vertex positions
            // calculate positions of inner four vertices
            Vector3 leftTopInner = new Vector3(-width / 2f + realCornerRadius, height / 2f - realCornerRadius, 0);
            Vector3 leftBottomInner = new Vector3(-width / 2f + realCornerRadius, -height / 2f + realCornerRadius, 0);
            Vector3 rightTopInner = new Vector3(width / 2f - realCornerRadius, height / 2f - realCornerRadius, 0);
            Vector3 rightBottomInner = new Vector3(width / 2f - realCornerRadius, -height / 2f + realCornerRadius, 0);

            // calculate positions of outer vertices (not part of the rounded corners)
            Vector3 leftTopOuterLeft = leftTopInner - new Vector3(realCornerRadius, 0, 0);
            Vector3 leftTopOuterTop = leftTopInner + new Vector3(0, realCornerRadius, 0);
            Vector3 leftBottomOuterLeft = leftBottomInner - new Vector3(realCornerRadius, 0, 0);
            Vector3 leftBottomOuterBottom = leftBottomInner - new Vector3(0, realCornerRadius, 0);
            Vector3 rightTopOuterRight = rightTopInner + new Vector3(realCornerRadius, 0, 0);
            Vector3 rightTopOuterTop = rightTopInner + new Vector3(0, realCornerRadius, 0);
            Vector3 rightBottomOuterRight = rightBottomInner + new Vector3(realCornerRadius, 0, 0);
            Vector3 rightBottomOuterBottom = rightBottomInner - new Vector3(0, realCornerRadius, 0);

            // calculate positions of the front vertices for the rounded corners
            // back vertices can be calculated from this later
            Vector3[] leftTopCorner = GetCornerVertexCoordinates(leftTopInner, 0f);
            Vector3[] rightTopCorner = GetCornerVertexCoordinates(rightTopInner, 90f);
            Vector3[] rightBottomCorner = GetCornerVertexCoordinates(rightBottomInner, 180f);
            Vector3[] leftBottomCorner = GetCornerVertexCoordinates(leftBottomInner, 270f);

            // create the areas twice: once for the front and once for the back of the menu
            for (int i = 0; i < 2; i++)
            {
                Vector3 depthOffset = new Vector3(0, 0, i * depth);

                // get indices for inner four vertices
                int iLeftTopInner = constructor.AddVertex(leftTopInner + depthOffset);
                int iLeftBottomInner = constructor.AddVertex(leftBottomInner + depthOffset);
                int iRightTopInner = constructor.AddVertex(rightTopInner + depthOffset);
                int iRightBottomInner = constructor.AddVertex(rightBottomInner + depthOffset);

                // get indices for outer vertices
                int iLeftTopOuterLeft = constructor.AddVertex(leftTopOuterLeft + depthOffset);
                int iLeftTopOuterTop = constructor.AddVertex(leftTopOuterTop + depthOffset);
                int iLeftBottomOuterLeft = constructor.AddVertex(leftBottomOuterLeft + depthOffset);
                int iLeftBottomOuterBottom = constructor.AddVertex(leftBottomOuterBottom + depthOffset);
                int iRightTopOuterRight = constructor.AddVertex(rightTopOuterRight + depthOffset);
                int iRightTopOuterTop = constructor.AddVertex(rightTopOuterTop + depthOffset);
                int iRightBottomOuterRight = constructor.AddVertex(rightBottomOuterRight + depthOffset);
                int iRightBottomOuterBottom = constructor.AddVertex(rightBottomOuterBottom + depthOffset);

                bool isBackFace = (i == 1);

                // create inner quad
                constructor.AddQuad(iLeftTopInner, iRightTopInner, iRightBottomInner, iLeftBottomInner, isBackFace);
                // create outer border
                constructor.AddQuad(iLeftTopOuterTop, iRightTopOuterTop, iRightTopInner, iLeftTopInner, isBackFace);
                constructor.AddQuad(iRightTopInner, iRightTopOuterRight, iRightBottomOuterRight, iRightBottomInner, isBackFace);
                constructor.AddQuad(iLeftBottomInner, iRightBottomInner, iRightBottomOuterBottom, iLeftBottomOuterBottom, isBackFace);
                constructor.AddQuad(iLeftTopOuterLeft, iLeftTopInner, iLeftBottomInner, iLeftBottomOuterLeft, isBackFace);

                // create the rounded corners
                CreateCorner(constructor, iLeftTopInner, iLeftTopOuterLeft, iLeftTopOuterTop, leftTopCorner, isBackFace);
                CreateCorner(constructor, iRightTopInner, iRightTopOuterTop, iRightTopOuterRight, rightTopCorner, isBackFace);
                CreateCorner(constructor, iRightBottomInner, iRightBottomOuterRight, iRightBottomOuterBottom, rightBottomCorner, isBackFace);
                CreateCorner(constructor, iLeftBottomInner, iLeftBottomOuterBottom, iLeftBottomOuterLeft, leftBottomCorner, isBackFace);
            }

            // create rim vertex indices
            // these vertices need to be separate from the ones above, even if they have the same coordinates to create sharp edges
            int[] rimLeftTopOuterLeft = new int[2];
            int[] rimLeftTopOuterTop = new int[2];
            int[] rimLeftBottomOuterLeft = new int[2];
            int[] rimLeftBottomOuterBottom = new int[2];
            int[] rimRightTopOuterRight = new int[2];
            int[] rimRightTopOuterTop = new int[2];
            int[] rimRightBottomOuterRight = new int[2];
            int[] rimRightBottomOuterBottom = new int[2];

            for (int i = 0; i < 2; i++)
            {
                Vector3 depthOffset = new Vector3(0, 0, i * depth);

                rimLeftTopOuterLeft[i] = constructor.AddVertex(leftTopOuterLeft + depthOffset);
                rimLeftTopOuterTop[i] = constructor.AddVertex(leftTopOuterTop + depthOffset);
                rimLeftBottomOuterLeft[i] = constructor.AddVertex(leftBottomOuterLeft + depthOffset);
                rimLeftBottomOuterBottom[i] = constructor.AddVertex(leftBottomOuterBottom + depthOffset);
                rimRightTopOuterRight[i] = constructor.AddVertex(rightTopOuterRight + depthOffset);
                rimRightTopOuterTop[i] = constructor.AddVertex(rightTopOuterTop + depthOffset);
                rimRightBottomOuterRight[i] = constructor.AddVertex(rightBottomOuterRight + depthOffset);
                rimRightBottomOuterBottom[i] = constructor.AddVertex(rightBottomOuterBottom + depthOffset);
            }

            // top rim
            constructor.AddQuad(rimLeftTopOuterTop[1], rimRightTopOuterTop[1], rimRightTopOuterTop[0], rimLeftTopOuterTop[0]);
            // right rim
            constructor.AddQuad(rimRightTopOuterRight[0], rimRightTopOuterRight[1], rimRightBottomOuterRight[1], rimRightBottomOuterRight[0]);
            // bottom rim
            constructor.AddQuad(rimLeftBottomOuterBottom[0], rimRightBottomOuterBottom[0], rimRightBottomOuterBottom[1], rimLeftBottomOuterBottom[1]);
            // left rim
            constructor.AddQuad(rimLeftTopOuterLeft[1], rimLeftTopOuterLeft[0], rimLeftBottomOuterLeft[0], rimLeftBottomOuterLeft[1]);

            // rim of the corners
            CreateCornerRim(constructor, rimLeftTopOuterLeft, leftTopCorner, rimLeftTopOuterTop);
            CreateCornerRim(constructor, rimRightTopOuterTop, rightTopCorner, rimRightTopOuterRight);
            CreateCornerRim(constructor, rimRightBottomOuterRight, rightBottomCorner, rimRightBottomOuterBottom);
            CreateCornerRim(constructor, rimLeftBottomOuterBottom, leftBottomCorner, rimLeftBottomOuterLeft);


            return constructor.ConstructMesh();
        }

        /// <summary>
        /// Calculates the corner vertex coordinates
        /// </summary>
        /// <param name="innerVertex">The position of the midpoint of the corner circle; vertices are rotated around this vertex</param>
        /// <param name="angleOffset">Determines at which angle the vertices start (0 for top left, 90 for top right, 180 for bottom right, 270 for bottom left)</param>
        /// <returns>The positions of the vertices which describe the rounded corner</returns>
        private Vector3[] GetCornerVertexCoordinates(Vector3 innerVertex, float angleOffset)
        {
            Vector3[] cornerVertices = new Vector3[subdivisions];
            for (int i = 0; i < subdivisions; i++)
            {
                float angleStep = 90f / (subdivisions + 1) * (i + 1);
                float radianAngle = Mathf.Deg2Rad * (angleStep + angleOffset - 90f); // -90 correction so that 0 degrees offset is for left top corner
                cornerVertices[i] = innerVertex + realCornerRadius * new Vector3(Mathf.Sin(radianAngle), Mathf.Cos(radianAngle), 0);
            }
            return cornerVertices;
        }

        /// <summary>
        /// Creates the rounded corner in the mesh constructor
        /// </summary>
        /// <param name="constructor">The mesh constructor to which the geometry of the corner should be added</param>
        /// <param name="innerVertex">The index of the inner vertex around which the corner is rotated</param>
        /// <param name="outerVertex1">The clockwise first outer vertex to which the rounded corner is connected</param>
        /// <param name="outerVertex2">The clockwise second outer vertex to which the rounded corner is connected</param>
        /// <param name="subdivisionCoordinates">The front coorindates of the vertices of the rounded corner</param>
        /// <param name="isBackFace">True if the back faces should be generated</param>
        private void CreateCorner(GeometryConstructor constructor, int innerVertex, int outerVertex1, int outerVertex2, Vector3[] subdivisionCoordinates, bool isBackFace)
        {
            // triangle fan must include existing endpoint vertices => two more entries in vertex indices
            int[] iCornerVertices = new int[subdivisions + 2];
            // determine at which depth the vertices should be placed
            Vector3 depthVector = isBackFace ? new Vector3(0, 0, depth) : Vector3.zero;
            // create the vertices and write the indices to the index array
            iCornerVertices[0] = outerVertex1;
            for (int i = 0; i < subdivisionCoordinates.Length; i++)
            {
                iCornerVertices[i + 1] = constructor.AddVertex(subdivisionCoordinates[i] + depthVector);
            }
            iCornerVertices[iCornerVertices.Length - 1] = outerVertex2;

            // create the triangle fan which represents the rounded corner
            constructor.AddTriangleFan(innerVertex, iCornerVertices, isBackFace);
        }

        /// <summary>
        /// Creates and adds the rim of a rounded corner to the geometry constructor
        /// </summary>
        /// <param name="constructor">The geometry constructor to which the rim of the corner should be added</param>
        /// <param name="endpoints1">Array of size two, containing the vertex index of the first endpoints (seen clockwise from the front)
        /// first entry: front vertex index
        /// second entry: back vertex index</param>
        /// <param name="cornerVertexCoordinates">Front vertex coordinates of the rounded corner</param>
        /// <param name="endpoints2">Array of size two, containing the vertex index of the second endpoints (seen clockwise from the front)
        /// first entry: front vertex index
        /// second entry: back vertex index</param>
        private void CreateCornerRim(GeometryConstructor constructor, int[] endpoints1, Vector3[] cornerVertexCoordinates, int[] endpoints2)
        {
            if (endpoints1.Length != 2 || endpoints2.Length != 2)
            {
                i5Debug.LogError("Expected endpoints array size of 2 but got " + endpoints1.Length + " and " + endpoints2.Length, this);
                return;
            }

            // generate vertex indices
            int[] frontCornerVertices = new int[subdivisions];
            int[] backCornerVertices = new int[subdivisions];
            Vector3 depthVector = new Vector3(0, 0, depth);
            for (int i = 0; i < subdivisions; i++)
            {
                frontCornerVertices[i] = constructor.AddVertex(cornerVertexCoordinates[i]);
                backCornerVertices[i] = constructor.AddVertex(cornerVertexCoordinates[i] + depthVector);
            }

            // connect top rim to first corner segment
            constructor.AddQuad(endpoints1[1], backCornerVertices[0], frontCornerVertices[0], endpoints1[0]);
            // connect corner segments
            for (int i = 0; i < subdivisions - 1; i++)
            {
                constructor.AddQuad(backCornerVertices[i], backCornerVertices[i + 1], frontCornerVertices[i + 1], frontCornerVertices[i]);
            }
            // connect last corner segment to right rim
            constructor.AddQuad(backCornerVertices[subdivisions - 1], endpoints2[1], endpoints2[0], frontCornerVertices[subdivisions - 1]);
        }

        private void AdaptCollider()
        {
            if (exactColliders)
            {
                ComponentUtilities.EnsureComponentReference(gameObject, ref meshCollider, true);
                meshCollider.convex = true;
                meshCollider.enabled = true;
                if (boxCollider != null)
                {
                    boxCollider.enabled = false;
                    boxCollider.center = Vector3.zero;
                    boxCollider.size = 0.00001f * Vector3.one;
                }
                meshCollider.sharedMesh = meshFilter.sharedMesh;
            }
            else
            {
                ComponentUtilities.EnsureComponentReference(gameObject, ref boxCollider, true);
                boxCollider.enabled = true;
                if (meshCollider != null)
                {
                    meshCollider.enabled = false;
                }
                boxCollider.size = new Vector3(width, height, depth);
                boxCollider.center = new Vector3(0, 0, depth / 2f);
            }
        }

        /// <summary>
        /// Checks if the input values are in the correct range
        /// If not, the values are corrected
        /// </summary>
        private void CheckValues()
        {
            width = Mathf.Max(0, width);
            height = Mathf.Max(0, height);
            depth = Mathf.Max(0, depth);
            cornerRadius = Mathf.Max(0, cornerRadius);
            subdivisions = Mathf.Max(1, subdivisions);
            realCornerRadius = Mathf.Clamp(cornerRadius, 0, Mathf.Min(width, height) / 2f);
        }

#if UNITY_EDITOR

        /// <summary>
        /// Called in the editor if a value in the inspector is changed
        /// </summary>
        private void OnValidate()
        {
            // only update object if it is in a scene, otherwise it will also update prefabs causing infinite loops
            if (gameObject.activeInHierarchy)
            {
                settingsChanged = true;
            }
        }

#endif

        /// <summary>
        /// Called by Unity every frame and on every change in the Unity editor
        /// </summary>
        private void Update()
        {
            // if something was changed: updat the mesh and colliders
            if (settingsChanged)
            {
                settingsChanged = false;

                CheckValues();
                ComponentUtilities.EnsureComponentReference(gameObject, ref meshFilter, true);
                meshFilter.sharedMesh = GenerateMesh();
                AdaptCollider();
            }
        }
    }
}