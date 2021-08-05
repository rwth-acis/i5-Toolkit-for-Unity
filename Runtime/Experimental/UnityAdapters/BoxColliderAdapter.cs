using UnityEngine;

namespace i5.Toolkit.Core.Experimental.UnityAdapters
{
    public class BoxColliderAdapter : IBoxVolume
    {
        public BoxCollider Adaptee { get; private set; }

        public Vector3 Size { get => Adaptee.size; set => Adaptee.size = value; }
        public Vector3 Center { get => Adaptee.center; set => Adaptee.center = value; }
        public Quaternion Rotation { get => Adaptee.transform.rotation; set => Adaptee.transform.rotation = value; }
    }
}