using UnityEngine;

namespace i5.Toolkit.Core.Experimental.UnityAdapters
{
    public class TransformAdapter : ITransformable
    {
        public Transform Adaptee { get; protected set; }

        public Vector3 Position { get => Adaptee.position; set => Adaptee.position = value; }
        public Vector3 LocalPosition { get => Adaptee.localPosition; set => Adaptee.localPosition = value; }
        public Quaternion Rotation { get => Adaptee.rotation; set => Adaptee.rotation = value; }
        public Quaternion LocalRotation { get => Adaptee.localRotation; set => Adaptee.localRotation = value; }
        public Vector3 EulerAngles { get => Adaptee.eulerAngles; set => Adaptee.eulerAngles = value; }
        public Vector3 LocalEulerAngles { get => Adaptee.localEulerAngles; set => Adaptee.localEulerAngles = value; }
        public Vector3 LocalScale { get => Adaptee.localScale; set => Adaptee.localScale = value; }
        public Vector3 LossyScale { get => Adaptee.lossyScale; }

        public TransformAdapter(Transform adaptee)
        {
            Adaptee = adaptee;
        }
    }
}