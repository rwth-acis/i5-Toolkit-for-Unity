using System;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    public interface IRedirectServerListener
    {
        void StartServer();
        void StopServerImmediately();

        event EventHandler<RedirectReceivedEventArgs> RedirectReceived;

        string GenerateRedirectUri(string protocol = "http");
    }
}
