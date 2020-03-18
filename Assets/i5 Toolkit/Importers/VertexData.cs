using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct VertexData
{
    public int vertexIndex;
    public int uvIndex;
    public int normalVectorIndex;

    public bool UseUvIndex { get; private set; }
    public bool UseNormalVectorIndex { get; private set; }

    public VertexData(int vertexIndex)
    {
        this.vertexIndex = vertexIndex;
        UseUvIndex = false;
        UseNormalVectorIndex = false;
        uvIndex = default;
        normalVectorIndex = default;
    }

    public VertexData(int vertexIndex, int normalVectorIndex)
    {
        this.vertexIndex = vertexIndex;
        UseNormalVectorIndex = true;
        this.normalVectorIndex = normalVectorIndex;
        UseUvIndex = false;
        uvIndex = default;
    }

    public VertexData(int vertexIndex, int uvIndex, int normalVectorIndex)
    {
        this.vertexIndex = vertexIndex;
        UseUvIndex = true;
        this.uvIndex = uvIndex;
        UseNormalVectorIndex = true;
        this.normalVectorIndex = normalVectorIndex;
    }
}
