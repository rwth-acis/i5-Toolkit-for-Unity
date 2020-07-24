using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    /// <summary>
    /// Configuration object for the OpenID Connect Service
    /// This object can be edited in the inspector and it can be passed to the service
    /// </summary>
    [Serializable]
    public class OpenIDConnectServiceConfiguration
    {
        [Tooltip("The Web page which should be shown to the user after the redirect." +
            "\nIt should tell the user to return to the app.")]
        public TextAsset redirectPage;

        public ClientDataObject clientDataObject;
    }
}