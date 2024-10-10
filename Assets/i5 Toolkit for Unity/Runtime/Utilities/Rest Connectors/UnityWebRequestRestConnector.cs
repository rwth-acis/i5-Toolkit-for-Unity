using i5.Toolkit.Core.Utilities.Async;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace i5.Toolkit.Core.Utilities
{
    public class UnityWebRequestRestConnector : IRestConnector
    {
        public virtual async Task<WebResponse<string>> DeleteAsync(string uri, Dictionary<string, string> headers = null)
        {
            using (UnityWebRequest req = UnityWebRequest.Delete(uri))
            {
                AddHeaders(req, headers);
                req.downloadHandler = new DownloadHandlerBuffer();
                await req.SendWebRequest();

                if (req.result == UnityWebRequest.Result.Success)
                {
					return new WebResponse<string>(req.downloadHandler.text, req.downloadHandler.data, req.responseCode);
				}
                else
                {
					return new WebResponse<string>(false, req.downloadHandler.text, req.downloadHandler.data, req.responseCode, req.error);
				}
            }
        }

        public virtual async Task<WebResponse<string>> GetAsync(string uri, Dictionary<string, string> headers = null)
        {
            using (UnityWebRequest req = UnityWebRequest.Get(uri))
            {
                AddHeaders(req, headers);
                await req.SendWebRequest();

                if (req.result == UnityWebRequest.Result.Success)
                {
					return new WebResponse<string>(req.downloadHandler.text, req.downloadHandler.data, req.responseCode);
				}
                else
                {
					return new WebResponse<string>(false, req.downloadHandler.text, req.downloadHandler.data, req.responseCode, req.error);
				}
            }
        }

        public virtual async Task<WebResponse<string>> PostAsync(string uri, string putJson, Dictionary<string, string> headers = null)
        {
            using (UnityWebRequest req = UnityWebRequest.PostWwwForm(uri, putJson))
            {
                req.downloadHandler = new DownloadHandlerBuffer();
                req.SetRequestHeader("Content-Type", "application/json");
                req.SetRequestHeader("Accept", "application/json");

                AddHeaders(req, headers);
                await req.SendWebRequest();

                if (req.result == UnityWebRequest.Result.Success)
                {
					return new WebResponse<string>(req.downloadHandler.text, req.downloadHandler.data, req.responseCode);
                }
                else
                {
					return new WebResponse<string>(false, req.downloadHandler.text, req.downloadHandler.data, req.responseCode, req.error);
				}
            }
        }

        public virtual async Task<WebResponse<string>> PostAsync(string uri, byte[] postData, Dictionary<string, string> headers = null)
        {
			return await PostPutDataAsync(uri, "POST", postData, "application/octet-stream", headers);
		}

        public virtual async Task<WebResponse<string>> PutAsync(string uri, string putJson, Dictionary<string, string> headers = null)
        {
            using (UnityWebRequest req = UnityWebRequest.Put(uri, putJson))
            {
                req.SetRequestHeader("Content-Type", "application/json");
                AddHeaders(req, headers);
                await req.SendWebRequest();

                if (req.result == UnityWebRequest.Result.Success)
                {
					return new WebResponse<string>(req.downloadHandler.text, req.downloadHandler.data, req.responseCode);
                }
                else
                {
					return new WebResponse<string>(false, req.downloadHandler.text, req.downloadHandler.data, req.responseCode, req.error);
				}
            }
        }

        public virtual async Task<WebResponse<string>> PutAsync(string uri, byte[] putData, Dictionary<string, string> headers = null)
        {
            return await PostPutDataAsync(uri, "PUT", putData, "application/octet-stream", headers);
		}

        protected async Task<WebResponse<string>> PostPutDataAsync(string uri, string method, byte[] data, string contentType, Dictionary<string, string> headers = null)
        {
            using (UnityWebRequest req = new UnityWebRequest(uri, method))
			{
				req.uploadHandler = new UploadHandlerRaw(data);
				req.downloadHandler = new DownloadHandlerBuffer();
				req.SetRequestHeader("Content-Type", contentType);

				AddHeaders(req, headers);

				await req.SendWebRequest();

				if (req.result == UnityWebRequest.Result.Success)
				{
					return new WebResponse<string>(req.downloadHandler.text, req.downloadHandler.data, req.responseCode);
				}
				else
				{
					return new WebResponse<string>(false, req.downloadHandler.text, req.downloadHandler.data, req.responseCode, req.error);
				}
			}
		}


		protected void AddHeaders(UnityWebRequest req, Dictionary<string, string> headers)
        {
            if (headers == null)
            {
                return;
            }
            foreach (KeyValuePair<string, string> header in headers)
            {
                req.SetRequestHeader(header.Key, header.Value);
            }
        }
    }
}