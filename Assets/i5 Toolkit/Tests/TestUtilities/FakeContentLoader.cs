using i5.Toolkit.Utilities;
using i5.Toolkit.Utilities.ContentLoaders;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.TestUtilities
{
    /// <summary>
    /// Fake content loader that simulates a Web request and returns pre-set content
    /// </summary>
    public class FakeContentLoader : IContentLoader
    {
        /// <summary>
        /// The content which should be returned by the simulated loading call
        /// </summary>
        private string content;

        /// <summary>
        /// Creates the fake content loader and assigns the content
        /// </summary>
        /// <param name="content">The content which should be returned</param>
        public FakeContentLoader(string content)
        {
            this.content = content;
        }

        /// <summary>
        /// Simulates a Web Response
        /// </summary>
        /// <param name="uri">An Uri (is ignored by the method)</param>
        /// <returns>Returns a successful WebResponse with the pre-set content</returns>
        public async Task<WebResponse<string>> LoadAsync(string uri)
        {
            WebResponse<string> resp = new WebResponse<string>(content, new byte[0], 200);
            await Task.Delay(1); // we need to do something async here
            return resp;
        }
    }
}