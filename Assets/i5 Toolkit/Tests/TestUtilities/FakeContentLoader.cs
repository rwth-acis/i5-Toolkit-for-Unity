using i5.Toolkit.Utilities;
using i5.Toolkit.Utilities.ContentLoaders;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.TestUtilities
{
    public class FakeContentLoader : IContentLoader
    {
        private string content;
        public FakeContentLoader(string content)
        {
            this.content = content;
        }

        public async Task<WebResponse<string>> LoadAsync(string uri)
        {
            WebResponse<string> resp = new WebResponse<string>(content, new byte[0], 200);
            await Task.Delay(1); // we need to do something async here
            return resp;
        }
    }
}