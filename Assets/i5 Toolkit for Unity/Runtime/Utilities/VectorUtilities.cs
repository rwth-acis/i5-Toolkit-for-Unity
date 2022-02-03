using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.Utilities
{
    /// <summary>
    /// Extensions and utilities for vectors
    /// </summary>
    public static class VectorUtilities
    {
        /// <summary>
        /// Converts the given vector to a float array
        /// </summary>
        /// <param name="vector">The vector to convert</param>
        /// <returns>Returns a float array with the vector's components</returns>
        public static float[] ToArray(this Vector2 vector)
        {
            return new float[] { vector.x, vector.y };
        }

        /// <summary>
        /// Converts the given vector to a float array
        /// </summary>
        /// <param name="vector">The vector to convert</param>
        /// <returns>Returns a float array with the vector's components</returns>
        public static float[] ToArray(this Vector3 vector)
        {
            return new float[] { vector.x, vector.y, vector.z };
        }

        /// <summary>
        /// Converts the given vector to a float array
        /// </summary>
        /// <param name="vector">The vector to convert</param>
        /// <returns>Returns a float array with the vector's components</returns>
        public static float[] ToArray(this Vector4 vector)
        {
            return new float[] { vector.x, vector.y, vector.z, vector.w };
        }

        /// <summary>
        /// Converts a float array to a Vector4
        /// </summary>
        /// <param name="array">The array to convert</param>
        /// <returns>Returns a Vector4 where the components are the first four entries of the array</returns>
        public static Vector4 Vector4FromArray(float[] array)
        {
            Vector4 result = new Vector4();
            for (int i = 0; i < Mathf.Min(4, array.Length); i++)
            {
                result[i] = array[i];
            }
            return result;
        }

        /// <summary>
        /// Converts a float array to a Vector3
        /// </summary>
        /// <param name="array">The array to convert</param>
        /// <returns>Returns a Vector3 where the components are the first three entries of the array</returns>
        public static Vector3 Vector3FromArray(float[] array)
        {
            // automatically converts
            return Vector4FromArray(array);
        }

        /// <summary>
        /// Converts a float array to a Vector2
        /// </summary>
        /// <param name="array">The array to convert</param>
        /// <returns>Returns a Vector2 where the components are the first three entries of the array</returns>
        public static Vector2 Vector2FromArray(float[] array)
        {
            // automatically converts
            return Vector4FromArray(array);
        }
    }
}