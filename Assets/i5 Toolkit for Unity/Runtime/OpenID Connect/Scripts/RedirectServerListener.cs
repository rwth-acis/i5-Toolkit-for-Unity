using i5.Toolkit.Core.Utilities;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace i5.Toolkit.Core.OpenIDConnectClient
{
    public class RedirectServerListener : IRedirectServerListener
    {
        private Thread serverThread;
        private HttpListener http;

        public event EventHandler<RedirectReceivedEventArgs> RedirectReceived;

        public bool ServerActive { get; private set; }

        public string RedirectUri { get; private set; }

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

        private void Listen()
        {
            http = new HttpListener();
            if (string.IsNullOrEmpty(RedirectUri))
            {
                RedirectUri = GenerateRedirectUri();
            }
            http.Prefixes.Add(RedirectUri);
            http.Start();
            i5Debug.Log("OIDC Redirect server now listening on address " + RedirectUri, this);

            while (ServerActive)
            {
                try
                {
                    HttpListenerContext context = http.GetContext();

                    string responseString = string.Format("<html><head></head><body>Please return to the app</body></html>");
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                    context.Response.ContentLength64 = buffer.Length;
                    var responseOutput = context.Response.OutputStream;
                    responseOutput.Write(buffer, 0, buffer.Length);
                    responseOutput.Close();
                    http.Stop();
                    ServerActive = false;
                    i5Debug.Log("Redirect was received. Server stopped", this);

                    RedirectReceivedEventArgs args = 
                        new RedirectReceivedEventArgs(context.Request.QueryString.ToDictionary(), RedirectUri);
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

        public string GenerateRedirectUri(string protocol = "http")
        {
            string redirectUri = protocol + "://" + IPAddress.Loopback + ":" + GetUnusedPort() + "/";
            RedirectUri = redirectUri;
            return redirectUri;
        }

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
