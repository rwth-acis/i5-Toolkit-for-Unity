using UnityEngine;

namespace i5.Toolkit.Core.Utilities
{
    public static class Vector3Extensions
    {
        /// <summary>
        /// Multiplies the values of the vector component wise
        /// </summary>
        /// <param name="vector">The first vector of the multiplication</param>
        /// <param name="other">The second vector of the multiplication</param>
        /// <returns>Returns the component-wise product of the two vectors, so (v1.x * v2.x, v1.y * v2.y, v1.z * v2.z)</returns>
        public static Vector3 MultiplyComponentWise(this Vector3 vector, Vector3 other)
        {
            return new Vector3(
                vector.x * other.x,
                vector.y * other.y,
                vector.z * other.z
                );
        }
    }
}
