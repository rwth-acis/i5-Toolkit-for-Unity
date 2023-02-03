using i5.Toolkit.Core.Utilities.Async;
using System.Collections.Generic;
using System.Text;
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
                req.downloadHandler = new DownloadHandlerBuffer();
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
            using (UnityWebRequest req = UnityWebRequest.Post(uri, "POST"))
            {
                byte[] data = new UTF8Encoding().GetBytes(postData);
                req.uploadHandler = new UploadHandlerRaw(data);
                req.downloadHandler = new DownloadHandlerBuffer();
                req.SetRequestHeader("Content-Type", "application/json");
                req.SetRequestHeader("Accept", "application/json");

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

        public async Task<WebResponse<string>> PostAsync(string uri, byte[] postData, Dictionary<string, string> headers = null)
        {
            using (UnityWebRequest req = new UnityWebRequest(uri, "POST"))
            {
                req.uploadHandler = new UploadHandlerRaw(postData);
                req.downloadHandler = new DownloadHandlerBuffer();
                req.SetRequestHeader("Content-Type", "application/octet-stream");

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
                req.SetRequestHeader("Content-Type", "application/json");
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