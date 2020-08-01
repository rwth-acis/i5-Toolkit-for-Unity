using i5.Toolkit.Core.Utilities.Async;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace i5.Toolkit.Core.Utilities
{
    public class UnityWebRequestRestConnector : IRestConnector
    {
        public async Task<WebResponse<string>> DeleteAsync(string uri)
        {
            using (UnityWebRequest req = UnityWebRequest.Delete(uri))
            {
                await req.SendWebRequest();

                if (req.isHttpError || req.isNetworkError)
                {
                    return new WebResponse<string>(req.error, req.responseCode);
                }
                else
                {
                    return new WebResponse<string>(req.downloadHandler.text, req.downloadHandler.data, req.responseCode);
                }
            }
        }

        public async Task<WebResponse<string>> GetAsync(string uri)
        {
            using (UnityWebRequest req = UnityWebRequest.Get(uri))
            {
                await req.SendWebRequest();

                if(req.isHttpError || req.isNetworkError)
                {
                    return new WebResponse<string>(req.error, req.responseCode);
                }
                else
                {
                    return new WebResponse<string>(req.downloadHandler.text, req.downloadHandler.data, req.responseCode);
                }
            }
        }

        public async Task<WebResponse<string>> PostAsync(string uri, string postData)
        {
            using (UnityWebRequest req = UnityWebRequest.Post(uri, postData))
            {
                await req.SendWebRequest();

                if (req.isHttpError || req.isNetworkError)
                {
                    return new WebResponse<string>(req.error, req.responseCode);
                }
                else
                {
                    return new WebResponse<string>(req.downloadHandler.text, req.downloadHandler.data, req.responseCode);
                }
            }
        }

        public async Task<WebResponse<string>> PutAsync(string uri, string postData)
        {
            using (UnityWebRequest req = UnityWebRequest.Put(uri, postData))
            {
                await req.SendWebRequest();

                if (req.isHttpError || req.isNetworkError)
                {
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