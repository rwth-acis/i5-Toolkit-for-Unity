using System;

namespace i5.Toolkit.Core.RocketChatClient
{
    [Serializable]
    public class UserInfo
    {
        public string _id;
        public string status;
        public bool active;
        public string _updatedAt;
        public string[] roles;
        public string username;
        public string name;
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
