using i5.Toolkit.Utilities;
using i5.Toolkit.Utilities.ContentLoaders;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Fake content loader that simulates a Web request which fails
/// </summary>
public class FakeContentFailLoader : IContentLoader<string>
{
    /// <summary>
    /// Simulates a Web request that fails
    /// </summary>
    /// <param name="uri">A URi (is ignored by the method)</param>
    /// <returns>A failed WebReponse answer</returns>
    public async Task<WebResponse<string>> LoadAsync(string uri)
    {
        WebResponse<string> resp = new WebResponse<string>("This is a simulated fail", 404);
        await Task.Delay(1); // we need to do something async here
        return resp;
    }
}
