using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.Utilities
{
    public static class ColorUtilities
    {
        public static float[] ToArray(this Color color, bool includeAlpha = false)
        {
            if (includeAlpha)
            {
                return new float[] { color.r, color.g, color.b, color.a };
            }
            else
            {
                return new float[] { color.r, color.g, color.b };
            }
        }
    }
}