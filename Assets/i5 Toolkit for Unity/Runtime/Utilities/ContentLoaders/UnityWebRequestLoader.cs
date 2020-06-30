using System.Threading.Tasks;
using UnityEngine.Networking;
using i5.Toolkit.Core.Utilities.Async;

namespace i5.Toolkit.Core.Utilities.ContentLoaders
{
    public class UnityWebRequestLoader : IContentLoader<string>
    {
        public async Task<WebResponse<string>> LoadAsync(string uri)
        {
            using (UnityWebRequest req = UnityWebRequest.Get(uri))
            {
                await req.SendWebRequest();

                if (req.isNetworkError || req.isHttpError)
                {
                    i5Debug.LogError("Get request to: " + uri + " returned with error " + req.error, this);
                    return new WebResponse<string>(req.error, req.responseCode);
                }
                else
                {
                    return new WebResponse<string>(req.downloadHandler.text, req.downloadHandler.data, req.responseCode);
                }
            }
        }
    }
}