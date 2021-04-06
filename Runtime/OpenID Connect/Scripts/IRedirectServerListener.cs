using System;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    /// <summary>
    /// Contract which specifies the capabilities of the server that listens for redirects
    /// </summary>
    public interface IRedirectServerListener
    {
        /// <summary>
        /// If true, the server is currently active
        /// </summary>
        bool ServerActive { get; }

        string ListeningUri { get; set; }

        /// <summary>
        /// The HTML string which is send as an answer if a request is made to the server
        /// </summary>
        string ResponseString { get; set; }

        /// <summary>
        /// Starts the server
        /// </summary>
        void StartServer();

        /// <summary>
        /// Stops the server immediately, aborting its current operation
        /// </summary>
        void StopServerImmediately();

        /// <summary>
        /// Event which is invoked once a redirect has been received
        /// </summary>
        event EventHandler<RedirectReceivedEventArgs> RedirectReceived;

        /// <summary>
        /// Generates and sets a redirect URI with a free port on which the server will listen once it is started
        /// </summary>
        /// <param name="protocol">The URI scheme that the URI should use</param>
        /// <returns>Returns a URI with a free port on which the server can listen</returns>
        string GenerateListeningUri(string protocol = "http");
    }
}
