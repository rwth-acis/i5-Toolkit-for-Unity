using System;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    /// <summary>
    /// Data description of the answer that is received after the access token was requested in the
    /// authorization flow of Google
    /// </summary>
    [Serializable]
    public class GoogleAuthorizationFlowAnswer : AbstractAuthorizationFlowAnswer
    {
        public string id_token;
        public string expires_in;
    }
}
