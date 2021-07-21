using UnityEngine;
using UnityEngine.UI;

namespace i5.Toolkit.Core.Experimental.UnityAdapters
{
    public class ScrollRectAdapter : IScrollView
    {
        public ScrollRect Adaptee { get; private set; }

        public Vector2 NormalizedPosition
        {
            get
            {
                return Adaptee.normalizedPosition;
            }

            set
            {
                Adaptee.normalizedPosition = value;
            }
        }

        public ScrollRectAdapter(ScrollRect adaptee)
        {
            Adaptee = adaptee;
        }
    }
}