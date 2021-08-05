using System;
using System.IO;
using UnityEngine;

namespace i5.Toolkit.Core.Utilities
{
    public static class PathUtils
    {
        /// <summary>
        /// Combines an absolute base path with an absolute or relative path.
        /// If addedPath is absolute, it is taken as the combined result, disregarding the base path.
        /// If addedPath is relative, it is combined based on the basePath
        /// Works with local file paths and Uris
        /// </summary>
        /// <param name="basePath">The base path where the combination of paths should start. Must be absolute.</param>
        /// <param name="addedPath">The added path that is combined with the base path. Can be relative or absolute.</param>
        /// <returns>The combined path where the basePath is concatenated with the relative addedPath or the absolute addedPath is returned</returns>
        public static string RewriteToAbsolutePath(string basePath, string addedPath)
        {
            if (!Uri.TryCreate(basePath, UriKind.Absolute, out Uri baseUri))
            {
                Debug.LogError($"[PathUtils] Base Path must be absolute.");
                return string.Empty;
            }

            if (baseUri.IsFile)
            {
                // check whether material path is given relative to the obj path or fully qualified
                if (Path.IsPathRooted(addedPath))
                {
                    // path is absolute
                    return addedPath;
                }
                else
                {
                    // path is relative
                    string baseDirectory = Path.GetDirectoryName(basePath);
                    return Path.Combine(baseDirectory, addedPath);
                }
            }
            else
            {
                // try to interprete as web uri
                // check wether addedPath is absolute or relative
                if (Uri.TryCreate(addedPath, UriKind.Absolute, out Uri uri))
                {
                    // addedPath is absolute
                    return uri.AbsoluteUri;
                }
                else
                {
                    // addedPath is relative
                    return UriUtils.RewriteFileUriPath(baseUri, addedPath);
                }
            }
        }
    }
}
