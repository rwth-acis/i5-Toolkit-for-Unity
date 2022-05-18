using System;

namespace i5.Toolkit.Core.RocketChatClient
{
    /// <summary>
    /// Parsed response object that is returned when asking the RocketChat API for the joined groups
    /// </summary>
    [Serializable]
    public class GroupsJoinedResponse
    {
        public ChannelGroup[] groups;
    }
}
