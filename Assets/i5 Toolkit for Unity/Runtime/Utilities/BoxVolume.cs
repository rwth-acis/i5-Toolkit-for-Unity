using i5.Toolkit.Core.Experimental.UnityAdapters;
using UnityEngine;

public class BoxVolume : IBoxVolume
{
    public Vector3 Size { get; set; }
    public Vector3 Center { get; set; }
    public Quaternion Rotation { get; set; }

    public BoxVolume(Vector3 center, Vector3 size) : this(center, size, Quaternion.identity)
    {
    }

    public BoxVolume(Vector3 center, Vector3 size, Quaternion rotation)
    {
        Center = center;
        Size = size;
        Rotation = rotation;
    }
}
