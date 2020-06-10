using i5.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.TestUtilities
{
    public class FakeTextureLoader : ITextureLoader
    {
        public async Task<WebResponse<Texture2D>> LoadTextureAsync(string uri)
        {
            await Task.Delay(1);
            Texture2D tex = new Texture2D(2, 2);
            WebResponse<Texture2D> resp = new WebResponse<Texture2D>(tex, new byte[0], 200);
            return resp;
        }
    }
}