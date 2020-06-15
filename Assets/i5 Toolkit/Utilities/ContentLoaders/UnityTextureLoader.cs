using i5.Toolkit.Utilities;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Microsoft.MixedReality.Toolkit.Utilities;

namespace i5.Toolkit.Utilities.ContentLoaders
{
    public class UnityTextureLoader : IContentLoader<Texture2D>
    {
        public async Task<WebResponse<Texture2D>> LoadAsync(string uri)
        {
            using (UnityWebRequest req = UnityWebRequestTexture.GetTexture(uri))
            {
                await req.SendWebRequest();

                if (req.isNetworkError || req.isHttpError)
                {
                    i5Debug.LogError("Error fetching texture: " + req.error, this);
                    return new WebResponse<Texture2D>(req.error, req.responseCode);
                }
                else
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(req);
                    return new WebResponse<Texture2D>(texture, req.downloadHandler.data, req.responseCode);
                }
            }
        }
    }
}