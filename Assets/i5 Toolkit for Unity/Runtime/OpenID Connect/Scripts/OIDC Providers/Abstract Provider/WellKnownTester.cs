using i5.Toolkit.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    public class WellKnownTester : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            //RequestWellKnownData("https://accounts.google.com/");
            RequestWellKnownData("https://auth.las2peer.org/auth/realms/main");
        }


        /// <summary>
        /// Extracts the required endpoints from the well-known definition of the server
        /// </summary>
        public async void RequestWellKnownData(string serverName)
        {
            IRestConnector RestConnector = new UnityWebRequestRestConnector();
            IJsonSerializer JsonSerializer = new JsonUtilityAdapter();
            WebResponse<string> response = await RestConnector.GetAsync(serverName + "/.well-known/openid-configuration");
            if (response.Successful)
            {
                WellKnownMetaData endpoints = JsonSerializer.FromJson<WellKnownMetaData>(response.Content);
                Debug.Log("Authorization endpoint: " + endpoints.authorization_endpoint);
                Debug.Log("Token endpoint: " + endpoints.token_endpoint);
                Debug.Log("Userinfo endpoint: " + endpoints.userinfo_endpoint);
            }
            else
            {
                i5Debug.LogError("Endpoints could not be fetched. Check whether the provided server.", this);
            }
        }
    }
}
