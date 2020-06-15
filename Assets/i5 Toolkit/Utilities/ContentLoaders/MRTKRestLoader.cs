using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

namespace i5.Toolkit.Utilities.ContentLoaders
{
    public class MRTKRestLoader : IContentLoader<string>
    {
        public async Task<WebResponse<string>> LoadAsync(string uri)
        {
            Response resp = await Rest.GetAsync(uri);
            return WebResponse<string>.FromResponse(resp);
        }
    }
}