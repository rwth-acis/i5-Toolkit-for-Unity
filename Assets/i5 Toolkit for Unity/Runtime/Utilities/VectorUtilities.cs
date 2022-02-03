using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.Utilities
{
    public static class VectorUtilities
    {
        public static float[] ToArray(this Vector2 vector)
        {
            return new float[] { vector.x, vector.y };
        }

        public static float[] ToArray(this Vector3 vector)
        {
            return new float[] { vector.x, vector.y, vector.z };
        }

        public static float[] ToArray(this Vector4 vector)
        {
            return new float[] { vector.x, vector.y, vector.z, vector.w };
        }

        public static Vector4 Vector4FromArray(float[] array)
        {
            Vector4 result = new Vector4();
            for (int i = 0; i < Mathf.Min(4, array.Length); i++)
            {
                result[i] = array[i];
            }
            return result;
        }

        public static Vector3 Vector3FromArray(float[] array)
        {
            // automatically converts
            return Vector4FromArray(array);
        }

        public static Vector2 Vector2FromArray(float[] array)
        {
            // automatically converts
            return Vector4FromArray(array);
        }
    }
}