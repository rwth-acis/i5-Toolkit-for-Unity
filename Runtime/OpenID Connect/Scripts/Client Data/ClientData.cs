using System;
using UnityEngine;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    [Serializable]
    public class ClientData
    {
        [SerializeField] private string clientId;
        [SerializeField] private string clientSecret;

        public string ClientId { get => clientId; }
        public string ClientSecret { get => clientSecret; }

        public ClientData(string clientId, string clientSecret)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
        }
    }
}