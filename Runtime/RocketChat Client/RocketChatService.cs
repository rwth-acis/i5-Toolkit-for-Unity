using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities;
using i5.Toolkit.Core.Utilities.Async;
using System;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace i5.Toolkit.Core.RocketChatClient
{
    /// <summary>
    /// A client for accessing the Rocket.Chat API
    /// </summary>
    public class RocketChatService : IService
    {

        public enum RequestType
        {
            GET,
            POST
        }

        #region Fields and Properties

        private CancellationTokenSource subscribeCancellationTokenSource = new CancellationTokenSource();

        private string username;
        private string password;

        // WebSocket fields
        private ClientWebSocket socket = new ClientWebSocket();
        private CancellationToken cancellationToken;
        private bool isWebSocketConnected = false;
        private bool isWebSocketLoggedIn = false;
        private bool isWebSocketSubscribed = false;

        /// <summary>
        /// The address where the Rocket.Chat server is hosted
        /// </summary>
        public string HostAddress
        {
            get; set;
        }

        /// <summary>
        /// Event delegate for receiving messages
        /// </summary>
        /// <param name="messageArgs">The arguments of the received message, e.g. contains the message's text or the sender</param>
        public delegate void ReceivedMessageHandler(MessageFieldsArguments messageArgs);

        /// <summary>
        /// Fired when the client receives a WebSocket message from a room.
        /// </summary>
        public event ReceivedMessageHandler OnMessageReceived;

        /// <summary>
        /// AuthToken (X-Auth-Token) of the user.
        /// It is automatically set during the login.
        /// </summary>
        public string AuthToken
        {
            get; private set;
        }

        /// <summary>
        /// UserID (X-User-Id) of the user.
        /// It is automatically set during the login.
        /// </summary>
        public string UserID
        {
            get; private set;
        }

        /// <summary>
        /// Module for serializing and de-serializing JSON files
        /// Initialized by default with the JsonUtilityAdapter
        /// </summary>
        public IJsonSerializer JsonSerializer { get; set; } = new JsonUtilityAdapter();

        #endregion

        #region IService Implementation

        /// <summary>
        /// Initializes the service as it gets registerd with a ServiceManager
        /// </summary>
        /// <param name="owner">The service manager which now owns this service</param>
        public void Initialize(IServiceManager owner)
        {
            i5Debug.Log("Host address: " + HostAddress, this);
            if (string.IsNullOrEmpty(HostAddress))
            {
                i5Debug.LogError("Please use the contructor to initialize the host address first", this);
            }
        }

        /// <summary>
        /// Cleans up the service as it is deregistered from the service manager
        /// </summary>
        public void Cleanup()
        {
            isWebSocketConnected = false;
            isWebSocketLoggedIn = false;
            isWebSocketSubscribed = false;
            subscribeCancellationTokenSource.Cancel();
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a new Rocket.Chat client instance
        /// </summary>
        /// <param name="hostAddress">The URL address where the Rocket.Chat server is hosted</param>
        public RocketChatService(string hostAddress)
        {
            cancellationToken = subscribeCancellationTokenSource.Token;
            this.HostAddress = hostAddress;
        }

        /// <summary>
        /// Log in to the server with a username and password.
        /// It sets the AuthToken and UserID properties.
        /// See https://developer.rocket.chat/reference/api/rest-api/endpoints/other-important-endpoints/authentication-endpoints/login
        /// </summary>
        /// <param name="username">The username of the user who wants to log in</param>
        /// <param name="password">The password of the user who wants to log in</param>
        /// <returns>Returns the parsed login response</returns>
        public async Task<WebResponse<LoginResponse>> LoginAsync(string username, string password)
        {
            string payload = $"{{ \"username\": \"{username}\", \"password\": \"{password}\" }}";
            WebResponse<LoginResponse> result = await LoginRequestAsync(payload);
            bool success = result.Successful;
            // cahce the username and password for the web socket if the login was successful
            if (success)
            {
                this.username = username;
                this.password = password;
            }
            else
            {
                this.username = "";
                this.password = "";
            }
            return result;
        }

        /// <summary>
        /// Log in to the server with an auth token.
        /// It sets the AuthToken and UserID properties.
        /// See https://developer.rocket.chat/reference/api/rest-api/endpoints/other-important-endpoints/authentication-endpoints/login
        /// </summary>
        /// <param name="authToken">The auth token that identifies and authorized the user who wants to log in</param>
        /// <returns>Returns true if the login was successful, otherwise false</returns>
        public async Task<WebResponse<LoginResponse>> LoginAsync(string authToken)
        {
            string payload = $"{{\"resume\": \"{authToken}\"}}";
            return await LoginRequestAsync(payload);
        }

        /// <summary>
        /// Post a message to a given room (channel, team, direct message etc.) of the user.
        /// Requires login first.
        /// See https://developer.rocket.chat/reference/api/rest-api/endpoints/team-collaboration-endpoints/chat-endpoints/postmessage
        /// </summary>
        /// <param name="targetID">rid of the room, channel name (#) or user name (@)</param>
        /// <returns>Returns true if the message was successfully sent</returns>
        public async Task<WebResponse<MessageSentResponse>> PostMessageAsync(string targetID, string text = "")
        {
            WebResponse<string> response = await SendEncodedPostRequestAsync($"https://{HostAddress}/api/v1/chat.postMessage", $"{{ \"channel\": \"{targetID}\", \"text\": \"{text}\" }}", true);
            if (!response.Successful)
            {
                i5Debug.LogError("Could not send message", this);
                return new WebResponse<MessageSentResponse>(response.ErrorMessage, response.Code);
            }
            MessageSentResponse messageSentResponse = JsonSerializer.FromJson<MessageSentResponse>(response.Content);
            return new WebResponse<MessageSentResponse>(messageSentResponse, response.ByteData, response.Code);
        }

        /// <summary>
        /// Get the profile information of the logged-in user.
        /// Requires login first.
        /// See https://developer.rocket.chat/reference/api/rest-api/endpoints/other-important-endpoints/authentication-endpoints/me
        /// </summary>
        public async Task<WebResponse<UserInfo>> GetMeAsync()
        {
            WebResponse<string> response = await SendHttpRequestAsync(RequestType.GET, "/api/v1/me");
            if (!response.Successful)
            {
                i5Debug.LogError("Unable to get information about logged in user.\n" + response.ErrorMessage, this);
                return new WebResponse<UserInfo>(response.ErrorMessage, response.Code);
            }
            UserInfo userInfo = JsonSerializer.FromJson<UserInfo>(response.Content);
            return new WebResponse<UserInfo>(userInfo, response.ByteData, response.Code);
        }

        /// <summary>
        /// Get the user's channel list of joined channels. Requires login first.
        /// Note that this only includes public channels. For private channels, get the user's groups.
        /// See https://developer.rocket.chat/reference/api/rest-api/endpoints/team-collaboration-endpoints/channels-endpoints/list
        /// </summary>
        /// <returns>Retunrs a WebResponse with the server's answer and an array of the user's channels</returns>
        public async Task<WebResponse<ChannelGroup[]>> GetChannelListJoinedAsync()
        {
            WebResponse<string> response = await SendHttpRequestAsync(RequestType.GET, "/api/v1/channels.list.joined");
            if (!response.Successful)
            {
                i5Debug.LogError("Could not retrieve channels", this);
                return new WebResponse<ChannelGroup[]>(response.ErrorMessage, response.Code);
            }
            ChannelsJoinedResponse channelsJoined = JsonSerializer.FromJson<ChannelsJoinedResponse>(response.Content);
            return new WebResponse<ChannelGroup[]>(channelsJoined.channels, response.ByteData, response.Code);
        }

        /// <summary>
        /// Gets the user's list of joined groups. Requires login first.
        /// This does not only include groups but also private channels.
        /// See https://developer.rocket.chat/reference/api/rest-api/endpoints/team-collaboration-endpoints/groups-endpoints/list
        /// </summary>
        /// <returns>Returns a WebResponse of the server and an array of the user's groups</returns>
        public async Task<WebResponse<ChannelGroup[]>> GetGroupListAsync()
        {
            WebResponse<string> response = await SendHttpRequestAsync(RequestType.GET, "/api/v1/groups.list");
            if (!response.Successful)
            {
                i5Debug.LogError("Could not retrieve groups", this);
                return new WebResponse<ChannelGroup[]>(response.ErrorMessage, response.Code);
            }
            GroupsJoinedResponse groupsJoined = JsonSerializer.FromJson<GroupsJoinedResponse>(response.Content);
            return new WebResponse<ChannelGroup[]>(groupsJoined.groups, response.ByteData, response.Code);
        }

        /// <summary>
        /// Send an arbitrary HTTP request to the host. Supports only GET and POST with application/json header.
        /// APIs See https://developer.rocket.chat/reference/api/rest-api
        /// </summary>
        /// <param name="type"> request type, supports GET and POST</param>
        /// <param name="apiSuffix">the api string behind the host, e.g. /api/v1/login </param>
        /// <param name="payload">Payload of a POST request, optional.</param>
        /// <returns>WebResponse with the server's answer</returns>
        public async Task<WebResponse<string>> SendHttpRequestAsync(RequestType type, string apiSuffix, string payload = "")
        {
            if (type == RequestType.GET)
            {
                using (UnityWebRequest request = UnityWebRequest.Get($"https://{HostAddress}{apiSuffix}"))
                {
                    request.SetRequestHeader("X-Auth-Token", AuthToken);
                    request.SetRequestHeader("X-User-Id", UserID);
                    request.SetRequestHeader("Content-type", "application/json");
                    await request.SendWebRequest();
                    if (!string.IsNullOrEmpty(request.error))
                    {
                        i5Debug.LogError($"Error with http Get Request: {request.error}\n{request.downloadHandler.text}", this);
                        return new WebResponse<string>(request.downloadHandler.text, request.responseCode);
                    }
                    else
                    {
                        return new WebResponse<string>(request.downloadHandler.text, request.downloadHandler.data, request.responseCode);
                    }
                }
            }
            else
            {
                WebResponse<string> response = await SendEncodedPostRequestAsync($"https://{HostAddress}{apiSuffix}", payload, true);
                return response;
            }
        }

        /// <summary>
        /// Start listening for messages in a particular channel
        /// Starts a web socket that keeps up a connection to the API.
        /// See https://developer.rocket.chat/reference/api/realtime-api/subscriptions/stream-room-messages
        /// </summary>
        /// <param name="roomID">rid of the room (must be the internal id, the human-readable name starting with the '#' does not work)</param>
        /// <param name="uniqueID">a unique ID of this subscription (can be chosen arbitrarily)</param>
        public async Task SubscribeRoomMessageAsync(string roomID, string uniqueID)
        {
            await WebSocketConnectAsync();
            await WebSocketLoginAsync(uniqueID);
            // Subscribe
            string subsribeRequest = $"{{\"msg\": \"sub\",\"id\": \"{uniqueID}\",\"name\": \"stream-room-messages\",\"params\":[\"{roomID}\",false]}}";
            await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(subsribeRequest)), WebSocketMessageType.Binary, true, cancellationToken);
            isWebSocketSubscribed = true;
            // ReceiveMessage
            StreamMessageAsync();
            Debug.Log("Subscription stream opened.");
        }

        /// <summary>
        /// Stop listening for messages in a room, given the ID of the former subscription.
        /// Closes this particular Web socket connection to the API
        /// </summary>
        /// <param name="uniqueID">The subscribtion ID that identifies the subscription</param>
        public async Task UnsubscribeRoomMessageAsync(string uniqueID)
        {
            if (isWebSocketSubscribed)
            {
                string unSubMessage = $"{{\"msg\": \"unsub\",\"id\": \"{uniqueID}\"}}";
                await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(unSubMessage)), WebSocketMessageType.Binary, true, cancellationToken);
                isWebSocketSubscribed = false;
                i5Debug.Log("Unsubscribed stream with id: " + uniqueID, this);
            }
            else
            {
                i5Debug.LogWarning("You have no active subscription.", this);
            }
        }

        /// <summary>
        /// Send an arbitrary WebSocket request to the host.
        /// APIs See https://developer.rocket.chat/reference/api/realtime-api
        /// </summary>
        /// <param name="uniqueID">id of the request</param>
        /// <param name="message">message to send</param>
        /// <returns>Returns the API's answer as a WebResponse object</returns>
        public async Task<WebResponse<string>> SendWebSocketRequestAsync(string uniqueID, string message)
        {
            await WebSocketConnectAsync();
            await WebSocketLoginAsync(uniqueID);
            await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)), WebSocketMessageType.Binary, true, cancellationToken);
            var response = new byte[1024];
            await socket.ReceiveAsync(new ArraySegment<byte>(response), new CancellationToken());
            var messageString = Encoding.UTF8.GetString(response);
            return new WebResponse<string>(messageString, response, 200);
        }

        #endregion

        #region Private Methods

        // combined login workflow where the payload is already set
        private async Task<WebResponse<LoginResponse>> LoginRequestAsync(string payload)
        {
            WebResponse<string> response = await SendEncodedPostRequestAsync(
                $"https://{HostAddress}/api/v1/login",
                payload,
                false);

            if (!response.Successful)
            {
                i5Debug.LogError("Could not log in", this);
                UserID = "";
                AuthToken = "";
                return new WebResponse<LoginResponse>(response.ErrorMessage, response.Code);
            }

            LoginResponse loginResponse = JsonSerializer.FromJson<LoginResponse>(response.Content);

            UserID = loginResponse.data.userId;
            AuthToken = loginResponse.data.authToken;
            return new WebResponse<LoginResponse>(loginResponse, response.ByteData, response.Code);
        }

        // Encrypt a string using SHA256
        // Useful for encrypting the password
        private string SHA256Encrypt(string data)
        {
            byte[] SHA256 = Encoding.UTF8.GetBytes(data);
            SHA256Managed encryptor = new SHA256Managed();
            byte[] hash = encryptor.ComputeHash(SHA256);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        // encodes the post requests so that they are accepted by Rocket.Chat's API
        private async Task<WebResponse<string>> SendEncodedPostRequestAsync(string url, string bodyData, bool loggedIn)
        {
            using (UnityWebRequest request = UnityWebRequest.Put(url, bodyData))
            {
                // to get the correct encoding, we need to first create a put request and then change its method to a post request
                request.method = "POST";
                request.SetRequestHeader("Content-type", "application/json");
                request.uploadHandler.contentType = "application/json";
                if (loggedIn)
                {
                    if (string.IsNullOrEmpty(UserID) || string.IsNullOrEmpty(AuthToken))
                    {
                        i5Debug.LogError("Cannot post a logged in request if the userID oder authToken are not set.", this);
                        return new WebResponse<string>("not logged in", -1);
                    }
                    request.SetRequestHeader("X-User-Id", UserID);
                    request.SetRequestHeader("X-Auth-Token", AuthToken);
                }
                await request.SendWebRequest();
                if (!string.IsNullOrEmpty(request.error))
                {
                    i5Debug.LogError($"RocketChat request yielded error {request.error}", this);
                    return new WebResponse<string>(request.downloadHandler.text, request.responseCode);
                }
                else
                {
                    return new WebResponse<string>(request.downloadHandler.text, request.downloadHandler.data, request.responseCode);
                }
            }
        }

        // Stream the message in an async thread
        private async void StreamMessageAsync()
        {
            while (isWebSocketSubscribed)
            {
                var message = new byte[1024];
                await socket.ReceiveAsync(new ArraySegment<byte>(message), new CancellationToken());
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                var messageString = Encoding.UTF8.GetString(message);
                
                WebSocketResponse messageObj = JsonSerializer.FromJson<WebSocketResponse>(messageString);

                if (messageObj.msg == "ping")
                {
                    string pongMessage = "{\"msg\": \"pong\"}";
                    await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(pongMessage)), WebSocketMessageType.Binary, true, cancellationToken);
                }
                else if (messageObj.fields.args != null && messageObj.fields.args.Length > 0)
                {
                    OnMessageReceived?.Invoke(messageObj.fields.args[0]);
                }
            }
            i5Debug.Log("Subscription stream closed", this);
        }

        // Connect the socket to the host.
        private async Task WebSocketConnectAsync()
        {
            if (!isWebSocketConnected)
            {
                Uri uri = new Uri($"wss://{HostAddress}/websocket");
                await socket.ConnectAsync(uri, cancellationToken);
                string connectMessage = "{\"msg\": \"connect\",\"version\": \"1\",\"support\": [\"1\"]}";
                await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(connectMessage)), WebSocketMessageType.Binary, true, cancellationToken);
                isWebSocketConnected = true;
                var message = new byte[1024];
                await socket.ReceiveAsync(new ArraySegment<byte>(message), new CancellationToken());
                var messageString = Encoding.UTF8.GetString(message);
                Debug.Log(messageString);
            }
            else
            {
                i5Debug.LogWarning("Web Socket is already connected", this);
            }
        }

        // Login to the host
        private async Task WebSocketLoginAsync(string uniqueID)
        {
            if (!isWebSocketLoggedIn)
            {
                if (!string.IsNullOrEmpty(AuthToken))
                {
                    string loginMessage = $"{{\"msg\": \"method\",\"method\": \"login\",\"id\":\"{uniqueID}\"," + $"\"params\":[{{\"resume\": \"{AuthToken}\"}}]}}";
                    await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(loginMessage)), WebSocketMessageType.Binary, true, cancellationToken);
                }
                else
                {
                    string encryptedPassword = SHA256Encrypt(password);
                    string loginMessage = $"{{\"msg\": \"method\",\"method\": \"login\",\"id\":\"{uniqueID}\"," +
                        $"\"params\":[{{\"user\": {{ \"username\": \"{username}\" }},\"password\": {{\"digest\": \"{encryptedPassword}\",\"algorithm\":\"sha-256\"}}}}]}}";
                    await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(loginMessage)), WebSocketMessageType.Binary, true, cancellationToken);
                }
                isWebSocketLoggedIn = true;
                var message = new byte[1024];
                await socket.ReceiveAsync(new ArraySegment<byte>(message), new CancellationToken());
                var messageString = Encoding.UTF8.GetString(message);
                Debug.Log(messageString);
            }
            else
            {
                i5Debug.LogWarning("Web socket is already logged in", this);
            }
        }
        #endregion
    }
}
