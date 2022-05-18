using System;

namespace i5.Toolkit.Core.RocketChatClient
{
    /// <summary>
    /// Parsed response object that is contains the login data when logging in on the RocketChat API
    /// </summary>
    [Serializable]
    public class LoginData
    {
        public string userId;
        public string authToken;
    }
}
