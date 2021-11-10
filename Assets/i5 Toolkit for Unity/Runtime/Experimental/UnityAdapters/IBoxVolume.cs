using UnityEngine;

namespace i5.Toolkit.Core.Experimental.UnityAdapters
{
    public interface IBoxVolume
    {
        Vector3 Size { get; set; }
        Vector3 Center { get; set; }
        Quaternion Rotation { get; set; }
    }
}