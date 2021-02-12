using i5.Toolkit.Core.Utilities;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    /// <summary>
    /// Implementation of a server that serves the redirect after the OpenID Connect login
    /// </summary>
    public class RedirectServerListener : IRedirectServerListener
    {
        private Thread serverThread;
        private HttpListener http;

        /// <summary>
        /// Event which is raised once a redirect is received
        /// </summary>
        public event EventHandler<RedirectReceivedEventArgs> RedirectReceived;

        /// <summary>
        /// True if the server is active
        /// </summary>
        public bool ServerActive { get; private set; }

        /// <summary>
        /// The URI whrere the server listens for the redirect
        /// </summary>
        public string ListeningUri { get; set; }

        /// <summary>
        /// HTML response that is given on the redirect request
        /// </summary>
        public string ResponseString { get; set; }

        /// <summary>
        /// Creates a new instance of the RedirectServerListener
        /// </summary>
        public RedirectServerListener()
        {
            ResponseString = "<html><head></head><body>Please return to the app</body></html>";
        }

        /// <summary>
        /// Starts the server
        /// </summary>
        public void StartServer()
        {
            if (serverThread == null)
            {
                ServerActive = true;
                serverThread = new Thread(Listen);
                serverThread.Start();
            }
            else
            {
                i5Debug.LogWarning("Server is already running. There is no need to start it at the moment.", this);
            }
        }

        /// <summary>
        /// Stops the server immediately and aborts the current operation
        /// </summary>
        public void StopServerImmediately()
        {
            if (serverThread != null)
            {
                serverThread.Abort();
                // check if http is not null (it might not even have run yet)
                if (http != null)
                {
                    http.Stop();
                }
                ServerActive = false;
                i5Debug.Log("HTTPListener stopped.", this);
            }
            else
            {
                i5Debug.LogWarning("Server is already stopped. There is no need to stop it at the moment.", this);
            }
        }

        /// <summary>
        /// Listens for requests
        /// This method should be executed on a separate thread
        /// </summary>
        private void Listen()
        {
            http = new HttpListener();
            if (string.IsNullOrEmpty(ListeningUri))
            {
                ListeningUri = GenerateListeningUri();
            }
            string origListeningUri = ListeningUri;
            // some services require the IP address and port without a trailing slash
            // however, the server requires it to start
            if (!ListeningUri.EndsWith("/"))
            {
                i5Debug.Log(
                    "Server's listening URI needs to end with a slash. " +
                    "I will automatically append this slash.", this);
                ListeningUri += "/";
            }
            http.Prefixes.Add(ListeningUri);
            http.Start();
            i5Debug.Log("OIDC Redirect server now listening on address " + ListeningUri, this);

            while (ServerActive)
            {
                try
                {
                    HttpListenerContext context = http.GetContext();

                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(ResponseString);
                    context.Response.ContentLength64 = buffer.Length;
                    var responseOutput = context.Response.OutputStream;
                    responseOutput.Write(buffer, 0, buffer.Length);
                    responseOutput.Close();
                    http.Stop();
                    ServerActive = false;
                    i5Debug.Log("Redirect was received. Server stopped", this);

                    RedirectReceivedEventArgs args = 
                        new RedirectReceivedEventArgs(context.Request.QueryString.ToDictionary(), origListeningUri);
                    RedirectReceived?.Invoke(this, args);
                }
                catch (ThreadAbortException)
                {
                    i5Debug.Log("OIDC server was shut shown manually.", this);
                }
                catch (Exception e)
                {
                    i5Debug.LogError("OIDC server encountered an error: " + e.ToString() + "\nShutting server down.", this);
                }
                finally
                {
                    serverThread = null;
                }
            }
        }

        /// <summary>
        /// Generates a redirect URI where the server can listen for the redirect
        /// </summary>
        /// <param name="protocol">Specify a custom URI schema.
        /// If the app version of this registers as a handler for the URI schema, the app will be opened again.</param>
        /// <returns>Returns a free URI where the server can listen</returns>
        public string GenerateListeningUri(string protocol = "http")
        {
            string redirectUri = protocol + "://" + IPAddress.Loopback + ":" + GetUnusedPort() + "/";
            ListeningUri = redirectUri;
            return redirectUri;
        }

        /// <summary>
        /// Gets an unused port on which the server can listen
        /// </summary>
        /// <returns>Returns the unused port</returns>
        private static int GetUnusedPort()
        {
            TcpListener listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }
    }
}
