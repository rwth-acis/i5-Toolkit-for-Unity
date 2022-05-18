using System;

namespace i5.Toolkit.Core.RocketChatClient
{
    /// <summary>
    /// Parsed response object that is returned when asking the RocketChat API for the joined channels
    /// </summary>
    [Serializable]
    public class ChannelsJoinedResponse
    {
        public ChannelGroup[] channels;
    }
}
