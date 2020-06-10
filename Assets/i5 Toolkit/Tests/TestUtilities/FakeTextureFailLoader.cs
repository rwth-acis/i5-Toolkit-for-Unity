using i5.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.TestUtilities
{
    public class FakeTextureFailLoader : ITextureLoader
    {
        public async Task<WebResponse<Texture2D>> LoadTextureAsync(string uri)
        {
            await Task.Delay(1);
            WebResponse<Texture2D> resp = new WebResponse<Texture2D>("This is a simulated fail", 404);
            return resp;
        }
    }
}