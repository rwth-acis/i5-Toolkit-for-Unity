using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

namespace i5.Toolkit.Utilities.ContentLoaders
{
    /// <summary>
    /// Content loader which fetches string data using MRTK's Rest.GetAsync
    /// </summary>
    public class MRTKRestLoader : IContentLoader<string>
    {
        /// <summary>
        /// Loads content using MRTK's Web access classes
        /// </summary>
        /// <param name="uri">The uri which should be loaded</param>
        /// <returns>Returns a WebResponse with the content of the web request</returns>
        public async Task<WebResponse<string>> LoadAsync(string uri)
        {
            Response resp = await Rest.GetAsync(uri);
            return WebResponse<string>.FromResponse(resp);
        }
    }
}