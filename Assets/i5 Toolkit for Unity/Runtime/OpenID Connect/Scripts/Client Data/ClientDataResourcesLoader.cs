using i5.Toolkit.Core.Utilities;
using i5.Toolkit.Core.Utilities.Async;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    public class ClientDataResourcesLoader : IClientDataLoader
    {
        public async Task<ClientData> LoadClientDataAsync()
        {
            TextAsset clientJson = (TextAsset)await Resources.LoadAsync("oidc_client.json");
            if (clientJson == null)
            {
                i5Debug.LogError("Could not load OpenID Connect client data from resources.\n" +
                    "Make sure that the json file named 'oidc_client' is placed in the Resources folder.", this);
                return null;
            }

            ClientData clientData = JsonUtility.FromJson<ClientData>(clientJson.text);

            if (clientData == null)
            {
                i5Debug.LogError("Could not read OpenID Connect client data.\n" +
                    "Make sure that the json file follows the correct structure.", this);
                return null;
            }
            return clientData;
        }
    }
}