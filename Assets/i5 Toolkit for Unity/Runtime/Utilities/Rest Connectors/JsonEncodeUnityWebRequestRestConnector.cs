using i5.Toolkit.Core.Utilities.Async;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace i5.Toolkit.Core.Utilities
{
    public class JsonEncodeUnityWebRequestRestConnector : UnityWebRequestRestConnector
    {
        public override async Task<WebResponse<string>> PostAsync(string uri, string postJson, Dictionary<string, string> headers = null)
        {
			byte[] data = new UTF8Encoding().GetBytes(postJson);
            return await PostPutDataAsync(uri, "POST", data, "application/json", headers);
        }

        public override async Task<WebResponse<string>> PutAsync(string uri, string postData, Dictionary<string, string> headers = null)
        {
            byte[] data = new UTF8Encoding().GetBytes(postData);
            return await PostPutDataAsync(uri, "PUT", data, "application/json", headers);
        }
    }
}