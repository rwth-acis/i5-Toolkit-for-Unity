using UnityEngine;

namespace i5.Toolkit.Core.Utilities
{
    public static class Vector3Extensions
    {
        /// <summary>
        /// Multiplies the values of the vector component-wise
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

        /// <summary>
        /// Divides the values of the vector component-wise by the given divisor
        /// </summary>
        /// <param name="divident">The divident of the division</param>
        /// <param name="divisor">The divisor of the division</param>
        /// <returns>Returns the component-wise division of the two vectors, so (v.x / divisor.x, v.y / divisor.y, v.z / divisor.z)</returns>
        public static Vector3 DivideComponentWiseBy(this Vector3 divident, Vector3 divisor)
        {
            return new Vector3(
                divident.x / divisor.x,
                divident.y / divisor.y,
                divident.z / divisor.z
                );
        }

        /// <summary>
        /// Gets teh smallest component in the vector
        /// </summary>
        /// <param name="vector">The vector to inspect</param>
        /// <returns>Returns the minimum component of the vector</returns>
        public static float MinimumComponent(this Vector3 vector)
        {
            return Mathf.Min(vector.x, vector.y, vector.z);
        }

        /// <summary>
        /// Gets the largest component in the vector
        /// </summary>
        /// <param name="vector">The vector to inspect</param>
        /// <returns>Returns the maximum component of the vector</returns>
        public static float MaximumComponent(this Vector3 vector)
        {
            return Mathf.Max(vector.x, vector.y, vector.z);
        }
    }
}
