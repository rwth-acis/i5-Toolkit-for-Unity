using System;

namespace i5.Toolkit.Core.RocketChatClient
{
    /// <summary>
    /// A base user object for the RocketChat API
    /// </summary>
    [Serializable]
    public class User
    {
        public string _id;
        public string username;
        public string name;
    }
}
