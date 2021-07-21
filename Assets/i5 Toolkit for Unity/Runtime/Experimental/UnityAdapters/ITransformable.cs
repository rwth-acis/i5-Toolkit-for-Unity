using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.Experimental.UnityAdapters
{
    public interface ITransformable
    {
        Vector3 Position { get; set; }

        Vector3 LocalPosition { get; set; }

        Quaternion Rotation { get; set; }

        Quaternion LocalRotation { get; set; }

        Vector3 EulerAngles { get; set; }

        Vector3 LocalEulerAngles { get; set; }

        Vector3 LocalScale { get; set; }

        Vector3 LossyScale { get; }
    }
}