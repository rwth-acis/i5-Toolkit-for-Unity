using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Net.WebSockets;
using System.Threading;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities.Async;
using i5.Toolkit.Core.Utilities;

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

        //WebSocket
        private ClientWebSocket socket = new ClientWebSocket();
        private CancellationToken cancellationToken;
        private bool isWebSocketConnected = false;
        private bool isWebSocketLogined = false;
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
        /// Username. Will not be automatically set even if one login with token, because the password is encrypted.
        /// </summary>
        public string Username
        {
            get => username;
        }

        /// <summary>
        /// Password in plain text. Will not be automatically set even if one login with token, because it is returned encrypted by the server.
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

        #endregion

        #region IService Implementation
        public void Initialize(IServiceManager owner)
        {
            Debug.Log("RocketChatClient host address: " + hostAddress);
            Debug.Log("RocketChatClient username: " + username);
            Debug.Log("RocketChatClient authToken: " + authToken);
            if (hostAddress == "")
            {
                Debug.LogError("Please use the contructor to create the RocketChatClient");
            }
        }

        public void Cleanup()
        {
            isWebSocketConnected = false;
            isWebSocketLogined = false;
            isWebSocketSubscribed = false;
            subscribeCancellationTokenSource.Cancel();
        }
        #endregion

        #region Public Methods

        public RocketChatClient(string hostAddress, string username, string password)
        {
            cancellationToken = subscribeCancellationTokenSource.Token;
            this.hostAddress = hostAddress;
            this.username = username;
            this.password = password;
        }

        public RocketChatClient(string hostAddress, string authToken)
        {
            cancellationToken = subscribeCancellationTokenSource.Token;
            this.hostAddress = hostAddress;
            this.authToken = authToken;
        }

        /// <summary>
        /// Login to the server, it prefer using AuthToken. If it is not given, it will use username and password, and set the AuthToken and UserID.
        /// See https://developer.rocket.chat/reference/api/rest-api/endpoints/other-important-endpoints/authentication-endpoints/login
        /// </summary>
        public async Task<WebResponse<string>> LoginAsync()
        {
            if (authToken == "")
            {
                using (UnityWebRequest request = UnityWebRequest.Put($"https://{hostAddress}/api/v1/login", $"{{ \"username\": \"{username}\", \"password\": \"{password}\" }}"))
                {
                    request.method = "POST";
                    request.SetRequestHeader("Content-type", "application/json");
                    await request.SendWebRequest();
                    if (request.error != null)
                    {
                        Debug.Log("Login: " + request.error);
                        return new WebResponse<string>(request.downloadHandler.text, request.responseCode);
                    }
                    else
                    {
                        Debug.Log("Login: Success");
                        //If the authToken and userID is not set yet, we set them here.
                        string[] strs = request.downloadHandler.text.Split('"');
                        if (userID == "")
                        {
                            userID = strs[9];
                        }
                        if (authToken == "")
                        {
                            authToken = strs[13];
                        }
                        Debug.Log($"userID is set to {userID}");
                        Debug.Log($"authToken is set to {authToken}");
                        return new WebResponse<string>(request.downloadHandler.text, request.downloadHandler.data, request.responseCode);
                    }
                }               
            }
            else
            {
                using (UnityWebRequest request = UnityWebRequest.Put($"https://{HostAddress}/api/v1/login", $"{{\"resume\": \"{authToken}\"}}"))
                {
                    request.method = "POST";
                    request.SetRequestHeader("Content-type", "application/json");
                    await request.SendWebRequest();
                    if (request.error != null)
                    {
                        Debug.Log("Login: " + request.error);
                        return new WebResponse<string>(request.downloadHandler.text, request.responseCode);
                    }
                    else
                    {
                        Debug.Log("Login: Success");
                        string[] strs = request.downloadHandler.text.Split('"');
                        if (userID == "")
                        {
                            userID = strs[9];
                        }
                        Debug.Log($"userID is set to {userID}");
                        Debug.Log($"authToken is set to {authToken}");
                        return new WebResponse<string>(request.downloadHandler.text, request.downloadHandler.data, request.responseCode);
                    }
                }               
            }
        }

        /// <summary>
        /// Post a message to a given room (channel, team, direct message etc.) of the user. Requires Login first.
        /// See https://developer.rocket.chat/reference/api/rest-api/endpoints/team-collaboration-endpoints/chat-endpoints/postmessage
        /// </summary>
        /// <param name="targetID">rid of the room, channel name (#) or user name (@)</param>
        public async Task<WebResponse<string>> PostMessageAsync(string targetID, string text = "", string alias = "", string emoji = "", string avatar = "", string attachement = "")
        {   
            using(UnityWebRequest request = UnityWebRequest.Put($"https://{HostAddress}/api/v1/chat.postMessage", $"{{ \"channel\": \"{targetID}\", \"text\": \"{text}\" }}"))
            {
                request.method = "POST";
                request.SetRequestHeader("X-Auth-Token", authToken);
                request.SetRequestHeader("X-User-Id", userID);
                request.SetRequestHeader("Content-type", "application/json");
                request.uploadHandler.contentType = "application/json";
                await request.SendWebRequest();
                if (request.error != null)
                {
                    Debug.Log("Post Message: " + request.error);
                    Debug.Log(request.downloadHandler.text);
                    return new WebResponse<string>(request.downloadHandler.text, request.responseCode);
                }
                else
                {
                    Debug.Log("Post Message: Success");
                    Debug.Log(request.downloadHandler.text);
                    return new WebResponse<string>(request.downloadHandler.text, request.downloadHandler.data, request.responseCode);
                }                
            }
        }

        /// <summary>
        /// Get the user profile. Requires Login first.
        /// See https://developer.rocket.chat/reference/api/rest-api/endpoints/other-important-endpoints/authentication-endpoints/me
        /// </summary>
        public async Task<WebResponse<string>> GetMeAsync()
        {
            using(UnityWebRequest request = UnityWebRequest.Get($"https://{HostAddress}/api/v1/me"))
            {
                request.SetRequestHeader("X-Auth-Token", authToken);
                request.SetRequestHeader("X-User-Id", userID);
                await request.SendWebRequest();
                if (request.error != null)
                {
                    Debug.Log("Me: " + request.error);
                    Debug.Log(request.downloadHandler.text);
                    return new WebResponse<string>(request.downloadHandler.text, request.responseCode);
                }
                else
                {
                    Debug.Log("Me: Success");
                    Debug.Log(request.downloadHandler.text);
                    return new WebResponse<string>(request.downloadHandler.text, request.downloadHandler.data, request.responseCode);
                }
            }
        }

        /// <summary>
        /// Get the user's channel list of joined channels. Requires Login first.
        /// See https://developer.rocket.chat/reference/api/rest-api/endpoints/team-collaboration-endpoints/channels-endpoints/list
        /// </summary>
        public async Task<WebResponse<string>> GetChannelListJoinedAsync()
        {
            using( UnityWebRequest request = UnityWebRequest.Get($"https://{HostAddress}/api/v1/channels.list.joined"))
            {
                request.SetRequestHeader("X-Auth-Token", authToken);
                request.SetRequestHeader("X-User-Id", userID);
                await request.SendWebRequest();
                if (request.error != null)
                {
                    Debug.Log("Get Channel List: " + request.error);
                    Debug.Log(request.downloadHandler.text);
                    return new WebResponse<string>(request.downloadHandler.text, request.responseCode);
                }
                else
                {
                    Debug.Log("Get Channel List: Success");
                    Debug.Log(request.downloadHandler.text);
                    return new WebResponse<string>(request.downloadHandler.text, request.downloadHandler.data, request.responseCode);
                }
            }
        }

        /// <summary>
        /// Send a arbitrary HTTP request to the host. Support only GET and POST with application/json header.
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
                    if (request.error != null)
                    {
                        Debug.Log("Http Get Request: " + request.error);
                        Debug.Log(request.downloadHandler.text);
                        return new WebResponse<string>(request.downloadHandler.text, request.responseCode);
                    }
                    else
                    {
                        Debug.Log("Http Get Request: Success");
                        Debug.Log(request.downloadHandler.text);
                        return new WebResponse<string>(request.downloadHandler.text, request.downloadHandler.data, request.responseCode);
                    }
                }
            }
            else
            {
                using (UnityWebRequest request = UnityWebRequest.Put($"https://{HostAddress}{apiSuffix}", payload))
                {
                    request.method = "POST";
                    request.SetRequestHeader("X-Auth-Token", authToken);
                    request.SetRequestHeader("X-User-Id", userID);
                    request.SetRequestHeader("Content-type", "application/json");
                    await request.SendWebRequest();
                    if (request.error != null)
                    {
                        Debug.Log("Http Post Request: " + request.error);
                        Debug.Log(request.downloadHandler.text);
                        return new WebResponse<string>(request.downloadHandler.text, request.responseCode);
                    }
                    else
                    {
                        Debug.Log("Http Post Request: Success");
                        Debug.Log(request.downloadHandler.text);
                        return new WebResponse<string>(request.downloadHandler.text, request.downloadHandler.data, request.responseCode);
                    }
                }
            }
        }

        /// <summary>
        /// Stream the message of the given room.
        /// See https://developer.rocket.chat/reference/api/realtime-api/subscriptions/stream-room-messages
        /// </summary>
        /// <param name="roomID">rid of the room</param>
        /// <param name="uniqueID">a unique ID of this subscribtion</param>
        public async Task SubscribeRoomMessageAsync(string roomID, string uniqueID)
        {
            await WebSocketConnectAsync();
            await WebSocketLoginAsync(uniqueID);
            //Subscribe
            string subsribeRequest = $"{{\"msg\": \"sub\",\"id\": \"{uniqueID}\",\"name\": \"stream-room-messages\",\"params\":[\"{roomID}\",false]}}";
            await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(subsribeRequest)), WebSocketMessageType.Binary, true, cancellationToken);
            isWebSocketSubscribed = true;
            //ReceiveMessage
            StreamMessageAsync();
            Debug.Log("Subscribtion stream opened.");
        }

        /// <summary>
        /// Unsubscribe the messages of a room, given the ID of the former subscribtion.
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
        public async Task SendWebSocketRequestAsync(string uniqueID, string message)
        {
            await WebSocketConnectAsync();
            await WebSocketLoginAsync(uniqueID);
            await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)), WebSocketMessageType.Binary, true, cancellationToken);
            var response = new byte[1024];
            await socket.ReceiveAsync(new ArraySegment<byte>(response), new CancellationToken());
            var messageString = Encoding.UTF8.GetString(response);
            Debug.Log(messageString);
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

        //Stream the message in an async thread
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
                    if(strs[7] == "stream-room-messages")
                    {
                        if (OnMessageReceived != null)
                        {
                            OnMessageReceived(messageString);
                        }
                    }
                }
            }
            Debug.Log("Subscribtion stream closed");
        }

        //Connect the socket to the host.
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
        }

        //Login to the host
        private async Task WebSocketLoginAsync(string uniqueID)
        {
            if (!isWebSocketLogined)
            {
                if (authToken != "")
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
                isWebSocketLogined = true;
                var message = new byte[1024];
                await socket.ReceiveAsync(new ArraySegment<byte>(message), new CancellationToken());
                var messageString = Encoding.UTF8.GetString(message);
                Debug.Log(messageString);
            }
        }
        #endregion
    }
}
