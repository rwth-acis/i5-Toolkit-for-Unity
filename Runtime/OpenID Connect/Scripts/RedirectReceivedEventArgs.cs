using System;
using System.Collections.Generic;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    public class RedirectReceivedEventArgs : EventArgs
    {
        public Dictionary<string, string> RequestParameters { get; private set; }
        public string RedirectUri { get; private set; }

        public RedirectReceivedEventArgs(Dictionary<string,string> requestParameters, string redirectUri)
        {
            RequestParameters = requestParameters;
            RedirectUri = redirectUri;
        }
    }
}
