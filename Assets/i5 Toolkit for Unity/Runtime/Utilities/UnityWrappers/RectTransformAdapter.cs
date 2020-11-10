using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.Utilities.UnityAdapters
{
    public class RectTransformAdapter : IRectangle
    {
        public RectTransform Adaptee { get; private set; }

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

        public RectTransformAdapter(RectTransform adaptee)
        {
            Adaptee = adaptee;
        }
    }
}