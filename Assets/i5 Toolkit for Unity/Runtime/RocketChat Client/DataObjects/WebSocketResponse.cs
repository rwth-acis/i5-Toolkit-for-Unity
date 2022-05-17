using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i5.Toolkit.Core.RocketChatClient
{
    [Serializable]
    public class WebSocketResponse
    {
        /// <summary>
        /// A category which labels this response
        /// This is not the message content by a user
        /// </summary>
        public string msg;

        public MessageFields fields;
    }
}
