using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct VertexData
{
    public int vertexIndex;
    public int uvIndex;
    public int normalVectorIndex;

    public bool UseUvIndex { get => uvIndex < 0; }
    public bool UseNormalVectorIndex { get => normalVectorIndex < 0; }

    public VertexData(int vertexIndex)
    {
        this.vertexIndex = vertexIndex;
        uvIndex = -1;
        normalVectorIndex = -1;
    }

    public VertexData(int vertexIndex, int normalVectorIndex)
    {
        this.vertexIndex = vertexIndex;
        this.normalVectorIndex = normalVectorIndex;
        uvIndex = -1;
    }

    public VertexData(int vertexIndex, int uvIndex, int normalVectorIndex)
    {
        this.vertexIndex = vertexIndex;
        this.uvIndex = uvIndex;
        this.normalVectorIndex = normalVectorIndex;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is VertexData)) return false;
        VertexData vd = (VertexData)obj;

        return vertexIndex == vd.vertexIndex
            && uvIndex == vd.uvIndex
            && normalVectorIndex == vd.normalVectorIndex;
    }

    public override int GetHashCode()
    {
        return Tuple.Create(vertexIndex, uvIndex, normalVectorIndex).GetHashCode();
    }
}
