using System;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    /// <summary>
    /// Data description of the answer that is received after the access token was requested in the
    /// authorization flow
    /// </summary>
    [Serializable]
    public class AbstractAuthorizationFlowAnswer
    {
        public string access_token;
        public string token_type;
        public string scope;
    }
}
