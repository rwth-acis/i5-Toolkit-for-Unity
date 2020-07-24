using System;
using UnityEngine;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    /// <summary>
    /// Describes configurations of client data
    /// </summary>
    [Serializable]
    public class ClientData
    {
        [Tooltip("The id of the client that is registered at the provider")]
        [SerializeField] private string clientId;
        [Tooltip("The secret of the client that was issued by the provider")]
        [SerializeField] private string clientSecret;

        /// <summary>
        /// The id of the client that is registered at the provider
        /// </summary>
        public string ClientId { get => clientId; }
        /// <summary>
        /// The secret of the client that was issued by the provider
        /// </summary>
        public string ClientSecret { get => clientSecret; }

        /// <summary>
        /// Creates a new client data instance with the given parameters
        /// </summary>
        /// <param name="clientId">The id of the client that is registered at the provider</param>
        /// <param name="clientSecret">The secret of the client that was issued by the provider</param>
        public ClientData(string clientId, string clientSecret)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
        }
    }
}