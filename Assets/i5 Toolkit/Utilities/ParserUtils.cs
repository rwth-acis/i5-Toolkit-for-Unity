using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace i5.Toolkit.Utilities
{
    public static class ParserUtils
    {
        private static char[] splitter = new char[] { ' ' };

        public static bool TryParseSpaceSeparatedVector2(string input, out Vector2 result)
        {
            string[] strValues = input.Trim().Split(splitter, System.StringSplitOptions.RemoveEmptyEntries);
            if (strValues.Length == 2)
            {
                return TryParseStringArrayToVector2(strValues, out result);
            }
            else if (strValues.Length == 3)
            {
                bool res = TryParseStringArrayToVector3(strValues, out Vector3 v3);
                result = v3;
                return res;
            }
            else
            {
                result = default;
                return false;
            }
        }

        public static bool TryParseStringArrayToVector2(string[] strValues, out Vector2 result)
        {
            // there should be two coordinates
            if (strValues.Length == 2)
            {
                float vCoord1, vCoord2;
                // parse the first three coordinates
                if (float.TryParse(strValues[0], NumberStyles.Any, CultureInfo.InvariantCulture, out vCoord1)
                    && float.TryParse(strValues[1], NumberStyles.Any, CultureInfo.InvariantCulture, out vCoord2))
                {
                    result = new Vector2(vCoord1, vCoord2);
                    return true;
                }
            }

            result = default;
            return false;
        }

        /// <summary>
        /// Tries to parse a string with three space-separated coordinates, e.g. "1.2 -5.3 1.0"
        /// </summary>
        /// <param name="input">The string which should be converted to a Vector3</param>
        /// <param name="result">The result of the conversion; if the conversion failed, it has the default value</param>
        /// <returns>True if the string could be parsed, otherwise false</returns>
        public static bool TryParseSpaceSeparatedVector3(string input, out Vector3 result)
        {
            string[] strValues = input.Trim().Split(splitter, System.StringSplitOptions.RemoveEmptyEntries);
            if (strValues.Length == 3)
            {
                return TryParseStringArrayToVector3(strValues, out result);
            }
            else if (strValues.Length == 4)
            {
                bool res = TryParseStringArrayToVector4(strValues, out Vector4 v4);
                result = v4;
                return res;
            }
            else
            {
                result = default;
                return false;
            }
        }

        public static bool TryParseStringArrayToVector3(string[] strValues, out Vector3 result)
        {
            // there should be three coordinates
            if (strValues.Length == 3)
            {
                Vector2 firstTwoComponents;
                float vCoord3;
                // parse the first three coordinates
                if (TryParseStringArrayToVector2(new string[] { strValues[0], strValues[1] }, out firstTwoComponents)
                    && float.TryParse(strValues[2], NumberStyles.Any, CultureInfo.InvariantCulture, out vCoord3))
                {
                    result = new Vector3(firstTwoComponents.x, firstTwoComponents.y, vCoord3);
                    return true;
                }
            }

            result = default;
            return false;
        }

        public static bool TryParseStringArrayToVector4(string[] strValues, out Vector4 result)
        {
            // there should be four coordinates
            if (strValues.Length == 4)
            {
                Vector3 firstThreeComponents;
                float vCoord4;
                // prase the first four coordinates
                if (TryParseStringArrayToVector3(new string[] { strValues[0], strValues[1], strValues[2] }, out firstThreeComponents)
                    && float.TryParse(strValues[3], NumberStyles.Any, CultureInfo.InvariantCulture, out vCoord4))
                {
                    result = new Vector4(firstThreeComponents.x, firstThreeComponents.y, firstThreeComponents.z, vCoord4);
                    return true;
                }
            }

            result = default;
            return false;
        }
    }
}