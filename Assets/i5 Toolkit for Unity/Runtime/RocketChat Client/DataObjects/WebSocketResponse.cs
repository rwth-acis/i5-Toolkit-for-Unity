using System;

namespace i5.Toolkit.Core.RocketChatClient
{
    /// <summary>
    /// Parsed response object which is sent by the RocketChat API for streamed web socket messages
    /// </summary>
    [Serializable]
    public class WebSocketResponse
    {
        /// <summary>
        /// A category which labels this response
        /// This is not the message content by a user
        /// </summary>
        public string msg;
        /// <summary>
        /// Additional fields in the message by the API
        /// </summary>
        public MessageFields fields;
    }
}
