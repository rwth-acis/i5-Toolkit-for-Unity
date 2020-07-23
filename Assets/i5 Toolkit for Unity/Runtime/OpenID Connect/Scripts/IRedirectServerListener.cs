using System;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    public interface IRedirectServerListener
    {
        bool ServerActive { get; }

        void StartServer();
        void StopServerImmediately();

        event EventHandler<RedirectReceivedEventArgs> RedirectReceived;

        string GenerateRedirectUri(string protocol = "http");
    }
}
