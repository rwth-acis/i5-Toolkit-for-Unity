using UnityEngine;

namespace i5.Toolkit.Core.Utilities
{
    /// <summary>
    /// Extension methods for the Color type
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Converts the given color to a float array
        /// </summary>
        /// <param name="color">The color to convert</param>
        /// <param name="includeAlpha">If true, the alpha value is included in the array, otherwise not</param>
        /// <returns>Returns a float array with three or four values containing the rgb and rgba components</returns>
        public static float[] ToArray(this Color color, bool includeAlpha = true)
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