using UnityEngine;

namespace i5.Toolkit.Core.Utilities
{
    /// <summary>
    /// Utilities for working with PlayerPrefs
    /// Contains e.g. support for more types
    /// </summary>
    public static class PlayerPrefsUtilities
    {
        /// <summary>
        /// Stores a float array in the PlayerPrefs
        /// </summary>
        /// <param name="key">The key of the array</param>
        /// <param name="array">The array to store in the PlayerPrefs</param>
        public static void SetFloatArray(string key, float[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                PlayerPrefs.SetFloat($"{key}[{i}]", array[i]);
            }
            PlayerPrefs.SetInt($"{key}.Length", array.Length);
        }

        /// <summary>
        /// Gets the array of a stored array
        /// </summary>
        /// <param name="key">The key of the array</param>
        /// <returns>Returns the intended length of the array. If no length was stored, it returns 0.</returns>
        public static int GetArrayLength(string key)
        {
            int length = PlayerPrefs.GetInt($"{key}.Length", 0);
            return length;
        }

        /// <summary>
        /// Gets a float array from the given key in the PlayerPrefs
        /// </summary>
        /// <param name="key">The key of the array</param>
        /// <returns>Returns the float array from the PlayerPrefs</returns>
        public static float[] GetFloatArray(string key)
        {
            int length = GetArrayLength(key);
            float[] array = new float[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = PlayerPrefs.GetFloat($"{key}[{i}]");
            }
            return array;
        }

        /// <summary>
        /// Stores a Vector2 in the PlayerPrefs
        /// </summary>
        /// <param name="key">The key under which to store the vector</param>
        /// <param name="vector">The value of the vector</param>
        public static void SetVector2(string key, Vector2 vector)
        {
            SetFloatArray(key, vector.ToArray());
        }

        /// <summary>
        /// Stores a Vector3 in the PlayerPrefs
        /// </summary>
        /// <param name="key">The key under which to store the vector</param>
        /// <param name="vector">The value of the vector</param>
        public static void SetVector3(string key, Vector3 vector)
        {
            SetFloatArray(key, vector.ToArray());
        }

        /// <summary>
        /// Stores a Vector4 in the PlayerPrefs
        /// </summary>
        /// <param name="key">The key under which to store the vector</param>
        /// <param name="vector">The value of the vector</param>
        public static void SetVector4(string key, Vector4 vector)
        {
            SetFloatArray(key, vector.ToArray());
        }

        /// <summary>
        /// Gets a Vector2 from the PlayerPrefs
        /// </summary>
        /// <param name="key">The key under which to the vector is stored</param>
        public static Vector2 GetVector2(string key)
        {
            return GetVector2(key, Vector2.zero);
        }

        /// <summary>
        /// Gets a Vector2 from the PlayerPrefs
        /// </summary>
        /// <param name="key">The key under which to the vector is stored</param>
        /// <param name="defaultValue">The default value which is taken if the vector does not exist in the PlayerPrefs</param>
        public static Vector2 GetVector2(string key, Vector2 defaultValue)
        {
            return GetVector(key, defaultValue);
        }

        /// <summary>
        /// Gets a Vector3 from the PlayerPrefs
        /// </summary>
        /// <param name="key">The key under which to the vector is stored</param>
        public static Vector3 GetVector3(string key)
        {
            return GetVector3(key, Vector3.zero);
        }

        /// <summary>
        /// Gets a Vector3 from the PlayerPrefs
        /// </summary>
        /// <param name="key">The key under which to the vector is stored</param>
        /// <param name="defaultValue">The default value which is taken if the vector does not exist in the PlayerPrefs</param>
        public static Vector3 GetVector3(string key, Vector3 defaultValue)
        {
            return GetVector(key, defaultValue);
        }

        /// <summary>
        /// Gets a Vector4 from the PlayerPrefs
        /// </summary>
        /// <param name="key">The key under which to the vector is stored</param>
        public static Vector4 GetVector4(string key)
        {
            return GetVector4(key, Vector4.zero);
        }

        /// <summary>
        /// Gets a Vector4 from the PlayerPrefs
        /// </summary>
        /// <param name="key">The key under which to the vector is stored</param>
        /// <param name="defaultValue">The default value which is taken if the vector does not exist in the PlayerPrefs</param>
        public static Vector4 GetVector4(string key, Vector4 defaultValue)
        {
            return GetVector(key, defaultValue);
        }

        // common function to retrieve vectors from the PlayerPrefs
        private static Vector4 GetVector(string key, Vector4 defaultValue)
        {
            int length = GetArrayLength(key);
            if (length == 0)
            {
                return defaultValue;
            }
            return VectorUtilities.Vector4FromArray(GetFloatArray(key));
        }

        /// <summary>
        /// Checks whether a vector was stored under the given key
        /// </summary>
        /// <param name="key">The key of the vector</param>
        /// <returns>Returns true if a vector with the key is stored in the PlayerPrefs, otherwise false</returns>
        public static bool HasVectorKey(string key)
        {
            return GetArrayLength(key) > 0;
        }

        /// <summary>
        /// Stores a color in the PlayerPrefs
        /// </summary>
        /// <param name="key">The key under which the color should be stored</param>
        /// <param name="color">The color to store</param>
        /// <param name="saveAlpha">If set to false, the alpha value is not stored</param>
        public static void SetColor(string key, Color color, bool saveAlpha = true)
        {
            float[] array = color.ToArray(saveAlpha);
            SetFloatArray(key, array);
        }

        /// <summary>
        /// Gets a color under the given key from the PlayerPrefs
        /// </summary>
        /// <param name="key">The key under which the color is stored</param>
        /// <returns>Returns the color if it exists, otherwise it returns white</returns>
        public static Color GetColor(string key)
        {
            return GetColor(key, Color.white);
        }

        /// <summary>
        /// Gets a color under the given key from the PlayerPrefs
        /// </summary>
        /// <param name="key">The key under which the color is stored</param>
        /// <param name="defaultValue">The default parameter which is returned in case no color is stored under this key</param>
        /// <returns>Returns the color if it exists, otherwise it returns the defaultValue</returns>
        public static Color GetColor(string key, Color defaultValue)
        {
            float[] array = GetFloatArray(key);
            if (array.Length == 0)
            {
                return defaultValue;
            }

            Color color = new Color();
            for (int i = 0; i < Mathf.Min(4, array.Length); i++)
            {
                color[i] = array[i];
            }
            return color;
        }

        /// <summary>
        /// Checks if a color is stored under the given key
        /// </summary>
        /// <param name="key">The key under which the color is stored</param>
        /// <returns>Returns true if a color is stored under this key</returns>
        public static bool HasColorKey(string key)
        {
            return GetArrayLength(key) > 0;
        }
    }
}