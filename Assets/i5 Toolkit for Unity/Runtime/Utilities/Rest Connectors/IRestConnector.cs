using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.Utilities
{
    public interface IRestConnector
    {
        Task<WebResponse<string>> GetAsync(string uri);

        Task<WebResponse<string>> PostAsync(string uri, string postData);

        Task<WebResponse<string>> PutAsync(string uri, string postData);

        Task<WebResponse<string>> DeleteAsync(string uri);
    }
}