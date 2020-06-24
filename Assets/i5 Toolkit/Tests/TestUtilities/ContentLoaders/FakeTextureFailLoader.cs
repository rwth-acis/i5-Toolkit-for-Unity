using i5.Toolkit.Core.Utilities;
using i5.Toolkit.Core.Utilities.ContentLoaders;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.TestUtilities
{
    /// <summary>
    /// Fake texture loader which simulates failing texture loading
    /// </summary>
    public class FakeTextureFailLoader : IContentLoader<Texture2D>
    {
        /// <summary>
        /// Simulates texture loading
        /// </summary>
        /// <param name="uri">An Uri (is ignored by the method)</param>
        /// <returns>Returns a WebResponse with a simulated fail</returns>
        public async Task<WebResponse<Texture2D>> LoadAsync(string uri)
        {
            await Task.Delay(1);
            WebResponse<Texture2D> resp = new WebResponse<Texture2D>("This is a simulated fail", 404);
            return resp;
        }
    }
}