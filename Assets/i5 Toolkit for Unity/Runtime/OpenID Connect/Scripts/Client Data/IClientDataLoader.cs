using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    /// <summary>
    /// Contract for loading client data
    /// </summary>
    public interface IClientDataLoader
    {
        Task<ClientData> LoadClientDataAsync();
    }
}