using i5.Toolkit.Core.RocketChatClient;
using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClientBootstrapper : BaseServiceBootstrapper
{

    public GameObject DemoCanvas;
    public string HostAddress = "";
    public string Username = "";
    public string Password = "";
    
    private RocketChatService client;
    private TMP_InputField hostAddress;
    private TMP_InputField username;
    private TMP_InputField password;
    private Button login;
    private Button getChannelList;
    private Button getGroupList;
    private TMP_InputField roomID;
    private TMP_InputField messageToPost;
    private Button postMessage;
    private Button subscribe;
    private Button unsubscribe;

    // Can be freely chosen.
    private string subscribtionUniqueID = "1";

    void Awake()
    {
        hostAddress = DemoCanvas.transform.Find("HostAddress").GetComponent<TMP_InputField>();
        username = DemoCanvas.transform.Find("Username").GetComponent<TMP_InputField>();
        password = DemoCanvas.transform.Find("Password").GetComponent<TMP_InputField>();
        login = DemoCanvas.transform.Find("Login").GetComponent<Button>();
        getChannelList = DemoCanvas.transform.Find("GetChannelListJoined").GetComponent<Button>();
        getGroupList = DemoCanvas.transform.Find("GetGroupList").GetComponent<Button>();
        roomID = DemoCanvas.transform.Find("RoomID").GetComponent<TMP_InputField>();
        messageToPost = DemoCanvas.transform.Find("MessageToPost").GetComponent<TMP_InputField>();
        postMessage = DemoCanvas.transform.Find("PostMessage").GetComponent<Button>();
        subscribe = DemoCanvas.transform.Find("Subscribe").GetComponent<Button>();
        unsubscribe = DemoCanvas.transform.Find("Unsubscribe").GetComponent<Button>();


        login.onClick.AddListener(LoginAsync);
        getChannelList.onClick.AddListener(GetChannelListJoinedAsync);
        getGroupList.onClick.AddListener(GetGroupListAsync);
        postMessage.onClick.AddListener(PostMessageAsync);
        subscribe.onClick.AddListener(SubscribeAsync);
        unsubscribe.onClick.AddListener(UnsubscribeAsync);
        if (HostAddress != "")
        {
            hostAddress.text = HostAddress;
        }
        if (Username != "")
        {
            username.text = Username;
        }
        if (Password != "")
        {
            password.text = Password;
        }
    }

    protected override void RegisterServices()
    {
        InstantiateClient();
    }

    protected override void UnRegisterServices()
    {
        if (ServiceManager.ServiceExists<RocketChatService>())
        {
            ServiceManager.RemoveService<RocketChatService>();
        }
    }

    //private void Start()
    //{
        
    //}

    public async void LoginAsync()
    {
        WebResponse<LoginResponse> response = await client.LoginAsync(username.text, password.text);
        if (response.Successful && response.Content.Successful)
        {
            i5Debug.Log("Login was successful", this);
        }
        else
        {
            if (!response.Successful)
            {
                i5Debug.Log("Login request failed: " + response.ErrorMessage, this);
            }
            else if (!response.Content.Successful)
            {
                i5Debug.Log("Login failed: " + response.Content.status, this);
            }
            else
            {
                i5Debug.Log("Login failed", this);
            }
        }
    }

    public async void GetChannelListJoinedAsync()
    {
        WebResponse<ChannelGroup[]> response = await client.GetChannelListJoinedAsync();
        if (!response.Successful)
        {
            i5Debug.LogError("Could not get joined channel list: " + response.ErrorMessage, this);
            return;
        }
        ChannelGroup[] joinedChannels = response.Content;
        string result = "";
        for (int i=0;i<joinedChannels.Length;i++)
        {
            result += $"{joinedChannels[i].name} ({joinedChannels[i]._id})\n";
        }
        i5Debug.Log("Joined channels: \n" + result, this);
    }

    public async void GetGroupListAsync()
    {
        WebResponse<ChannelGroup[]> response = await client.GetGroupListAsync();
        if(!response.Successful)
        {
            i5Debug.LogError("Could not get groups: " + response.ErrorMessage, this);
            return;
        }
        ChannelGroup[] joinedGroups = response.Content;
        string result = "";
        for (int i = 0; i < joinedGroups.Length; i++)
        {
            result += $"{joinedGroups[i].name} ({joinedGroups[i]._id})\n";
        }
        i5Debug.Log("Joined groups: \n" + result, this);
    }

    public async void PostMessageAsync()
    {
        WebResponse<MessageSentResponse> response = await client.PostMessageAsync(roomID.text, messageToPost.text);
        if (response.Successful && response.Content.success)
        {
            i5Debug.Log("Message was successfully sent", this);
        }
        else
        {
            i5Debug.LogError("Error sending message: " + response.ErrorMessage, this);
        }
    }

    public async void SubscribeAsync()
    {
        await client.SubscribeRoomMessageAsync(roomID.text, subscribtionUniqueID);
    }

    public async void UnsubscribeAsync()
    {
        await client.UnsubscribeRoomMessageAsync(subscribtionUniqueID);
    }

    private void InstantiateClient()
    {
        client = new RocketChatService(hostAddress.text);
        client.OnMessageReceived += messageArgs => i5Debug.Log($"Message Received by {messageArgs.Sender.name}: {messageArgs.MessageContent}", this);
        ServiceManager.RegisterService(client);
    }
}
