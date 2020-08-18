using System;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.Utilities
{
    /// <summary>
    /// Utility functions for operating on Uris
    /// </summary>
    public class UriUtils
    {
        /// <summary>
        /// Rewrites a given URI that points to a file so that it points to the location specified by the relative file path
        /// The relative file path starts at the uri's destination
        /// </summary>
        /// <param name="uri">The uri which should be rewritten</param>
        /// <param name="relativeFilePath">A relative file path starting at uri's location</param>
        /// <returns>Returns an absolute Uri which points to the location of the relative file path</returns>
        public static string RewriteFileUriPath(Uri uri, string relativeFilePath)
        {
            // construct the uri by first collecting the authority part of the uri
            string resultUri = uri.GetLeftPart(UriPartial.Authority);
            // add all segments except of the last one which is the file name
            for (int i = 0; i < uri.Segments.Length - 1; i++)
            {
                resultUri += uri.Segments[i];
            }
            resultUri += relativeFilePath;
            // after the file path, add any arguments that were specified
            resultUri += uri.Query;
            return resultUri;
        }

        public static Dictionary<string, string> GetUriFragments(Uri uri)
        {
            return GetUriParams(uri, '#');
        }

        /// <summary>
        /// Extracts the parameters of a given uri and returns them
        /// </summary>
        /// <param name="uri">The uri which contains the parameters</param>
        /// <returns>Returns the parameters of the uri</returns>
        public static Dictionary<string, string> GetUriParameters(Uri uri)
        {
            return GetUriParams(uri, '?');
        }

        private static Dictionary<string, string> GetUriParams(Uri uri, char separator)
        {
            string[] parts = uri.ToString().Split(separator);
            if (parts.Length < 2)
            {
                // return an empty dictionary since the uri apparently does not have parameters
                return new Dictionary<string, string>();
            }
            else if (parts.Length > 2)
            {
                Debug.LogError("Unexpected amount of parts of the uri. The uri seems to contain multiple '" + separator + "'");
                return null;
            }

            // if the uri has two parts: we have the front part, e.g. http://www.google.de
            // and the back part, e.g. param=example&number=1

            string[] parameterArray = parts[1].Split('&');
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            for (int i = 0; i < parameterArray.Length; i++)
            {
                string[] splittedParameter = parameterArray[i].Split('=');
                if (splittedParameter.Length != 2)
                {
                    Debug.LogWarning("Parameter could not be resolved into key and value: " + parameterArray[i]);
                    continue;
                }

                parameters.Add(splittedParameter[0], splittedParameter[1]);
            }

            return parameters;

        }

        public static string WordArrayToSpaceEscapedString(string[] array)
        {
            string concatArray = "";
            for (int i = 0; i < array.Length; i++)
            {
                concatArray += array[i];
                if (i < array.Length - 1)
                {
                    concatArray += "%20";
                }
            }
            return concatArray;
        }

        public static string DictionaryToParameterString<T>(Dictionary<string, T> dictionary)
        {
            string parameters = "";
            foreach(KeyValuePair<string, T> entry in dictionary)
            {
                parameters += entry.Key + "=" + entry.Value.ToString() + "&";
            }
            // remove the last &
            if (parameters.Length > 0)
            {
                parameters = parameters.Substring(0, parameters.Length - 1);
            }
            // escape spaces
            parameters.Replace(" ", "%20");
            return parameters;
        }
    }
}