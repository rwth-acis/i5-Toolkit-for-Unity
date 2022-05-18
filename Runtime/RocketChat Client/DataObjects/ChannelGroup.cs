using System;

namespace i5.Toolkit.Core.RocketChatClient
{
    /// <summary>
    /// A combined data object for representing channels and groups in the RocketChat API
    /// </summary>
    [Serializable]
    public class ChannelGroup
    {
        public string _id;
        public string fname;
        public string description;
        public string name;
        public int msgs;
        public int userCount;
    }
}
