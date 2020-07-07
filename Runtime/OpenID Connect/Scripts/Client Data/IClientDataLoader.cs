using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    public interface IClientDataLoader
    {
        Task<ClientData> LoadClientDataAsync();
    }
}