using i5.Toolkit.Core.Utilities;
using i5.Toolkit.Core.Utilities.Async;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    /// <summary>
    /// Loader that can load ClientData json files which are stored in the resources folder
    /// </summary>
    public class ClientDataResourcesLoader : IClientDataLoader
    {
        /// <summary>
        /// Loads the client data from the resource folder
        /// The client data have to be named "oidc_client.json"
        /// </summary>
        /// <returns>The loaded ClientData if they exist, otherwise null</returns>
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