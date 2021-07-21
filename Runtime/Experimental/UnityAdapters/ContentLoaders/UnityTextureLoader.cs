using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using i5.Toolkit.Core.Utilities.Async;

namespace i5.Toolkit.Core.Utilities.ContentLoaders
{
    /// <summary>
    /// Adapter class which loads textures using Unity's WebRequestsTexture
    /// </summary>
    public class UnityTextureLoader : IContentLoader<Texture2D>
    {
        /// <summary>
        /// Loads the texture at the given URI using Unity's built-in methods
        /// </summary>
        /// <param name="uri">The uri where the texture is stored</param>
        /// <returns>Returns a WebResponse with the results of the web request</returns>
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