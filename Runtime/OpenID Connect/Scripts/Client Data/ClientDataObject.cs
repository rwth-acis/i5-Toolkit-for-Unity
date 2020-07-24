using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    /// <summary>
    /// Scriptable object for storing client data in the assets folder as a separate file
    /// This is used so that the client data can be specified and put into a .gitignore file
    /// so that they are not uploaded in public repositories, e.g. on GitHub
    /// </summary>
    [CreateAssetMenu(fileName = "OpenID Connect Client Data", menuName = "i5 Toolkit/OpenID Connect Client Data")]
    public class ClientDataObject : ScriptableObject
    {
        public ClientData clientData;
    }
}