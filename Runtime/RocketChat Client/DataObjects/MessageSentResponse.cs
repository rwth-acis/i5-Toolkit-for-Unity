using System;

namespace i5.Toolkit.Core.RocketChatClient
{
    /// <summary>
    /// Parsed response object of the RocketChat API for sending a message
    /// </summary>
    [Serializable]
    public class MessageSentResponse
    {
        public long ts;
        public string channel;
        public bool success;
    }
}
