using i5.Toolkit.Core.Utilities.Async;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace i5.Toolkit.Core.Utilities
{
    public class UnityWebRequestRestConnector : IRestConnector
    {
        public async Task<WebResponse<string>> DeleteAsync(string uri, Dictionary<string, string> headers = null)
        {
            using (UnityWebRequest req = UnityWebRequest.Delete(uri))
            {
                AddHeaders(req, headers);
                await req.SendWebRequest();

                if (req.isHttpError || req.isNetworkError)
                {
                    return new WebResponse<string>(false, req.downloadHandler.text, req.downloadHandler.data, req.responseCode, req.error);
                }
                else
                {
                    return new WebResponse<string>(req.downloadHandler.text, req.downloadHandler.data, req.responseCode);
                }
            }
        }

        public async Task<WebResponse<string>> GetAsync(string uri, Dictionary<string, string> headers = null)
        {
            using (UnityWebRequest req = UnityWebRequest.Get(uri))
            {
                AddHeaders(req, headers);
                await req.SendWebRequest();

                if(req.isHttpError || req.isNetworkError)
                {
                    return new WebResponse<string>(false, req.downloadHandler.text, req.downloadHandler.data, req.responseCode, req.error);
                }
                else
                {
                    return new WebResponse<string>(req.downloadHandler.text, req.downloadHandler.data, req.responseCode);
                }
            }
        }

        public async Task<WebResponse<string>> PostAsync(string uri, string postData, Dictionary<string, string> headers = null)
        {
            using (UnityWebRequest req = UnityWebRequest.Post(uri, postData))
            {
                AddHeaders(req, headers);
                await req.SendWebRequest();

                if (req.isHttpError || req.isNetworkError)
                {
                    return new WebResponse<string>(false, req.downloadHandler.text, req.downloadHandler.data, req.responseCode, req.error);
                }
                else
                {
                    return new WebResponse<string>(req.downloadHandler.text, req.downloadHandler.data, req.responseCode);
                }
            }
        }

        public async Task<WebResponse<string>> PutAsync(string uri, string postData, Dictionary<string, string> headers = null)
        {
            using (UnityWebRequest req = UnityWebRequest.Put(uri, postData))
            {
                AddHeaders(req, headers);
                await req.SendWebRequest();

                if (req.isHttpError || req.isNetworkError)
                {
                    return new WebResponse<string>(false, req.downloadHandler.text, req.downloadHandler.data, req.responseCode, req.error);
                }
                else
                {
                    return new WebResponse<string>(req.downloadHandler.text, req.downloadHandler.data, req.responseCode);
                }
            }
        }

        private void AddHeaders(UnityWebRequest req, Dictionary<string,string> headers)
        {
            if (headers == null)
            {
                return;
            }
            foreach(KeyValuePair<string,string> header in headers)
            {
                req.SetRequestHeader(header.Key, header.Value);
            }
        }
    }
}