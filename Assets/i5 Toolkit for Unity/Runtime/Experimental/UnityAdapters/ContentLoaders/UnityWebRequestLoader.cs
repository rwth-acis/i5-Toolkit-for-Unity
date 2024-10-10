using System.Threading.Tasks;
using UnityEngine.Networking;
using i5.Toolkit.Core.Utilities.Async;
using UnityEngine;

namespace i5.Toolkit.Core.Utilities.ContentLoaders
{
    /// <summary>
    /// Content loader that uses UnityWebRequests to fech data from the Web
    /// </summary>
    public class UnityWebRequestLoader : IContentLoader<string>
    {
        /// <summary>
        /// Loads content from the given URI
        /// </summary>
        /// <param name="uri">The URI from where content should be downloaded</param>
        /// <returns>Returns the downloaded string content</returns>
        public async Task<WebResponse<string>> LoadAsync(string uri)
        {
            using (UnityWebRequest req = UnityWebRequest.Get(uri))
            {
                await req.SendWebRequest();

                if (req.result == UnityWebRequest.Result.Success)
                {
					return new WebResponse<string>(req.downloadHandler.text, req.downloadHandler.data, req.responseCode);
                }
                else
                {
					i5Debug.LogError("Get request to: " + uri + " returned with error " + req.error, this);
					return new WebResponse<string>(false, req.downloadHandler.text, req.downloadHandler.data, req.responseCode, req.error);
				}
            }
        }
    }
}