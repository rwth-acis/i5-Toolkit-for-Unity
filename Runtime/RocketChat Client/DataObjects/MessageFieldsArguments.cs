using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i5.Toolkit.Core.RocketChatClient
{
    [Serializable]
    public class MessageFieldsArguments
    {
        public string _id;
        public string rid;
        public string msg;
        public User u;
        public User[] mentions;

        public string MessageContent { get => msg; }
        public User Sender { get => u; }
    }
}
