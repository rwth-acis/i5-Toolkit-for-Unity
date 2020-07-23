using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    [Serializable]
    public class OpenIDConnectServiceConfiguration
    {
        [Tooltip("The Web page which should be shown to the user after the redirect." +
            "\nIt should tell the user to return to the app.")]
        public TextAsset redirectPage;

        public ClientDataObject clientDataObject;
    }
}