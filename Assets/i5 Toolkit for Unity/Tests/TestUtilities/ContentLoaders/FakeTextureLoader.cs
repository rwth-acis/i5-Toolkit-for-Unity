using i5.Toolkit.Core.Utilities;
using i5.Toolkit.Core.Utilities.ContentLoaders;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.TestUtilities
{
    /// <summary>
    /// Fake texture loader which returns a 2x2 texture
    /// </summary>
    public class FakeTextureLoader : IContentLoader<Texture2D>
    {
        /// <summary>
        /// Simulates texture loading
        /// </summary>
        /// <param name="uri">An Uri (is ignored by the method)</param>
        /// <returns>Returns a WebResponse with the created 2x2 texture</returns>
        public async Task<WebResponse<Texture2D>> LoadAsync(string uri)
        {
            await Task.Delay(1);
            Texture2D tex = new Texture2D(2, 2);
            WebResponse<Texture2D> resp = new WebResponse<Texture2D>(tex, new byte[0], 200);
            return resp;
        }
    }
}