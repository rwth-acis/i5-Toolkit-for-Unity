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

    public class RocketChatClient : IService
    {

        public enum RequestType
        {
            GET,
            POST
        }

        #region Fields and Properties

        private string hostAddress = "";
        private CancellationTokenSource subscribeCancellationTokenSource = new CancellationTokenSource();
        private string username = "";
        private string password = "";
        private string authToken = "";
        private string userID = "";

        // WebSocket
        private ClientWebSocket socket = new ClientWebSocket();
        private CancellationToken cancellationToken;
        private bool isWebSocketConnected = false;
        private bool isWebSocketLoggedIn = false;
        private bool isWebSocketSubscribed = false;

        public string HostAddress
        {
            get => hostAddress;
            private set => hostAddress = value;
        }

        public delegate void ReceivedMessageHandler(string message);

        /// <summary>
        /// Fired when the client receives a WebSocket message from a room.
        /// </summary>
        public event ReceivedMessageHandler OnMessageReceived;

        /// <summary>
        /// The username of the user who accesses the RocketChat API
        /// Will not be automatically set even if one logs in with a token because the password is encrypted.
        /// </summary>
        public string Username
        {
            get => username;
        }

        /// <summary>
        /// The password in plain text.
        /// Will not be automatically set even if one logs in with a token, because it is returned encrypted by the server.
        /// </summary>
        public string Password
        {
            get => password;
        }

        /// <summary>
        /// AuthToken (X-Auth-Token) of the user. It can be automatically set if one login with username first.
        /// </summary>
        public string AuthToken
        {
            get => authToken;
        }

        /// <summary>
        /// UserID (X-User-Id) of the user. It can be automatically set if one login with username first.
        /// </summary>
        public string UserID
        {
            get => userID;
        }

        public IJsonSerializer JsonSerializer { get; set; } = new JsonUtilityAdapter();

        #endregion

        #region IService Implementation
        public void Initialize(IServiceManager owner)
        {
            i5Debug.Log("RocketChatClient host address: " + hostAddress, this);
            i5Debug.Log("RocketChatClient username: " + username, this);
            i5Debug.Log("RocketChatClient authToken: " + authToken, this);
            if (string.IsNullOrEmpty(hostAddress))
            {
                i5Debug.LogError("Please use the contructor to create the RocketChatClient", this);
            }
        }

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
        /// Creates a new instance of the RocketChat client
        /// </summary>
        /// <param name="hostAddress">The address where the RocketChat server is hosted</param>
        /// <param name="username">The name of the user who should be logged in</param>
        /// <param name="password">The password of the user who should be logged in</param>
        public RocketChatClient(string hostAddress, string username, string password)
        {
            cancellationToken = subscribeCancellationTokenSource.Token;
            this.hostAddress = hostAddress;
            this.username = username;
            this.password = password;
        }

        /// <summary>
        /// Creates a new instance of the RocketChat client
        /// </summary>
        /// <param name="hostAddress">The address where the RocketChat server is hosted</param>
        /// <param name="authToken">The auth token for accessing the API with this user</param>
        public RocketChatClient(string hostAddress, string authToken)
        {
            cancellationToken = subscribeCancellationTokenSource.Token;
            this.hostAddress = hostAddress;
            this.authToken = authToken;
        }

        /// <summary>
        /// Login to the server.
        /// It prefers the AuthToken. If it is not given, it will use username and password, and set the AuthToken and UserID.
        /// See https://developer.rocket.chat/reference/api/rest-api/endpoints/other-important-endpoints/authentication-endpoints/login
        /// </summary>
        /// <returns>Returns true if the login was successful, otherwise false</returns>
        public async Task<bool> LoginAsync()
        {
            string payload;

            // construct the payload by checking whether an authToken is given or whether credentials are given
            if (string.IsNullOrEmpty(authToken))
            {
                payload = $"{{ \"username\": \"{username}\", \"password\": \"{password}\" }}";
            }
            else
            {
                payload = $"{{\"resume\": \"{authToken}\"}}";
            }

            WebResponse<string> response = await SendEncodedPostRequestAsync(
                $"https://{hostAddress}/api/v1/login",
                payload,
                false);

            if (!response.Successful)
            {
                i5Debug.LogError("Could not log in", this);
                return false;
            }

            LoginResponse loginResponse = JsonSerializer.FromJson<LoginResponse>(response.Content);

            userID = loginResponse.data.userId;
            authToken = loginResponse.data.authToken;
            return true;
        }

        /// <summary>
        /// Post a message to a given room (channel, team, direct message etc.) of the user. Requires Login first.
        /// See https://developer.rocket.chat/reference/api/rest-api/endpoints/team-collaboration-endpoints/chat-endpoints/postmessage
        /// </summary>
        /// <param name="targetID">rid of the room, channel name (#) or user name (@)</param>
        /// <returns>Returns true if the message was successfully sent</returns>
        public async Task<WebResponse<MessageSentResponse>> PostMessageAsync(string targetID, string text = "", string alias = "", string emoji = "", string avatar = "", string attachement = "")
        {
            WebResponse<string> response = await SendEncodedPostRequestAsync($"https://{HostAddress}/api/v1/chat.postMessage", $"{{ \"channel\": \"{targetID}\", \"text\": \"{text}\" }}", true);
            if (!response.Successful)
            {
                i5Debug.LogError("Could not send message", this);
                return new WebResponse<MessageSentResponse>(response.Content, response.Code);
            }
            MessageSentResponse messageSentResponse = JsonSerializer.FromJson<MessageSentResponse>(response.Content);
            return new WebResponse<MessageSentResponse>(messageSentResponse, response.ByteData, response.Code);
        }

        /// <summary>
        /// Get the user profile. Requires login first.
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
        public async Task<WebResponse<ChannelGroup[]>> GetChannelListJoinedAsync()
        {
            WebResponse<string> response = await SendHttpRequestAsync(RequestType.GET, "/api/v1/channels.list.joined");
            if (!response.Successful)
            {
                i5Debug.LogError("Could not retrieve channels", this);
                return new WebResponse<ChannelGroup[]>(response.Content, response.Code);
            }
            ChannelsJoinedResponse channelsJoined = JsonSerializer.FromJson<ChannelsJoinedResponse>(response.Content);
            return new WebResponse<ChannelGroup[]>(channelsJoined.channels, response.ByteData, response.Code);
        }

        public async Task<WebResponse<ChannelGroup[]>> GetGroupListAsync()
        {
            WebResponse<string> response = await SendHttpRequestAsync(RequestType.GET, "/api/v1/groups.list");
            if (!response.Successful)
            {
                i5Debug.LogError("Could not retrieve groups", this);
                return new WebResponse<ChannelGroup[]>(response.Content, response.Code);
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
        /// <returns></returns>
        public async Task<WebResponse<string>> SendHttpRequestAsync(RequestType type, string apiSuffix, string payload = "")
        {
            if (type == RequestType.GET)
            {
                using (UnityWebRequest request = UnityWebRequest.Get($"https://{HostAddress}{apiSuffix}"))
                {
                    request.SetRequestHeader("X-Auth-Token", authToken);
                    request.SetRequestHeader("X-User-Id", userID);
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
        /// Stream the message of the given room.
        /// See https://developer.rocket.chat/reference/api/realtime-api/subscriptions/stream-room-messages
        /// </summary>
        /// <param name="roomID">rid of the room</param>
        /// <param name="uniqueID">a unique ID of this subscription</param>
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
        /// Unsubscribe the messages of a room, given the ID of the former subscription.
        /// </summary>
        /// <param name="uniqueID">The subscribtion ID</param>
        public async Task UnsubscribeRoomMessageAsync(string uniqueID)
        {
            if (isWebSocketSubscribed)
            {
                string unSubMessage = $"{{\"msg\": \"unsub\",\"id\": \"{uniqueID}\"}}";
                await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(unSubMessage)), WebSocketMessageType.Binary, true, cancellationToken);
                isWebSocketSubscribed = false;
                Debug.Log("Unsubscribed stream with id: " + uniqueID);
            }
            else
            {
                Debug.LogError("You have no subscribtion.");
            }
        }

        /// <summary>
        /// Send a arbitrary WebSocket request to the host.
        /// APIs See https://developer.rocket.chat/reference/api/realtime-api
        /// </summary>
        /// <param name="uniqueID">id of the request</param>
        /// <param name="message">message to send</param>
        /// <returns></returns>
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
        //Encrypt a string using SHA256
        private string SHA256Encrypt(string data)
        {
            byte[] SHA256 = Encoding.UTF8.GetBytes(data);
            SHA256Managed encryptor = new SHA256Managed();
            byte[] hash = encryptor.ComputeHash(SHA256);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

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
                    if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(authToken))
                    {
                        i5Debug.LogError("Cannot post a logged in request if the userID oder authToken are not set.", this);
                        return new WebResponse<string>("not logged in", -1);
                    }
                    request.SetRequestHeader("X-User-Id", userID);
                    request.SetRequestHeader("X-Auth-Token", authToken);
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
                Debug.Log(messageString);
                if (messageString.IndexOf("ping") != -1)
                {
                    string pongMessage = "{\"msg\": \"pong\"}";
                    await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(pongMessage)), WebSocketMessageType.Binary, true, cancellationToken);
                }
                else
                {
                    string[] strs = messageString.Split('\"');
                    if (strs[7] == "stream-room-messages")
                    {
                        if (OnMessageReceived != null)
                        {
                            OnMessageReceived(messageString);
                        }
                    }
                }
            }
            Debug.Log("Subscription stream closed");
        }

        // Connect the socket to the host.
        private async Task WebSocketConnectAsync()
        {
            if (!isWebSocketConnected)
            {
                Uri uri = new Uri($"wss://{hostAddress}/websocket");
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
                if (!string.IsNullOrEmpty(authToken))
                {
                    string loginMessage = $"{{\"msg\": \"method\",\"method\": \"login\",\"id\":\"{uniqueID}\"," + $"\"params\":[{{\"resume\": \"{authToken}\"}}]}}";
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
