using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Utilities
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
    }
}