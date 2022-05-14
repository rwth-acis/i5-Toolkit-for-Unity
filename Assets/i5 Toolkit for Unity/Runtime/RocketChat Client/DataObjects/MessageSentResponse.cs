using System;

namespace i5.Toolkit.Core.RocketChatClient
{
    [Serializable]
    public class MessageSentResponse
    {
        public long ts;
        public string channel;
        public bool success;        
    }
}
