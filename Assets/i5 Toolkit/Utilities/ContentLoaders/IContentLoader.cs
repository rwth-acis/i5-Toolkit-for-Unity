using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Utilities.ContentLoaders
{
    /// <summary>
    /// Interface for content loaders which load string resources from the Web
    /// </summary>
    public interface IContentLoader
    {
        /// <summary>
        /// Loads a string resource from the given URI
        /// Should be used asynchronously
        /// </summary>
        /// <param name="uri">The uri where the string resource is stored</param>
        /// <returns>The fetched string resource</returns>
        Task<WebResponse<string>> LoadAsync(string uri);
    }
}