using System;
using System.Collections.Generic;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    public class RedirectReceivedEventArgs : EventArgs
    {
        public Dictionary<string, string> RedirectParameters { get; private set; }
        public string RedirectUri { get; private set; }

        public RedirectReceivedEventArgs(Dictionary<string,string> requestParameters, string redirectUri)
        {
            RedirectParameters = requestParameters;
            RedirectUri = redirectUri;
        }
    }
}
