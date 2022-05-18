using System;

namespace i5.Toolkit.Core.RocketChatClient
{
    /// <summary>
    /// Fields which are part of a response of the RocketChat web socket
    /// </summary>
    [Serializable]
    public class MessageFields
    {
        public string eventName;
        public MessageFieldsArguments[] args;
    }
}
