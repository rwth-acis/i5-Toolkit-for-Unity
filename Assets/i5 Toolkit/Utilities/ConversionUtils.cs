using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Utilities
{

    public static class ConversionUtils
    {
        public static Color ToColor(this Vector3 vector)
        {
            return new Color(vector.x, vector.y, vector.z);
        }

        public static Color ToColor(this Vector4 vector)
        {
            return new Color(vector.x, vector.y, vector.z, vector.w);
        }

        public static Vector3 ToVector3(this Color color)
        {
            return new Vector3(color.r, color.g, color.b);
        }

        public static Vector4 ToVector4(this Color color)
        {
            return new Vector4(color.r, color.g, color.b, color.a);
        }
    }
}