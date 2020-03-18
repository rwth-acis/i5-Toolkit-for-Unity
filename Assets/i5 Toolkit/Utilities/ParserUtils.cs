using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace i5.Toolkit.Utilities
{
    public static class ParserUtils
    {
        /// <summary>
        /// Tries to parse a string with three space-separated coordinates, e.g. "1.2 -5.3 1.0"
        /// </summary>
        /// <param name="input">The string which should be converted to a Vector3</param>
        /// <param name="result">The result of the conversion; if the conversion failed, it has the default value</param>
        /// <returns>True if the string could be parsed, otherwise false</returns>
        public static bool TryParseSpaceSeparatedVector3(string input, out Vector3 result)
        {
            string[] strValues = input.Trim().Split(' ');
            // there should be three coordinates
            if (strValues.Length == 3)
            {
                float vCoord1, vCoord2, vCoord3;
                // parse the first three coordinates
                if (float.TryParse(strValues[0], NumberStyles.Any, CultureInfo.InvariantCulture, out vCoord1)
                    && float.TryParse(strValues[1], NumberStyles.Any, CultureInfo.InvariantCulture, out vCoord2)
                    && float.TryParse(strValues[2], NumberStyles.Any, CultureInfo.InvariantCulture, out vCoord3))
                {
                    result = new Vector3(vCoord1, vCoord2, vCoord3);
                    return true;
                }
            }

            result = default;
            return false;
        }
    }
}