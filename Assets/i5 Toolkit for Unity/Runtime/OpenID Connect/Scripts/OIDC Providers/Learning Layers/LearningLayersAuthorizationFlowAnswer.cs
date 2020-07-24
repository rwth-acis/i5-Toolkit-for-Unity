using System;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    /// <summary>
    /// Data description of the answer that is received after the access token was requested in the
    /// authorization flow
    /// </summary>
    [Serializable]
    public class LearningLayersAuthorizationFlowAnswer
    {
        public string access_token;
        public string token_type;
        public int expires_in;
        public string scope;
        public string id_token;
        public string error;
        public string error_description;
    }
}
