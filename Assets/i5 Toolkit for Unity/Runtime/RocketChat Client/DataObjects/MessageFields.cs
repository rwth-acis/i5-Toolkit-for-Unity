using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i5.Toolkit.Core.RocketChatClient
{
    [Serializable]
    public class MessageFields
    {
        public string eventName;
        public MessageFieldsArguments[] args;
    }
}
