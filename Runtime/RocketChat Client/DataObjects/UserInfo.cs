using System;

namespace i5.Toolkit.Core.RocketChatClient
{
    /// <summary>
    /// Additional information about the RocketChat user which is e.g. received when querying the logged in user
    /// </summary>
    [Serializable]
    public class UserInfo : User
    {
        public string status;
        public bool active;
        public string _updatedAt;
        public string[] roles;
        public int utcOffset;
        public string avatarETag;
        public string avatarOrigin;
        public string statusConnectoin;
        public string statusDefault;
        public string email;
        public string avatarUrl;
        public bool success;
    }
}
