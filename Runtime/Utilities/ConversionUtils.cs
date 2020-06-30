using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.Utilities
{
    /// <summary>
    /// Extensions for converting data formats to each other
    /// </summary>
    public static class ConversionUtils
    {
        /// <summary>
        /// Converts a Vector3 to a Color
        /// Maps the x-value to the red channel,
        /// the y-value to the green channel
        /// and the z-value to the blue channel
        /// </summary>
        /// <param name="vector">The Vector3 which should be converted</param>
        /// <returns>Returns a color which has the same rgb values as the vector has xyz values</returns>
        public static Color ToColor(this Vector3 vector)
        {
            return new Color(vector.x, vector.y, vector.z);
        }

        /// <summary>
        /// Converts a Vector4 to a Color
        /// Maps the x-value to the red channel, 
        /// the y-value to the green channel,
        /// the z-value to the blue channel
        /// and the w-value to the alpha channel
        /// </summary>
        /// <param name="vector">The Vector4 which should be converted</param>
        /// <returns>Returns a color which has the same rgba values as the vector has xyzw values</returns>
        public static Color ToColor(this Vector4 vector)
        {
            return new Color(vector.x, vector.y, vector.z, vector.w);
        }

        /// <summary>
        /// Converts a Color to a Vector3
        /// Maps the red channel to the x value,
        /// the green channel to the y value
        /// and the blue channel to the z value
        /// </summary>
        /// <param name="color">The color to convert</param>
        /// <returns>Returns a Vector3 which has the same xyz values as the color has rgb values</returns>
        public static Vector3 ToVector3(this Color color)
        {
            return new Vector3(color.r, color.g, color.b);
        }

        /// <summary>
        /// Converts a Color to a Vector4
        /// Maps the red channel to the x value,
        /// the green channel to the y value,
        /// the blue channel to the z value
        /// and the alpha channel to the w value
        /// </summary>
        /// <param name="color">The color to convert</param>
        /// <returns>Returns a Vector4 which has the same xyzw values as the color has rgba values</returns>
        public static Vector4 ToVector4(this Color color)
        {
            return new Vector4(color.r, color.g, color.b, color.a);
        }
    }
}