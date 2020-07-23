using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    [CreateAssetMenu(fileName = "OpenID Connect Client Data", menuName = "i5 Toolkit/OpenID Connect Client Data")]
    public class ClientDataObject : ScriptableObject
    {
        public ClientData clientData;
    }
}