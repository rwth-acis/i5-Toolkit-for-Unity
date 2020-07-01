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

        public static ClientData LoadFromResources()
        {
            TextAsset clientJson = Resources.Load<TextAsset>("oidc_client.json");
            if (clientJson == null)
            {
                Debug.LogError("Could not load OpenID Connect client data from resources.\n" +
                    "Make sure that the json file named 'oidc_client' is placed in the Resources folder.");
                return null;
            }

            ClientData clientData = JsonUtility.FromJson<ClientData>(clientJson.text);

            if (clientData == null)
            {
                Debug.LogError("Could not read OpenID Connect client data.\n" +
                    "Make sure that the json file follows the correct structure.");
                return null;
            }
            return clientData;
        }
    }
}