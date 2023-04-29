using System.Collections.Generic;
using System.Threading.Tasks;

namespace i5.Toolkit.Core.Utilities
{
    public interface IRestConnector
    {
        Task<WebResponse<string>> GetAsync(string uri, Dictionary<string, string> headers = null);

        Task<WebResponse<string>> PostAsync(string uri, string postData, Dictionary<string, string> headers = null);

        Task<WebResponse<string>> PostAsync(string uri, byte[] postData, Dictionary<string, string> headers = null);

        Task<WebResponse<string>> PutAsync(string uri, string postData, Dictionary<string, string> headers = null);

        Task<WebResponse<string>> DeleteAsync(string uri, Dictionary<string, string> headers = null);
    }
}