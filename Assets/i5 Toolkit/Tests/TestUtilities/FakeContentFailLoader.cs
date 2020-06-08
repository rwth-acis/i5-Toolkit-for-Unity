using i5.Toolkit.Utilities;
using i5.Toolkit.Utilities.ContentLoaders;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FakeContentFailLoader : IContentLoader
{
    public async Task<WebResponse<string>> LoadAsync(string uri)
    {
        WebResponse<string> resp = new WebResponse<string>("This is a simulated fail", 404);
        await Task.Delay(1); // we need to do something async here
        return resp;
    }
}
