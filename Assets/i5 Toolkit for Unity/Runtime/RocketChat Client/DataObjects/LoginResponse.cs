using System;

namespace i5.Toolkit.Core.RocketChatClient
{
    [Serializable]
    public class LoginResponse
    {
        public string status;
        public LoginData data;

        public bool Successful { get => status == "success"; }
    }
}