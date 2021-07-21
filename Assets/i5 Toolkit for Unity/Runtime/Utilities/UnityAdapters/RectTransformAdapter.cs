using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.Experimental.UnityAdapters
{
    public class RectTransformAdapter : TransformAdapter, IRectangle
    {
        public new RectTransform Adaptee { get => (RectTransform)base.Adaptee; private set => base.Adaptee = value; }

        public Vector2 Size
        {
            get
            {
                return Adaptee.sizeDelta;
            }

            set
            {
                Adaptee.sizeDelta = value;
            }
        }

        public RectTransformAdapter(RectTransform adaptee) : base(adaptee)
        {
        }
    }
}