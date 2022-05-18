using System;

namespace i5.Toolkit.Core.RocketChatClient
{
    /// <summary>
    /// Arguments of a message received by the RocketChat web socket
    /// </summary>
    [Serializable]
    public class MessageFieldsArguments
    {
        public string _id;
        public string rid;
        public string msg;
        public User u;
        public User[] mentions;

        /// <summary>
        /// Gets the raw text of the message; same content as the msg field
        /// Easier to read/understand in code than msg
        /// </summary>
        public string MessageContent { get => msg; }
        /// <summary>
        /// Gets the sender of the message; same content as the u field
        /// Easier to read/understand in code than u
        /// </summary>
        public User Sender { get => u; }
    }
}
