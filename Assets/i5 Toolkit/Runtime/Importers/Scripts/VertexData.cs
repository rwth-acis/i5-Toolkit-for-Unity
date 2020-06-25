using System;

namespace i5.Toolkit.Core.ModelImporters
{
    /// <summary>
    /// Stores data for one vertex
    /// </summary>
    public struct VertexData
    {
        /// <summary>
        /// The index of the vertex
        /// </summary>
        public int vertexIndex;
        /// <summary>
        /// The index in the UV layout
        /// </summary>
        public int uvIndex;
        /// <summary>
        /// The index in the normal vector array
        /// </summary>
        public int normalVectorIndex;

        /// <summary>
        /// True if the UV index is set
        /// </summary>
        public bool UseUvIndex { get => uvIndex >= 0; }
        /// <summary>
        /// True if the normal vector index is set
        /// </summary>
        public bool UseNormalVectorIndex { get => normalVectorIndex >= 0; }

        /// <summary>
        /// Creates a vertex data object with the vertex index only
        /// </summary>
        /// <param name="vertexIndex">The index of the vertex</param>
        public VertexData(int vertexIndex)
        {
            this.vertexIndex = vertexIndex;
            uvIndex = -1;
            normalVectorIndex = -1;
        }

        /// <summary>
        /// Creates a vertex data object where the vertex index and normal vector index are set
        /// </summary>
        /// <param name="vertexIndex">The index of the vertex</param>
        /// <param name="normalVectorIndex">The index of the vertex in the normal vector array</param>
        public VertexData(int vertexIndex, int normalVectorIndex)
        {
            this.vertexIndex = vertexIndex;
            this.normalVectorIndex = normalVectorIndex;
            uvIndex = -1;
        }

        /// <summary>
        /// Creates a vertex data object where the vertex index, uv index and normal vector index are set
        /// </summary>
        /// <param name="vertexIndex">The index of the vertex</param>
        /// <param name="uvIndex">The index of the vertex in the UV layout</param>
        /// <param name="normalVectorIndex">The index of the vertex in the normal vector array</param>
        public VertexData(int vertexIndex, int uvIndex, int normalVectorIndex)
        {
            this.vertexIndex = vertexIndex;
            this.uvIndex = uvIndex;
            this.normalVectorIndex = normalVectorIndex;
        }

        /// <summary>
        /// Checks if two vertex data objects are equal
        /// </summary>
        /// <param name="obj">The other vertex data object</param>
        /// <returns>True if the vertex data objects resemble the same vertex</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is VertexData)) return false;
            VertexData vd = (VertexData)obj;

            return vertexIndex == vd.vertexIndex
                && uvIndex == vd.uvIndex
                && normalVectorIndex == vd.normalVectorIndex;
        }

        /// <summary>
        /// Gets a hash code for the vertex data object
        /// </summary>
        /// <returns>Returns a hash code for the vertex data object</returns>
        public override int GetHashCode()
        {
            return Tuple.Create(vertexIndex, uvIndex, normalVectorIndex).GetHashCode();
        }
    }
}