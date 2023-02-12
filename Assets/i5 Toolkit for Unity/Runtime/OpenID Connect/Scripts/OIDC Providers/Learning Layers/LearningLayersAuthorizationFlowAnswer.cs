using System;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    /// <summary>
    /// Data description of the answer that is received after the access token was requested in the
    /// authorization flow of LearningLayers
    /// </summary>
    [Serializable]
    public class LearningLayersAuthorizationFlowAnswer: AbstractAuthorizationFlowAnswer
    {
        public int expires_in;
        public string id_token;
        public string error;
        public string error_description;
    }
}
